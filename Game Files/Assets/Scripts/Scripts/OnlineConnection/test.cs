using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//using Assets.Scripts.Scripts.OnlineConnection.tcp;


// define a class to implement the abstract msghandler
public class User : msghandler
    {
        private session ss = null;
        private string who_first = "";

        private string usr = null;
        private string passwd = null;

        private long commanderId = 0;
        private long[] unitId;
        private long[] abilityId;

        private long actionUnit = 0;
        private int[] actionXY;
        private long actionAbility = -1;

        public User(string ip, int port)
        {
            ss = new session(this, ip, port);
        }

        public void testScript(string usr, string passwd, long commanderId, long[] unitId, long[] abilityId, long actionUnit, int[] actionXY, long actionAbility)
        {
            this.usr = usr;
            this.passwd = passwd;

            this.commanderId = commanderId;

            this.unitId = unitId;
            this.abilityId = abilityId;
            
            this.actionUnit = actionUnit;
            this.actionXY = actionXY;

            this.actionAbility = actionAbility;

            // 1.login
            result r = ss.login(usr, passwd, 10000);
            if (!r.succ)
            {
                Console.WriteLine("ERROR: failed to login: {0}", r.info);
                return;
            }
            else
            {
                Console.WriteLine("{0} login.", usr);
            }

            // 2. quickplay
            ss.quickplay();
        }

        public void stop()
        {
            ss.logout(10000);
            ss.stop();
        }

        // the message proc
        public override void proc(response rsp)
        {
            switch (rsp.cmd)
            {
                case (short)cmd.RSP_QUICKPLAY: // match ok, print the opponent's name
                    {
                        QuickPlayRsp t = new QuickPlayRsp(rsp);
                        Console.WriteLine("quickplay matched, {0}'s opponent is {1}, {2} will play first.", usr, t.opponent, t.who_first);

                        who_first = t.who_first;
                        result r = ss.setCommander(commanderId);
                        if (!r.succ)
                        {
                            Console.WriteLine("ERROR: failed to set commander: {0}", r.info);
                            return;
                        }
                    }
                    break;
                case (short)cmd.RSP_SET_COMMANDER:
                    {
                        SetCommanderRsp t = new SetCommanderRsp(rsp);
                        if (t.succ)
                        {
                            Console.WriteLine("{0} selects commander {1}.", usr, t.commanderId);

                            // after commander selected, select units
                            result r = ss.setUnits(unitId);
                            if (!r.succ)
                            {
                                Console.WriteLine("ERROR: failed to set units: {0}", r.info);
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} fails to select commander {1}, error=[{2}].", usr, t.commanderId, t.info);
                        }
                    }
                    break;
                case (short)cmd.RSP_SET_UNITS:
                    {
                        SetUnitsRsp t = new SetUnitsRsp(rsp);
                        if (t.succ)
                        {
                            Console.WriteLine("{0} selects units({1}, {2}, {3}, {4}, {5}).", usr,
                                unitId[0],
                                unitId[1],
                                unitId[2],
                                unitId[3],
                                unitId[4]);

                            // after units selected, select units' abilities
                            result r = ss.setUnitsAbilities(unitId, abilityId);
                            if (!r.succ)
                            {
                                Console.WriteLine("ERROR: failed to set units' abilities: {0}", r.info);
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} fails to select units({1}:{2}, {3}:{4}, {5}:{6}, {7}:{8}, {9}:{10}), error=[{11}].", usr, 
                                unitId[0], t.check[0],
                                unitId[1], t.check[1],
                                unitId[2], t.check[2],
                                unitId[3], t.check[3],
                                unitId[4], t.check[4],
                                t.info);
                        }
                    }
                    break;
                case (short)cmd.RSP_SET_UNITS_ABILITIES:
                    {
                        SetUnitsAbilitiesRsp t = new SetUnitsAbilitiesRsp(rsp);
                        if (t.succ)
                        {
                            Console.WriteLine("{0} selects units' abilities({1}:{2}, {3}:{4}, {5}:{6}, {7}:{8}, {9}:{10}).", 
                                usr,
                                unitId[0], abilityId[0],
                                unitId[1], abilityId[1],
                                unitId[2], abilityId[2],
                                unitId[3], abilityId[3],
                                unitId[4], abilityId[4]);

                            // game start, lily starts with GameAction
                            if (usr.Equals(who_first))
                            {
                                result r = ss.gameAction(actionUnit, actionXY[0], actionXY[1], actionAbility);
                                if (!r.succ)
                                {
                                    Console.WriteLine("ERROR: failed to move unit: {0}", r.info);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} fails to select units' abilities({1}:{2}:{3}, {4}:{5}:{6}, {7}:{8}:{9}, {10}:{11}:{12}, {13}:{14}:{15}), error=[{16}].",
                                usr,
                                unitId[0], abilityId[0], t.check[0],
                                unitId[1], abilityId[1], t.check[1],
                                unitId[2], abilityId[2], t.check[2],
                                unitId[3], abilityId[3], t.check[3],
                                unitId[4], abilityId[4], t.check[4], t.info);

                        }
                    }
                    break;
                case (short)cmd.RSP_SET_OPPONENT_COMMANDER:
                    {
                        SetOpponentCommanderRsp t = new SetOpponentCommanderRsp(rsp);
                        Console.WriteLine("{0}'s opponent selects commander {1}.", usr, t.commanderId);
                    }
                    break;
                case (short)cmd.RSP_SET_OPPONENT_UNITS:
                    {
                        SetOpponentUnitsRsp t = new SetOpponentUnitsRsp(rsp);
                        Console.WriteLine("{0}'s opponent selects units({1}).", usr, t.ToString());
                    }
                    break;
                case (short)cmd.RSP_SET_OPPONENT_UNITS_ABILITIES:
                    {
                        SetOpponentUnitsAbilitiesRsp t = new SetOpponentUnitsAbilitiesRsp(rsp);
                        Console.WriteLine("{0}'s opponent selects units' ablities({1}).", usr, t.ToString());
                    }
                    break;
                case (short)cmd.RSP_GAME_MOVE:
                    {
                        GameMoveRsp t = new GameMoveRsp(rsp);
                        Console.WriteLine("{0}'s move: result({1}), info({2}).", usr, t.succ, t.info);
                    }
                    break;
                case (short)cmd.RSP_GAME_ACTION:
                    {
                        GameActionRsp t = new GameActionRsp(rsp);
                        Console.WriteLine("{0}'s action: result({1}), info({2}).", usr, t.succ, t.info);
                    }
                    break;
                case (short)cmd.RSP_OPPONENT_GAME_ACTION:
                    {
                        OpponentGameActionRsp t = new OpponentGameActionRsp(rsp);
                        Console.WriteLine("{0}'s opponent move unit({1}) to ({2}, {3}), action=({4}).", usr, t.cur_unit, t.dst_x, t.dst_y, t.ability);

                        // goodman starts after lily, and GameMove
                        result r = ss.gameMove(actionUnit, actionXY[0], actionXY[1]);
                        if (!r.succ)
                        {
                            Console.WriteLine("ERROR: failed to move unit: {0}", r.info);
                            return;
                        }
                    }
                    break;
                case (short)cmd.RSP_OPPONENT_GAME_MOVE:
                    {
                        OpponentGameMoveRsp t = new OpponentGameMoveRsp(rsp);
                        Console.WriteLine("{0}'s opponent move unit({1}) to ({2}, {3}).", usr, t.cur_unit, t.dst_x, t.dst_y);

                        // lily quits after recevied goodman's GameMove
                        result r = ss.quitPlay();
                        if (!r.succ)
                        {
                            Console.WriteLine("ERROR: failed to quit game: {0}", r.info);
                            return;
                        }
                    }
                    break;
                case (short)cmd.RSP_QUITPLAY:
                    {
                        QuitPlayRsp t = new QuitPlayRsp(rsp);
                        Console.WriteLine("{0}'s action: result({1}), info({2}).", usr, t.succ, t.info);
                    }
                    break;
                case (short)cmd.RSP_OPPONENT_QUITPLAY:
                    {
                        Console.WriteLine("{0}'s opponent quits battle.", usr);
                    }
                    break;

            }
        }
    }
    
    // the main test entry
    class test
    {
        static void Main(string[] args)
        {
            // create two players(two threads)
#if false
            user u1 = new user("127.0.0.1", 9252);
            user u2 = new user("127.0.0.1", 9252);
#else
            User u1 = new User("35.197.31.116", 9252);
            User u2 = new User("35.197.31.116", 9252);
#endif

            // run login/quickplay/setCommander/setUnits/setUnitsAbilities/gameAction
            u1.testScript("lily", "123456", 
                1, /* commander */
                new long[] { 1, 2, 3, 2, 1 }, /* units */
                new long[]{1, 8, 13, 8, 1}, /* units abilities */
                3, /* first action: unit */
                new int[]{1, 3}, /* first atcion: x, y */
                2); /* first action: ability */

            u2.testScript("goodman", "good",
                2, /* commander */
                new long[] { 2, 2, 3, 3, 1 },  /* units */
                new long[] { 10, 10, 15, 15, 2 }, /* units abilities */
                4, /* first action: unit */
                new int[]{4, 2}, /* first atcion: x, y */
                4); /* first action: ability */

            // pause for a while, both logout
            Thread.Sleep(10000);

            u1.stop();
            u2.stop();
        }
    }

