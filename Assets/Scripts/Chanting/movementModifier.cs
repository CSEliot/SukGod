using UnityEngine;
using System.Collections;

public class movementModifier : MonoBehaviour {

    public GameObject player;
    public float chantDuration;

    //Assuming characters can only chant once
    public bool haveChanted;
    public bool isCurrentlyDead;

    void Start()
    {
        isCurrentlyDead = false;
        haveChanted = false;
        chantDuration = 1.0f;
        Debug.Log("start here");
        player = GameObject.Find("BluePlayer4");
    }

    //Halt movement during chanting
	void Update ()
    {
        

        if(player.GetComponent<ChantBehavior>().chantStatus() && !haveChanted && !isCurrentlyDead)
        {
            Debug.Log("chanting");
            haveChanted = true;
            Chanting();
        }
	}

    void Chanting()
    {
        StartCoroutine("stopMoving", chantDuration);  
    }

    public IEnumerator stopMoving(float duration)
    {
        player.GetComponent<Rigidbody>().Sleep();
        player.GetComponent<FirstPersonController>().enabled = false;
        yield return new WaitForSeconds(duration);
        //renable movement after chanting
        player.GetComponent<Rigidbody>().WakeUp();
        player.GetComponent<FirstPersonController>().enabled = true;

    }

    public void resetEverything()
    {
        Vector3 pos = GameObject.Find("BlueSpawnArea").GetComponent<deathSpawnManager>().getWayPointLoc();
        transform.position = pos;
        GetComponent<FirstPersonController>().enabled = true;
        GetComponent<Rigidbody>().WakeUp();
        GetComponent<fireDeath>().isOnFire = false;
        isCurrentlyDead = false;
        haveChanted = false;
        GetComponent<AnimationManager>().isDead(false);
        GetComponent<AnimationManager>().isReset(true);
        StartCoroutine("waitToClearReset");
    }

    IEnumerator waitToClearReset()
    {
        yield return new WaitForSeconds(2.0F);
        GetComponent<AnimationManager>().isReset(false);

    }


}
