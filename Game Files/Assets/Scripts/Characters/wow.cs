using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wow : MonoBehaviour {
    Animator animator;
   
    // Use this for initialization
    public bool hasAttacked = false;
    public bool hasDied = false;
    public bool Alert = false;
    public bool Dashing = false;
	void Start () {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey("space"))
        {
            if (!hasAttacked)
            {
                animator.SetTrigger("IsAttacking");
                //animator.SetBool("IsIdle", false);
                hasAttacked = true;
                animator.SetBool("IsIdle", true);
            }
            else
            {
                hasAttacked = false;
            }
        }

        if (Input.GetKey("q"))
        {
            if (!hasAttacked)
            {
                animator.SetTrigger("IsAttacking1");
                //animator.SetBool("IsIdle", false);
                hasAttacked = true;
                animator.SetBool("IsIdle", true);
            }
            else
            {
                hasAttacked = false;
            }
        }

        if (Input.GetKey("w"))
        {
            if (!hasAttacked)
            {
                animator.SetTrigger("IsAttacking2");
                //animator.SetBool("IsIdle", false);
                hasAttacked = true;
                animator.SetBool("IsIdle", true);
            }
            else
            {
                hasAttacked = false;
            }
        }

        if (Input.GetKey("e"))
        {
            if (!Alert)
            {
                animator.SetBool("IsALert", true);
                //animator.SetBool("IsIdle", false);
                Alert = true;

            }
            else
            {
                Alert = false;
            }
        }

        if (Input.GetKey("r"))
        {
            if (!hasAttacked)
            {
                animator.SetTrigger("IsBlocking");
                //animator.SetBool("IsIdle", false);
                hasAttacked = true;
                animator.SetBool("IsIdle", true);
            }
            else
            {
                hasAttacked = false;
            }
        }

        if (Input.GetKey("d"))
        {
            if (!hasDied)
            {
                animator.SetTrigger("Die2");
                //animator.SetBool("IsIdle", false);
                hasDied = true;
            }
        }

        if (Input.GetKey("x"))
        {
            if (!hasAttacked)
            {
                animator.SetTrigger("Win");
                //animator.SetBool("IsIdle", false);
                hasAttacked = true;
                animator.SetBool("IsIdle", true);
            }
            else
            {
                hasAttacked = false;
            }
        }

        if (Input.GetKey("g"))
        {
            if (!Dashing)
            {
                animator.SetBool("IsMoving", true);
                Dashing = true;
                animator.SetBool("IsIdle", false);
            }
            else
            {
                Dashing = false;
                animator.SetBool("IsMoving", false);
                animator.SetBool("IsIdle", true);
            }
        } 
    }
}
