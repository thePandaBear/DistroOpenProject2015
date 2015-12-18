using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class HostScreen : MonoBehaviour {

	string username; 
	string gamename; 
	InputField usernameField; 
	InputField gamenameField;
    public NetworkView nView;
    public int nr;
    public void Start () {
        nView = GetComponent<NetworkView>();
        nr = PlayerPrefs.GetInt("nr");
        gamename = "gamename";
        if (nr <= 0)
        {
            nr = 0;
        }
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

        //symbol
        GUI.DrawTexture(new Rect(width * 0.2f - symbol.width / 2, height / 2 - symbol.height / 2, symbol.width, symbol.height), symbol);
        GUI.DrawTexture(new Rect(width * 0.8f - symbol.width / 2, height / 2 - symbol.height / 2, symbol.width, symbol.height), symbol);

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
        gamename = GUI.TextField(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), gamename, textFont);

        if (NetworkManager.Instance.serverStarted && NetworkManager.Instance.playersConnected == nr - 1) {
            if (GUI.Button(new Rect(buttonX, buttonY * 2, buttonWidth, buttonHeight), "start Game", buttonFont))
            {
                NetworkManager.Instance.allowStart = true;
                Application.LoadLevel("InGame");
            }
        } else if (NetworkManager.Instance.serverStarted) {
            if (GUI.Button(new Rect(buttonX, buttonY * 2, buttonWidth, buttonHeight), "waiting for players: " + (NetworkManager.Instance.playersConnected + 1).ToString(), buttonFont)) {
                // do nothing
            }
        } else {
            if (GUI.Button(new Rect(buttonX, buttonY * 2, buttonWidth, buttonHeight), "start LAN Server", buttonFont))
            {
                //check if gamename is set, required to open a server
                if (gamename != null && gamename.Length > 0) {

                    PlayerPrefs.SetString("gamename", gamename);
                    NetworkManager.Instance.StartHost(gamename, nr);

                }
                else {
                    //TODO display some message
                    // !!probably not needed anymore.
                    Debug.Log("input missing: gamename");
                }
            }
        }

		
		if(GUI.Button(new Rect(buttonX,buttonY*3,buttonWidth,buttonHeight), "Back", buttonFont)){
			Application.LoadLevel("LoginMenu");
		}
	}
}
