using UnityEngine;
using System.Collections;

//Unity Asset Desc
//collider cliffStart tag== cliffStart
//collider cliffEnd tag == cliffEnd

//Script Attached to player

public class cliffDeath : MonoBehaviour {

    public bool cliffStart;
    public bool cliffEnd;

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
            cliffEnd = true;
            StartCoroutine("cliffFallOffCheck");
            if (cliffStart)
            {
                GetComponent<FirstPersonController>().enabled = false;
                GetComponent<movementModifier>().isCurrentlyDead = true;
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
            Debug.Log("cliff death");
            if (GetComponent<ChantBehavior>().chantStatus())
            {
                GameStats.addPointsTeam(1, "blue");
            }
            GetComponent<movementModifier>().resetEverything();
        }
        else
         cliffEnd = false;
         cliffStart = false;
        }
    }
