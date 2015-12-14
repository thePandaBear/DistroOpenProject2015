using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour {
	
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

		// create custom style for label font
		GUIStyle labelFont = new GUIStyle("label");
		labelFont.fontSize = 60;
		
		// create gui box
		GUI.Box (new Rect (0, 0, width, height), "");

		GUI.Label (new Rect (buttonX, buttonY/2, buttonWidth, buttonHeight), "Options", labelFont);

		// get current difficulty
		int difficulty = PlayerPrefs.GetInt ("difficulty");

		// check if difficulty is set or not.
		if (difficulty == 0) {
			// not set yet, set to 1 (easy)
			PlayerPrefs.SetInt("difficulty", 1);
			difficulty = 1;
		}

		// text for options menu
		string difficultyText = "";

		if (difficulty == 1) {
			difficultyText = "Difficulty: Easy";
		} else if (difficulty == 2) {
			difficultyText = "Difficulty: Medium";
		} else if (difficulty == 3) {
			difficultyText = "Difficulty: Hard";
		}

		if(GUI.Button(new Rect(buttonX,buttonY,buttonWidth,buttonHeight), difficultyText, buttonFont)){

			// increase
			difficulty++;
			if(difficulty > 3) {
				difficulty = 1;
			}
			// save new difficulty
			PlayerPrefs.SetInt("difficulty", difficulty);

		}

		if(GUI.Button(new Rect(buttonX,buttonY*2,buttonWidth,buttonHeight), "Back", buttonFont)){
			Application.LoadLevel("LoginMenu");
		}
	}
}
