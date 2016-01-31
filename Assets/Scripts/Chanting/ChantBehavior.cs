/**
 * @author Chris Ridgely
 * @updated 12:09 Jan 30
 */

using UnityEngine;
using System.Collections;

public class ChantBehavior : MonoBehaviour {

	public string chantKey;
	public AudioSource chant;
	public float chantDuration = 5.0f;
	private bool isChanting;
    private bool chantStop;
    private bool isOnFire;
    private PhotonView m_p;


    // Use this for initialization
    void Start()
    {
        m_p = GetComponent<PhotonView>();
        isOnFire = false;
        chantStop = true;
        isChanting = false;
        //chant.Stop();
	}

	// Update is called once per frame
	void Update () {

        //Debug.Log("BLUE POINTS: " + GameStats.blueTeamPoints);
        //if character is not jumping
        //if not current chanting and not currently in a fire
        if (gameObject.tag == "Blue Player" && Input.GetButtonDown(chantKey) && isChanting == false && !isOnFire && m_p.isMine)
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
	}

    IEnumerator EndChantIn()
    {
        yield return new WaitForSeconds(chantDuration);
        if (isChanting)
        {
            chantStop = true;
            GameStats.addPointsTeam(-1, "blue");
        }
    }

    public IEnumerator startChanting()
    {
        StartCoroutine(EndChantIn());
        while (!chantStop)
        {
            //Debug.Log("stop chanting"); //Stops Chanting
            yield return null;
        }
        chantStop = false;
        isChanting = false;
    }

    public void CliffDeath()
    {
        Debug.Log("Cliff Death");
        if (isChanting)
        {
            GameStats.addPointsTeam(1, "blue");
        }
        else
        {
            GameStats.addPointsTeam(-1, "blue");
        }
        chantStop = true;
    }

    public void FireDeath(float firetime)
    {
        isOnFire = true;
        Debug.Log("Fire Death");
        if (isChanting)
        {
            StartCoroutine(winPointsIn(firetime));
        }
        else
        {
            StartCoroutine(losePointsIn(firetime));
        }
        chantStop = true;
    }

    IEnumerator losePointsIn(float firetime)
    {
        yield return new WaitForSeconds(firetime);
        GameStats.addPointsTeam(-1, "blue");
        isOnFire = false;
    }

    IEnumerator winPointsIn(float firetime)
    {
        yield return new WaitForSeconds(firetime);
        GameStats.addPointsTeam(1, "blue");
        isOnFire = false;
    }

    public void Murdered()
    {
        if (isChanting)
        {
            Debug.Log("Murdered while Chanting!");
            GameStats.addPointsTeam(-2, "blue");
            chantStop = true;
        }
        else
        {
            Debug.Log("Murdered!");
            GameStats.addPointsTeam(-1, "blue");
        }
        isOnFire = false;
    }

	public bool chantStatus(){
		return isChanting;
	}

    public void StopChanting()
    {
        chantStop = true;
    }
}