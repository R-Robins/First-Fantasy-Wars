using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Use this for initialization
    public float ScrollSpeed = 2000.0f;
    private Vector3 Anchor;
    private bool rightMouseButtonDown = false;
    private bool middleMouseButtonDown = false;
    private float turnAround = 1.0f;
    private float upsideDown;
    public float rotationSpeed = 100.0f;
    void Start()
    {
        upsideDown = Mathf.Abs(transform.position.z) / transform.position.z;
        Anchor = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float scrollUnit = Input.GetAxis("Mouse ScrollWheel") * ScrollSpeed;
        float rotationAxis = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        //Debug.Log("Scroll unit is " + scrollUnit);
        transform.Translate(new Vector3(0, 0, scrollUnit * Time.deltaTime), Space.Self);
        if (transform.position.y < 25.0f || transform.position.y > 150.0f)
        {
            transform.Translate(new Vector3(0, 0, -scrollUnit * Time.deltaTime), Space.Self);
        }
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseButtonDown = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            rightMouseButtonDown = false;
        }
        if (rightMouseButtonDown)
        {
            if (rotationAxis != 0)
            {
                Vector3 delta = (transform.position - Anchor);
                float magnitude = Mathf.Sqrt(Mathf.Pow(delta.x, 2.0f) + Mathf.Pow(delta.z, 2.0f));
                rotationAxis = rotationAxis * turnAround;
                float newX = delta.x + rotationAxis;

                if (Mathf.Abs(newX) > magnitude)
                {
                    turnAround = -1.0f * turnAround;
                    upsideDown = -1.0f * upsideDown;
                    return;
                }

                float angle = Mathf.Acos(newX / magnitude);
                float z = Mathf.Sin(angle) * upsideDown;
                transform.LookAt(Anchor);
                float y = z * magnitude;
                transform.Translate(new Vector3(rotationAxis, 0, y - delta.z), Space.World);
            }
            return;
        }
        if (Input.GetMouseButtonDown(2))
        {
            middleMouseButtonDown = true;
        }
        if (Input.GetMouseButtonUp(2))
        {
            middleMouseButtonDown = false;
        }
        if (middleMouseButtonDown)
        {
            float moveY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;
            Vector3 moveOnPlane = Vector3.ClampMagnitude(new Vector3(rotationAxis, moveY, 0.0f), rotationSpeed);
            transform.Translate(moveOnPlane, Space.Self);
            Anchor += moveOnPlane;
            if (transform.position.y < 25.0f || transform.position.y > 150.0f)
            {
                transform.Translate(new Vector3(0, moveOnPlane.y, 0), Space.Self);
                Anchor.y -= moveOnPlane.y;
            }
        }
    }
}
