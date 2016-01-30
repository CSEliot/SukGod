using UnityEngine;
using System.Collections;

//NOTES:
//asumes ChantBeahvior script and this script are
//part of the same game object

//attached to the blue char obj
public class fireDeath : MonoBehaviour {

    public bool isOnFire;
	// Use this for initialization

    void Start()
    {
        isOnFire = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fireBurn" && !isOnFire)
        {
            Debug.Log("burning");
            isOnFire = true;
            StartCoroutine("dieByFire");
        }
    }

    IEnumerator dieByFire()
    {
        yield return new WaitForSeconds(10.0f);
        //after 10 seconds
        if (GetComponent<ChantBehavior>().chantStatus())
        {
            GameStats.addPointsTeam(1, "blue");
        }
        Vector3 pos = GameObject.Find("BlueSpawnArea").GetComponent<deathSpawnManager>().getWayPointLoc();
        transform.position = pos;
    }
}
