using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour {

	public static NetworkManager Instance; 

	void Awake(){
		Instance = this; 
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
		MasterServer.RegisterHost ("gameType", gamename);
		Debug.Log ("server:  " + gamename); 
	}

}
