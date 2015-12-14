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
		int buttonWidth = width / 3;
		int buttonHeight = height / 10;

		// get standard button height and width
		int buttonX = width / 2 - buttonWidth / 2;
		int buttonY = height / 6;

		// create custom style for bigger font
		GUIStyle buttonFont = new GUIStyle("button");
		buttonFont.fontSize = 40;

		// custom style for label font
		GUIStyle labelFont = new GUIStyle ("label");
		labelFont.fontSize = 60;

		// create gui box
		GUI.Box (new Rect (0, 0, width, height), "");

		GUI.Label (new Rect (buttonX, buttonY/2, buttonWidth, buttonHeight), "Menu", labelFont);

		if(GUI.Button(new Rect(buttonX,buttonY,buttonWidth,buttonHeight), "Start Singleplayer", buttonFont)){
			Application.LoadLevel("testScene");
		}
		if(GUI.Button(new Rect(buttonX,buttonY*2,buttonWidth,buttonHeight), "Host Multiplayer", buttonFont)){
			Application.LoadLevel("HostMultiplayer");
		}
		if(GUI.Button(new Rect(buttonX,buttonY*3,buttonWidth,buttonHeight), "Join Multiplayer", buttonFont)){
			Application.LoadLevel("JoinMultiplayer");
		}
		if(GUI.Button(new Rect(buttonX,buttonY*4,buttonWidth,buttonHeight), "Options", buttonFont)){
			Application.LoadLevel("Options");
		}
	}
}
