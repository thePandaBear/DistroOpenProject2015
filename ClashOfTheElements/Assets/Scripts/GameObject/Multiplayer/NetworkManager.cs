using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;


public class NetworkManager : MonoBehaviour {

	public static NetworkManager Instance;
    private HostData[] hostList;
	public GameObject gameManPrefab; 

	//TODO: display some message if server couldnt be created or server couldnt be joined 

    void Awake(){
		Instance = this; 
		DontDestroyOnLoad (transform.gameObject);
	}

	void Start () {
	}
	
	void Update () {
	}

	public void StartHost(string gamename){
		int listenport = 25000;
		NetworkConnectionError error = Network.InitializeServer (4, listenport, true);

		//loop while no connection could be established 
		while(error != NetworkConnectionError.NoError){
			listenport =  UnityEngine.Random.Range (25000, 26000);
			error= Network.InitializeServer (4, listenport, true);
		}
	
		MasterServer.RegisterHost ("ClashOfTheElements", gamename);
		Debug.Log ("server:  " + gamename); 
	}

    public void SearchServers() {
        MasterServer.RequestHostList("ClashOfTheElements");
    }

	void OnLevelWasLoaded(int index) {
		if (index == 0) {
			SpawnGame ();
		}
	}

    void OnServerInitialized()
    {
		Application.LoadLevel("InGame");
        Debug.Log("Server Initializied");
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
        {
            // get hotlist
            hostList = MasterServer.PollHostList();
        }
    }

    public HostData[] getServerList() {
        return hostList;
    }

    public void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    void OnConnectedToServer()
    {
		Application.LoadLevel("InGame");
        Debug.Log("Server Joined");
    }

	private void SpawnGame(){
		Network.Instantiate (gameManPrefab, new Vector3 (0, 0, 1), Quaternion.identity, 0);
	}
}
