using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class guid
        {
            private int num = 0;
            public static guid instance = new guid();

            public int sn()
            {
                lock (this)
                {
                    return num++;
                }
            }
        }

        // a helper class for raw request
        public class request
        {
            public int sn = 0;
            private int pos = 0;
            private byte[] buf = new byte[4096];

            public request(short cmd)
            {
                sn = guid.instance.sn();
                pos = 2; // reserve two bytes for message length
                encode(cmd);
                encode(sn);
            }

            public void clear()
            {
                pos = 0;
            }

            public request encode(byte value)
            {
                if (pos + 1 > buf.Length)
                    throw new Exception("too long request");

                buf[pos++] = value;
                return this;
            }

            public request encode(short value)
            {
                if (pos + 2 > buf.Length)
                    throw new Exception("too long request");

                buf[pos++] = (byte)(value & 0xff);
                buf[pos++] = (byte)((value >> 8) & 0xff);
                return this;
            }

            public request encode(int value)
            {
                if (pos + 4 > buf.Length)
                    throw new Exception("too long request");

                buf[pos++] = (byte)(value & 0xff);
                buf[pos++] = (byte)((value >> 8) & 0xff);
                buf[pos++] = (byte)((value >> 16) & 0xff);
                buf[pos++] = (byte)((value >> 24) & 0xff);
                return this;
            }

            public request encode(long value)
            {
                if (pos + 8 > buf.Length)
                    throw new Exception("too long request");

                buf[pos++] = (byte)(value & 0xff);
                buf[pos++] = (byte)((value >> 8) & 0xff);
                buf[pos++] = (byte)((value >> 16) & 0xff);
                buf[pos++] = (byte)((value >> 24) & 0xff);
                buf[pos++] = (byte)((value >> 32) & 0xff);
                buf[pos++] = (byte)((value >> 40) & 0xff);
                buf[pos++] = (byte)((value >> 48) & 0xff);
                buf[pos++] = (byte)((value >> 56) & 0xff);
                return this;
            }

            public request encode(string value)
            {
                short len = (short)value.Length;
                encode(len);

                byte[] bytes = Encoding.UTF8.GetBytes(value);
                if (pos + len > buf.Length)
                    throw new Exception("too long request");

                Array.Copy(bytes, 0, buf, pos, len);
                pos += len;
                return this;
            }

            public bool send(client c)
            {
                buf[0] = (byte)((pos - 2) & 0xff);
                buf[1] = (byte)(((pos - 2) >> 8) & 0xff);
                return c.send(buf, 0, pos);
            }

            public response decode()
            {
                buf[0] = (byte)((pos - 2) & 0xff);
                buf[1] = (byte)(((pos - 2) >> 8) & 0xff);
                return new response(buf, 2, pos);
            }
        }

        // a helper class for raw response
        public class response
        {
            private int pos, len;
            private byte[] buf = null;
            public short cmd;
            public int sn;

            public response(byte[] buf, int offset, int length)
            {
                this.buf = buf;
                this.pos = offset;
                this.len = length;

                cmd = nextShort();
                sn = nextInt();
            }

            public byte nextByte()
            {
                if (pos + 1 > len)
                    throw new Exception("decode out of range");

                return buf[pos++];
            }

            public short nextShort()
            {
                if (pos + 2 > len)
                    throw new Exception("decode out of range");

                ushort v0 = (ushort)buf[pos++];
                ushort v1 = (ushort)buf[pos++];
                return (short)((v1 << 8) | v0);
            }

            public int nextInt()
            {
                if (pos + 4 > len)
                    throw new Exception("decode out of range");

                uint v0 = (uint)buf[pos++];
                uint v1 = (uint)buf[pos++];
                uint v2 = (uint)buf[pos++];
                uint v3 = (uint)buf[pos++];
                return (int)((v3 << 24) | (v2 << 16) | (v1 << 8) | v0);
            }


            public long nextLong()
            {
                if (pos + 8 > len)
                    throw new Exception("decode out of range");

                ulong v0 = (ulong)buf[pos++];
                ulong v1 = (ulong)buf[pos++];
                ulong v2 = (ulong)buf[pos++];
                ulong v3 = (ulong)buf[pos++];
                ulong v4 = (ulong)buf[pos++];
                ulong v5 = (ulong)buf[pos++];
                ulong v6 = (ulong)buf[pos++];
                ulong v7 = (ulong)buf[pos++];
                return (long)((v7 << 56) | (v6 << 48) | (v5 << 40) | (v4 << 32) | (v3 << 24) | (v2 << 16) | (v1 << 8) | v0);
            }

            public string nextString()
            {
                short n = nextShort();
                if (pos + n > len)
                    throw new Exception("decode out of range");

                int i = pos;
                pos += n;
                return Encoding.UTF8.GetString(buf, i, n);
            }
        }

        // a tcp client handling message send & recv
        public class client
        {
            private IPAddress ipaddr;
            private int port;
            private Socket server;

            public client(string ip, int port)
            {
                this.ipaddr = IPAddress.Parse(ip);
                this.port = port;
            }

            ~client()
            {
                close();
            }

            // close the tcp connection
            public void close()
            {
                if (server != null)
                {
                    if (server.Connected)
                    {
                        server.Shutdown(SocketShutdown.Both);
                    }
                    server.Close();
                    server = null;
                }
            }

            public bool isConnected()
            {
                return server != null && server.Connected;
            }

            // connect to the server
            public bool connect()
            {
                if (isConnected())
                {
                    close();
                }

                try
                {
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    server.Connect(new IPEndPoint(ipaddr, port));
                    server.NoDelay = true;
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: failed to connect to server: " + e.Message);
                    return false;
                }
            }

            public bool send(byte[] buf, int offset, int length)
            {
                try
                {
                    server.Send(buf, offset, length, SocketFlags.None);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    if (!server.Connected) close();
                    return false;
                }
            }

            // recv enough buffer
            private bool recv(byte[] buf, int len)
            {
                int i = 0;
                while (i < len)
                {
                    int c = server.Receive(buf, i, len - i, SocketFlags.Partial);
                    if (c <= 0) { close(); return false; }
                    i += c;
                }
                return true;
            }

            public response recv()
            {
                try
                {
                    // 1. get the pack length
                    byte[] buf = new byte[2];
                    if (!recv(buf, 2)) return null;
                    short len = (short)((((ushort)buf[1]) << 8) | ((ushort)buf[0]));

                    // 2. get the message data
                    buf = new byte[len];
                    if (!recv(buf, len)) return null;

                    // 3. return the response
                    return new response(buf, 0, len);
                }
                catch (Exception e)
                {
                    Console.WriteLine("WARN: failed to receive messages: " + e.Message);
                    return null;
                }
            }
        }
