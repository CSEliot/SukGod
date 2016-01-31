using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Master : MonoBehaviour {

    private int myTeam;
    private int totalReady;
    public Text Countdown;
    private PhotonView m_PhotonView;

    private bool countdownStarted;

    void Awake ()
    {
        myTeam = -1;
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        m_PhotonView = GetComponent<PhotonView>();
        countdownStarted = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(";"))
        {
            PhotonNetwork.Disconnect();
            SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}

    public void OnJoinedRoom()
    {
    }

    void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        totalReady--;
        if (player.isMasterClient || countdownStarted)
        {
            PhotonNetwork.Disconnect();
            SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
        if(totalReady > 3 && !countdownStarted)
        {
            countdownStarted = true;
            StartCoroutine(BeginCountdown());
        }
    }

    [PunRPC]
    void SetCountdown(int count)
    {
        Countdown.gameObject.SetActive(true);
        Countdown.text = "" + count;
    }

    [PunRPC]
    void EndCountdown()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Wall"))
        {
            g.SetActive(false);
        }
        Countdown.gameObject.SetActive(false);
        PhotonNetwork.room.open = false;
    }

    IEnumerator BeginCountdown()
    {
        int count = 30;
        while(count >= 0)
        {
            m_PhotonView.RPC("SetCountdown", PhotonTargets.All, count);
            yield return new WaitForSeconds(1f);
            count--;
        }

        m_PhotonView.RPC("EndCountdown", PhotonTargets.All);
    }

    public int GetTeam()
    {
        return myTeam;
    }
}
