using UnityEngine;
using System.Collections;

public class LoginScreen : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){

		// previously w=1200, h=900
		int width = Screen.width;
		int height = Screen.height;

		// size of buttons
		int buttonWidth = width / 5;
		int buttonHeight = height / 10;

		// get standard button height and width
		int buttonX = width / 2 - buttonWidth / 2;
		int buttonY = height / 6;

		// create custom style for bigger font
		GUIStyle customButton = new GUIStyle("button");
		customButton.fontSize = 40;

		// create gui box
		GUI.Box (new Rect (0, 0, width, height), "");


		if(GUI.Button(new Rect(buttonX,buttonY,buttonWidth,buttonHeight), "Start Singleplayer", customButton)){
			Application.LoadLevel("testScene");
		}
		if(GUI.Button(new Rect(buttonX,buttonY*2,buttonWidth,buttonHeight), "Host Multiplayer", customButton)){
			Application.LoadLevel("HostMultiplayer"); 
		}
		if(GUI.Button(new Rect(buttonX,buttonY*3,buttonWidth,buttonHeight), "Join Multiplayer", customButton)){
			Application.LoadLevel("JoinMultiplayer"); 
		}
		if(GUI.Button(new Rect(buttonX,buttonY*4,buttonWidth,buttonHeight), "Options", customButton)){
			Application.LoadLevel("Options"); 
		}
	}
}
