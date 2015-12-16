using UnityEngine;
using System.Collections;


//this class is used for data that should stay alive during all scene transitions 

public class PersistentData : MonoBehaviour {

	string gamename;	

	void Awake(){
		DontDestroyOnLoad (transform.gameObject);
	}

	void Start () {

	}
	
	void Update () {
		
	}
	
	public void setGamename(string s){
		gamename = s;
	}

	public string getGamename(){
		return gamename;
	}
}
