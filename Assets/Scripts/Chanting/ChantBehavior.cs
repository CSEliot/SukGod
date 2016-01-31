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
    private bool hasChanted;

	// Use this for initialization
	void Start () {
        isChanting = false;
        hasChanted = false;
        //chant.Stop();
	}

	// Update is called once per frame
	void Update () {

        //Debug.Log("BLUE POINTS: " + GameStats.blueTeamPoints);
        //if character is not jumping
        //if not current chanting and not currently in a fire
        if (Input.GetButton(chantKey) && isChanting == false && !GetComponent<fireDeath>().isOnFire)
        {
            isChanting = true;
            Debug.Log("start chanting"); //Start Chanting
            StartCoroutine(startChanting ());
        }

        //Debug.Log("Chanting: " + isChanting);

        //if chanting and if player has died
        //if (isChanting && GetComponent<movementModifier>().isCurrentlyDead)
        //{
        //    GameStats.addPointsTeam(1, "blue");
        //    GetComponent<movementModifier>().isCurrentlyDead = false;
        //}

        ////if player has chanted in the past then dies, player loses points
        //if (hasChanted && GetComponent<movementModifier>().isCurrentlyDead)
        //{
        //    GameStats.addPointsTeam(-1, "blue");
        //    GetComponent<movementModifier>().isCurrentlyDead = false;
        //}

        //if player has chanted in the past then DOES NOT die, player loses points
        if (hasChanted && !GetComponent<movementModifier>().isCurrentlyDead)
        {
            GameStats.addPointsTeam(-1, "blue");
            hasChanted = false;
        }

	}

    public IEnumerator startChanting()
    {
        yield return new WaitForSeconds(chantDuration);
        Debug.Log("stop chanting"); //Stops Chanting
        bool suicidedDuringChant = false;
        if (GetComponent<movementModifier>().isCurrentlyDead)
        {
            suicidedDuringChant = true;
        }
        isChanting = false;
        if(!suicidedDuringChant)
            hasChanted = true;
    }

    public void CliffDeath()
    {
        Debug.Log("Cliff Death");
        if (GetComponent<movementModifier>().isCurrentlyDead)
        {
            if (isChanting)
            {
                GameStats.addPointsTeam(1, "blue");
            }
            else
            {
                GameStats.addPointsTeam(-1, "blue");
            }
            isChanting = false;
        }
    }

	public bool chantStatus(){
		return isChanting;
	}

    public bool GetHasChanted()
    {
        return hasChanted;
    }

    public void SetHasChanted(bool myChant)
    {
        hasChanted = myChant;
    }
}