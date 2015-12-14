using UnityEngine;
using System.Collections;

public class LoginScreen : MonoBehaviour {
	
	void Start () {
		
	}
	
	void Update () {
		
	}

	void OnGUI(){
		GUI.Box (new Rect (0, 0, 1200, 900), "Menu");

		if(GUI.Button(new Rect(525,200,150,50), "Start Singleplayer")){
			Application.LoadLevel("testScene");
		}
		if(GUI.Button(new Rect(525,300,150,50), "Host Multiplayer")){
			Application.LoadLevel("HostMultiplayer"); 
		}
		if(GUI.Button(new Rect(525,400,150,50), "Join Multiplayer")){
			Application.LoadLevel("JoinMultiplayer"); 
		}
		if(GUI.Button(new Rect(525,500,150,50), "Options")){
			Application.LoadLevel("Options"); 
		}
	}
}
