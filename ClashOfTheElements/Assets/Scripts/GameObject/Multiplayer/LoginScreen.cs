using UnityEngine;
using System.Collections;

public class LoginScreen : MonoBehaviour {

    public string number;
    public int nr;


    void Start () {
        number = "2";
	}
	
	void Update () {
		if(!number.Equals(""))
        {
            nr = int.Parse(number);
            if(nr <= 0) {
                nr = 1;
            }
        }
	}

    public Texture2D symbol;

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
		buttonFont.fontSize = width / 30;

		// custom style for label font
		GUIStyle labelFont = new GUIStyle ("label");
		labelFont.fontSize = width / 30;

        GUIStyle textFont = new GUIStyle(GUI.skin.textField);
        textFont.fontSize = width / 30;

        //symbol
        GUI.DrawTexture(new Rect(width * 0.2f - symbol.width / 2, height / 2 - symbol.height / 2, symbol.width, symbol.height), symbol);
        GUI.DrawTexture(new Rect(width * 0.8f - symbol.width / 2, height / 2 - symbol.height / 2, symbol.width, symbol.height), symbol);

        // create gui box
        GUI.Box (new Rect (0, 0, width, height), "");

		GUI.Label (new Rect (buttonX, buttonY/2, buttonWidth, buttonHeight), "Menu", labelFont);

        number = GUI.TextField(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), number, textFont);

        if (GUI.Button(new Rect(buttonX,buttonY*2,buttonWidth,buttonHeight), "Host Multiplayer", buttonFont)){
            PlayerPrefs.SetInt("nr", nr);
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
