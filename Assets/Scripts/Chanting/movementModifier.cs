using UnityEngine;
using System.Collections;

public class movementModifier : MonoBehaviour {

    //attached to the blue char obj

    public GameObject player;
    public float chantDuration;

    //Assuming characters can only chant once
    public bool isCurrentlyDead;

    void Start()
    {
        isCurrentlyDead = false;
      //  haveChanted = false;
        chantDuration = 2.0f;
        player = GameObject.Find("BluePlayer4");
    }

    //Halt movement during chanting
	void Update ()
    {
        if (player.GetComponent<ChantBehavior>().chantStatus() /*&& !haveChanted*/ && !isCurrentlyDead)
        {
            StartCoroutine("stopMoving", chantDuration);
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
        GetComponent<FirstPersonController>().enabled = true;
        GetComponent<fireDeath>().isOnFire = false;
        isCurrentlyDead = false;
        GetComponent<AnimationManager>().isDead(false);
        GetComponent<AnimationManager>().isReset(true);
        GetComponent<ChantBehavior>().hasChanted = false;
        StartCoroutine("waitToClearReset");
    }

    IEnumerator waitToClearReset()
    {
        yield return new WaitForSeconds(2.0F);
        GetComponent<AnimationManager>().isReset(false);
    }
}
