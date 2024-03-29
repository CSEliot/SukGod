using UnityEngine;

/// <summary>
/// This script automatically connects to Photon (using the settings file),
/// tries to join a random room and creates one if none was found (which is ok).
/// </summary>
public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
    /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
    public bool AutoConnect = true;
    private PhotonAnimatorView m_AnimatorView;
    public byte Version = 1;

    /// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
    private bool ConnectInUpdate = false;

    public Master MyMaster;

    public virtual void Start()
    {
        PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
    }

    public void BeginConnect()
    {
        ConnectInUpdate = true;
        GetComponent<ShowStatusWhenConnecting>().enabled = true;
    }

    public virtual void Update()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
            PhotonNetwork.sendRate = 100;
            PhotonNetwork.sendRateOnSerialize = 100;
            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
            PhotonNetwork.sendRate = 100;
            PhotonNetwork.sendRateOnSerialize = 100;
        }
    }


    // below, we implement some callbacks of PUN
    // you can find PUN's callbacks in the class PunBehaviour or in enum PhotonNetworkingMessage


    public virtual void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 20 }, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
        if((PhotonNetwork.room.playerCount-1)%5 == 0)
        {
            MyMaster.SetTeam(0);
        }
        else
        {
            MyMaster.SetTeam(1);
        }
        //CreatePlayerObject();
    }

    #region Photon
    
    public void CreatePlayerObject()
    {
        Vector3 myPosition;
        string playerTag = "";
        string spawnSide = ""; 
        if(MyMaster.GetTeam() == 0)
        {
            spawnSide = "SpawnsRed";
            playerTag = "Red Player";
        }else if (MyMaster.GetTeam() == 1)
        {
            spawnSide = "SpawnsBlue";
            playerTag = "Blue Player";
        }
        else
        {
            Debug.LogError("SPAWN SIDE TEAM NUM IS NOT 0 OR 1");
        }
        int totalSpawns = GameObject.FindGameObjectWithTag(spawnSide).transform.childCount;
        int randomSpawnNum = Random.Range(0, totalSpawns - 1);
        myPosition = GameObject.FindGameObjectWithTag(spawnSide).transform.GetChild(randomSpawnNum).transform.position;

        //Choose between blue and red player here
        if (playerTag == "Red Player")
        {

            GameObject newPlayerObject = PhotonNetwork.Instantiate("Player-Net44Red", myPosition, Quaternion.identity, 0);
            m_AnimatorView = newPlayerObject.GetComponent<PhotonAnimatorView>();
        }
        else
        {
            GameObject newPlayerObject = PhotonNetwork.Instantiate("Player-Net44Blue", myPosition, Quaternion.identity, 0);
            m_AnimatorView = newPlayerObject.GetComponent<PhotonAnimatorView>();
        }

        
        
        
        
    }

    #endregion
}
