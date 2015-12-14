using UnityEngine;
using System.Collections;

public class LobbyScreen : MonoBehaviour {
	string gamename;	

	void Start () {
		Debug.Log ("wee lobby entered");
		PersistentData ps = GameObject.Find("notDestroyed").GetComponent("PersistentData") as PersistentData;
		gamename = ps.getGamename();
	}
	
	void Update () {
		
	}
	

}
