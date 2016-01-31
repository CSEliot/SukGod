/**
 * @author Chris Ridgely
 * @updated 12:09 Jan 30
 */

using UnityEngine;
using System.Collections;

public class ChantBehavior : MonoBehaviour {

	public string chantKey;
	public AudioSource chant;
	public float chantDuration = 10.0f;
	private bool isChanting;
    public bool hasChanted;

	// Use this for initialization
	void Start () {
        isChanting = false;
        hasChanted = false;
        //chant.Stop();
	}

	// Update is called once per frame
	void Update () {

        Debug.Log("Chanting: " + isChanting);

        //if chanting and if player has died
        if (isChanting && GetComponent<movementModifier>().isCurrentlyDead)
        {
            GameStats.addPointsTeam(1, "blue");
            GetComponent<movementModifier>().isCurrentlyDead = false;
        }

        //if player has chanted in the past then dies, player loses points
        if (hasChanted && GetComponent<movementModifier>().isCurrentlyDead)
        {
            GameStats.addPointsTeam(-1, "blue");
            GetComponent<movementModifier>().isCurrentlyDead = false;
        }
        Debug.Log("BLUE POINTS: " + GameStats.blueTeamPoints);
        //if character is not jumping
        StartCoroutine(startChanting ());
	}

    public IEnumerator startChanting()
    {
        //if not current chanting and not currently in a fire
        if (Input.GetButton(chantKey) && isChanting == false && !GetComponent<fireDeath>().isOnFire)
        {
            isChanting = true;
            Debug.Log("start chanting"); //Start Chanting
            yield return new WaitForSeconds(chantDuration);
            Debug.Log("stop chanting"); //Stops Chanting
            isChanting = false;
            hasChanted = true;
        }
    }

	public bool chantStatus(){
		return isChanting;
	}
}