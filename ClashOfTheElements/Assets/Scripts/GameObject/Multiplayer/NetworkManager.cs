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

	public void StartHost(string username, string gamename){

	}


}
