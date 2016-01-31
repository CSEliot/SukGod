using UnityEngine;
using System.Collections;
using System.Timers;


//asumes this script is attached to the player right
public class poolDrown : MonoBehaviour {

    public float timeLeftoLive;
    public float secondsToDrown;
  

	// Use this for initialization
	void Start () {
        secondsToDrown = 5.0f;
        timeLeftoLive = secondsToDrown;
	}

    void Update()
    {
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "drownPool")
        {
            //count down
            timeLeftoLive -= Time.deltaTime;
            if (timeLeftoLive < 0.0f)
            {
                StartCoroutine("dieInPool");
            }
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "poolDrown")
        {
            timeLeftoLive = secondsToDrown;
        }
    }

    IEnumerator dieInPool()
    {
        GetComponent<movementModifier>().isCurrentlyDead = true;
        GetComponent<FirstPersonController>().enabled = false;
        GetComponent<Rigidbody>().Sleep();
        GetComponent<AnimationManager>().isDead(true);
        yield return new WaitForSeconds(GameStats.TimeBeforeRespawn);
        GetComponent<FirstPersonController>().enabled = true;
        GetComponent<movementModifier>().resetEverything();

    }

}
