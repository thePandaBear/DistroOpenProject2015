using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;


public class NetworkManager : MonoBehaviour {

	public static NetworkManager Instance; 

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
		MasterServer.RegisterHost ("clashofelements", gamename);
		Debug.Log ("server:  " + gamename); 
	}

	void OnServerInitialized(){
		Debug.Log ("server created");
	}

	void OnConnectedToServer(){
		Debug.Log ("server joined");

	}


}
