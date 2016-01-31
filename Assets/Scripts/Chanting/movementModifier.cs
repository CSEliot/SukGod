using UnityEngine;
using System.Collections;

public class movementModifier : MonoBehaviour {

    //attached to the blue char obj

    public GameObject player;
    private ChantBehavior myChantLogic;

    //Assuming characters can only chant once
    public bool isCurrentlyDead;

    void Start()
    {
        myChantLogic = GetComponent<ChantBehavior>();
        isCurrentlyDead = false;
      //  haveChanted = false;
    }

    //Halt movement during chanting
	void Update ()
    {
        if (myChantLogic.chantStatus() /*&& !haveChanted*/ && !isCurrentlyDead)
        {
            //StartCoroutine("stopMoving", chantDuration);
        }
	}

    public IEnumerator stopMoving(float duration)
    {
        Debug.Log("Stop Moving");
        GetComponent<FirstPersonController>().PauseCharacter();
        //transform.position.y = 0.0f;
        //transform.position.x = 0.0f;
        yield return new WaitForSeconds(duration);
        GetComponent<FirstPersonController>().UnpauseCharacter();
    }

    public void resetEverything()
    {
        Vector3 pos = GameObject.Find("BlueSpawnArea").GetComponent<deathSpawnManager>().getWayPointLoc();
        transform.position = pos;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<FirstPersonController>().UnpauseCharacter();
        GetComponent<fireDeath>().isOnFire = false;
        isCurrentlyDead = false;
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
