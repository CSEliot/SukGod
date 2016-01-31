using UnityEngine;
using System.Collections;

public class ChooseTeamUIEnable : MonoBehaviour {

    int team;

    public void OnEnable()
    {
       
    }

	// Use this for initialization
	void Start () {
        team = -1;
	}
	
	// Update is called once per frame
	void Update () {
        if(team == -1)
        {
            team = GameObject.FindGameObjectWithTag("Master").GetComponent<Master>().GetTeam();
            if(team != -1)
                transform.GetChild(team).gameObject.SetActive(true);
        }
    }
}
