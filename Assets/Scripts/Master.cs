using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Master : MonoBehaviour {

    private int myTeam;
    

    void Awake ()
    {
        myTeam = -1;
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    /// <summary>
    /// Team 0 is Red, Team 1 is Blue
    /// </summary>
    /// <param name="team"></param>
    public void SetTeam(int team)
    {
        myTeam = team;
        if(myTeam == 0)
        {
            StartRed();
        }
        else if(myTeam == 1)
        {
            StartBlue();
        }
    }

    private void StartRed()
    {
        //GameObject.FindGameObjectWithTag("SpawnRed")
    }

    private void StartBlue()
    {

    }

    public int GetTeam()
    {
        return myTeam;
    }
}
