using UnityEngine;
using System.Collections;

//Unity Asset Desc
//collider cliffStart tag== cliffStart
//collider cliffEnd tag == cliffEnd

//Script Attached to player

public class cliffDeath : MonoBehaviour {

    private bool cliffStart;
    private bool cliffEnd;


    void Start()
    {
        cliffEnd = false;
        cliffStart = false;
    }
    
	void OnTriggerExit(Collider other)
    {

        if (other.tag == "cliffStart")
        {
            cliffStart = true;
            StartCoroutine("cliffFallOffCheck");
            cliffFallOffCheck();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cliffEnd")
        {
            Debug.Log("Entering clifend: " + other.name + gameObject.name);
            cliffEnd = true;
            StartCoroutine("cliffFallOffCheck");
            if (cliffStart)
            {
                GetComponent <Rigidbody>().isKinematic = true;
                GetComponent<FirstPersonController>().PauseCharacter();
                GetComponent<movementModifier>().isCurrentlyDead = true;
                GetComponent<ChantBehavior>().CliffDeath();
                GetComponent<AnimationManager>().isDead(true);           
             }
        }

    }

    //Check if character falls off the cliff
    IEnumerator cliffFallOffCheck()
    {
        yield return new WaitForSeconds(GameStats.TimeBeforeRespawn);
        if (cliffStart && cliffEnd)
        {
            GetComponent<FirstPersonController>().UnpauseCharacter();
            GetComponent<movementModifier>().resetEverything();
        }
        else
         cliffEnd = false;
         cliffStart = false;
        }
    }
