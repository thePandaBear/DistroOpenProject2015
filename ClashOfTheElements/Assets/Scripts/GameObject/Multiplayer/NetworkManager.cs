using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;


public class NetworkManager : MonoBehaviour {

	public static NetworkManager Instance;
    private HostData[] hostList;

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

    void OnServerInitialized()
    {
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

    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }


}
