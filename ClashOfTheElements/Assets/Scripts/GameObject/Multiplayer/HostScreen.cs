using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class HostScreen : MonoBehaviour {

	string username; 
	string gamename; 
	InputField usernameField; 
	InputField gamenameField; 

	// Use this for initialization
	void Start () {
		 usernameField = GameObject.Find("InputUsername").GetComponent<InputField>();
		 gamenameField = GameObject.Find("InputGameName").GetComponent<InputField>();
		 usernameField.onEndEdit.AddListener (setUsername);
		 gamenameField.onEndEdit.AddListener (setGameName);
	}

	private void setUsername (string arg){
		 username = arg;
	}

	private void setGameName (string arg){
		 gamename = arg;
	}

	// Update is called once per frame
	void Update () {
	}
	
	void OnGUI(){
		GUI.Box (new Rect (0, 0, 1125, 900), "Host a multiplayer Server");

		if(GUI.Button(new Rect(525,500,150,50), "start LAN Server")){
			//check if username and gamename are set
				if((username != null) & (gamename != null)) {
					//TODO save this user as server with username
					//change to Lobby
					Application.LoadLevel("Lobby"); 
				}else{
				//TODO display some message
				Debug.Log("input missing: username or gamename");
				}
		}
	}
}
