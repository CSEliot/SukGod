using UnityEngine;
using System.Collections;

//NOTES:
//asumes ChantBeahvior script and this script are
//part of the same game object
//attached to the blue char obj
public class fireDeath : MonoBehaviour {

    public float onFireSpeed;
    public float aliveBurningTime;
    public bool isOnFire;
	// Use this for initialization

    void Start()
    {
        isOnFire = false;
        onFireSpeed = 50.0f;
        aliveBurningTime = 10.0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fireBurn" && !isOnFire)
        {
            Debug.Log("burning");
            isOnFire = true;
            StartCoroutine("dieByFire");
            //increase speed
            GetComponent<FirstPersonController>().moveSpeed = onFireSpeed;
            //catch on fire <here>

        }
    }

    IEnumerator dieByFire()
    {
        yield return new WaitForSeconds(aliveBurningTime);
        //after burning for 10 seconds
        if (GetComponent<ChantBehavior>().chantStatus())
        {
            GameStats.addPointsTeam(1, "blue");
        }
        //character dies and stops moving after being burned
        GetComponent<movementModifier>().isCurrentlyDead = true;
        GetComponent<FirstPersonController>().enabled = false;
        GetComponent<Rigidbody>().Sleep();
        GetComponent<AnimationManager>().isDead(true);
        
        //needs to fall to the ground
        yield return new WaitForSeconds(GameStats.TimeBeforeRespawn);

        //RESET TO BEGINNING with Inital variables
        GetComponent<movementModifier>().resetEverything();
    }


}
