﻿using UnityEngine;
using System.Collections;


//attached to the blue char obj
public class AnimationManager : MonoBehaviour {

    public Animator manager;
    public bool reset;
    public bool dead;

	// Use this for initialization
	void Start () {
        reset = false;
        dead = false;
    }
	
    public void isMoving(bool value)
    {
        manager.SetBool("isMoving", value);
    }

    public void isJumping(bool value)
    {
        manager.SetBool("isJumping", value);
    }

    public void isDead(bool value)
    {
        dead = value;
        manager.SetBool("isDead", value);
    }

    public void isGrounded(bool value)
    {
        manager.SetBool("isGrounded", value);
    }

    public void isAttacking(bool value)
    {
        manager.SetBool("isAttacking", value);
    }

    public void isReset(bool value)
    {
        reset = value;
        manager.SetBool("isReset", value);  
    }
}
