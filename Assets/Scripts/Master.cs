﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Master : MonoBehaviour {

    private int myTeam;
    private int totalReady;
    public Text Countdown;
    private PhotonView m_PhotonView;

    void Awake ()
    {
        myTeam = -1;
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        m_PhotonView = GetComponent<PhotonView>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void OnJoinedRoom()
    {
    }

    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
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

    public void Ready()
    {
        m_PhotonView.RPC("ReadyCountAdd", PhotonTargets.MasterClient);
    }

    [PunRPC]
    public void ReadyCountAdd()
    {
        totalReady++;
        if(totalReady > 3)
        {
            Countdown.gameObject.SetActive(true);
            m_PhotonView.RPC("DoCountdown", PhotonTargets.All);
        }
    }

    [PunRPC]
    void DoCountdown()
    {
            StartCoroutine(BeginCountdown());
    }

    IEnumerator BeginCountdown()
    {
        int count = 30;
        while(count >= 0)
        {
            Countdown.text = "" + count;
            yield return new WaitForSeconds(1f);
            count--;
        }
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Wall"))
        {
            g.SetActive(false);
        }
        Countdown.gameObject.SetActive(false);
        PhotonNetwork.room.open = false;
    }

    public int GetTeam()
    {
        return myTeam;
    }
}