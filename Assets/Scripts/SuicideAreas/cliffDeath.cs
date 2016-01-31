using UnityEngine;
using System.Collections;

//Unity Asset Desc
//collider cliffStart tag== cliffStart
//collider cliffEnd tag == cliffEnd

//Script Attached to player

public class cliffDeath : MonoBehaviour
{

    private bool cliffStart;
    private bool cliffEnd;
    private Transform deathPos;

    private bool doneDead;

    void Start()
    {
        doneDead = false;
        deathPos = GameObject.Find("Cliff Death").transform;
        cliffEnd = false;
        cliffStart = false;
    }

    void OnTriggerExit(Collider other)
    {

        if (other.tag == "cliffStart")
        {
            cliffStart = true;
            //
        }
    }

    void Update()
    {
        if (cliffEnd)
        {
            if (deathPos.position.y > transform.position.y)
            {
                if (!doneDead)
                    DoDead();
            }
            cliffEnd = false;
        }
    }


    private void DoDead()
    {
        doneDead = true;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<FirstPersonController>().PauseCharacter();
        GetComponent<movementModifier>().isCurrentlyDead = true;
        GetComponent<ChantBehavior>().CliffDeath();
        GetComponent<AnimationManager>().isJumping(false);
        GetComponent<AnimationManager>().isGrounded(true);
        GetComponent<AnimationManager>().isDead(true);
        StartCoroutine(cliffFallOffCheck());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cliffStart")
        {
            GetComponent<AnimationManager>().isJumping(true);
            GetComponent<AnimationManager>().isGrounded(false);
            GetComponent<FirstPersonController>().PauseCharacter();
            Debug.Log("Entering clif " + other.name + gameObject.name);
            cliffEnd = true;
        }
    }

    //Check if character falls off the cliff
    IEnumerator cliffFallOffCheck()
    {
        yield return new WaitForSeconds(GameStats.TimeBeforeRespawn);
        GetComponent<FirstPersonController>().UnpauseCharacter();
        GetComponent<movementModifier>().resetEverything();
    }
}

