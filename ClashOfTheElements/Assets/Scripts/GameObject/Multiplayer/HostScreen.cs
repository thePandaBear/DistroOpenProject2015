using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class HostScreen : MonoBehaviour {

	string username; 
	string gamename; 
	InputField usernameField; 
	InputField gamenameField; 

	public void Start () {
        /*
		 usernameField = GameObject.Find("InputUsername").GetComponent<InputField>();
		 gamenameField = GameObject.Find("InputGameName").GetComponent<InputField>();
		 usernameField.onEndEdit.AddListener (setUsername);
		 gamenameField.onEndEdit.AddListener (setGameName);
         */
	}

	private void setUsername (string arg){
		 username = arg;
	}

	private void setGameName (string arg){
		 gamename = arg;
	}

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
        buttonFont.fontSize = width / 30;

        // create custom style for label font
        GUIStyle labelFont = new GUIStyle("label");
        labelFont.fontSize = width / 30;

        GUIStyle textFont = new GUIStyle(GUI.skin.textField);
        textFont.fontSize = width / 30;

        GUI.Box (new Rect (0, 0, width, height), "");

		GUI.Label (new Rect (buttonX, buttonY/2, buttonWidth, buttonHeight), "Host Game", labelFont);

		// get username
		string username = PlayerPrefs.GetString ("username");

		// if not set, set to Spongebob
		if (username.Equals ("")) {
			username = "Spongebob";
			PlayerPrefs.SetString("username", username);
		}
        gamename = "gamename";
        gamename = GUI.TextField(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), gamename, textFont);

		if(GUI.Button(new Rect(buttonX,buttonY*2,buttonWidth,buttonHeight), "start LAN Server", buttonFont)){
			//check if gamename is set, required to open a server
				if(gamename != null && gamename.Length > 0) {

                    PlayerPrefs.SetString("gamename", gamename);
                    Application.LoadLevel("Lobby");

                

                /* TEMP */

                
                /*
					sorry yen, schnalle ned wieni das mues mache
					PersistentData ps = GameObject.Find("notDestroyed").GetComponent("PersistentData") as PersistentData;
					ps.setGamename(gamename);
                */
                
				}else{
					//TODO display some message
					// !!probably not needed anymore.
					Debug.Log("input missing: gamename");
				}
		}
		if(GUI.Button(new Rect(buttonX,buttonY*3,buttonWidth,buttonHeight), "Back", buttonFont)){
			Application.LoadLevel("LoginMenu");
		}
	}
}
