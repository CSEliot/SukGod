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
            cliffStart = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cliffEnd")
            cliffEnd = true;
        StartCoroutine("cliffFallOffCheck");
    }

    //Check if character falls off the cliff
    IEnumerator cliffFallOffCheck()
    {
        yield return new WaitForSeconds(2.0f);
   
        if (cliffStart && cliffEnd)
        {
            Debug.Log("cliff death");
            //play death animation
            //IF CHANTING POINTS COMPLETE
              if(GetComponent<ChantBehavior>().chantStatus())
                GameStats.addPointsTeam(1, "blue");
            //Could also use getSpawnLoc()
            Vector3 spawnLoc = GameObject.Find("BlueSpawnArea").GetComponent<deathSpawnManager>().getWayPointLoc();
            transform.position = spawnLoc;
        }
        else
         cliffEnd = false;
         cliffStart = false;
        }
    }
