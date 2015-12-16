﻿using UnityEngine;
using System.Collections.Generic;

public class JoinScreen : MonoBehaviour {

    public Vector2 scrollPosition = Vector2.zero;

    // list for servers
    private List<string> serverList;

    // Use this for initialization
    void Start() {
        // example for adding server
        serverList = new List<string>();

        // !!!!!! ADD SERVERS TO THIS LIST! !!!!!!
        serverList.Add("testserver1");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver2");
        serverList.Add("testserver11");
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnGUI() {

        // previously w=1200, h=900
        int width = Screen.width;
        int height = Screen.height;

        // size of buttons
        int buttonWidth = width / 3;
        int buttonHeight = height / 10;

        // get standard button positions
        int buttonX = width / 2 - buttonWidth / 2;
        int buttonY = height / 6;

        // create custom style for bigger font
        GUIStyle buttonFont = new GUIStyle("button");
        buttonFont.fontSize = width / 30;

        // create custom style for label font
        GUIStyle labelFont = new GUIStyle("label");
        labelFont.fontSize = width / 30;

        // create custom style for servers in list
        GUIStyle serverFont = new GUIStyle("label");
        serverFont.fontSize = width / 50;

        // create background texture
        var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);

        // set the pixel values
        texture.SetPixel(0, 0, Color.black);
        texture.SetPixel(1, 0, Color.blue);
        texture.SetPixel(0, 1, Color.blue);
        texture.SetPixel(1, 1, Color.black);

        // Apply all SetPixel calls
        texture.Apply();

        // box style 1
        GUIStyle listStyle = new GUIStyle(GUI.skin.box);
        listStyle.normal.background = texture;
        
        // create gui box
        GUI.Box(new Rect(0, 0, width, height), "");

        // create list background
        GUI.Box(new Rect(buttonX-6, buttonY-6, buttonWidth+12, buttonHeight * 5+12), "", listStyle);


        // add label
        GUI.Label(new Rect(buttonX, buttonY / 2, buttonWidth, buttonHeight), "Join Multiplayer", labelFont);

        // get scroll view size
        int scrollViewSize = 60 + (int)(width / 50 * 1.5) * (serverList.Count - 1);

        // begin the scroll view for the server listing
        scrollPosition = GUI.BeginScrollView(new Rect(buttonX, buttonY, buttonWidth, buttonHeight*5), scrollPosition, new Rect(0, 0, buttonWidth-20, scrollViewSize));

        for(int i = 0; i < serverList.Count; i++) {
            GUI.Label(new Rect(10, 10 + (int)(width / 50 * 1.5)*i, buttonWidth / 5 * 4 - 20, 100), serverList[i], serverFont);
        }

        // end the scroll view
        GUI.EndScrollView();

        // add back button to screen
        if (GUI.Button(new Rect(buttonX, (int)(buttonY *4.5) , buttonWidth, buttonHeight), "Back", buttonFont)) {
            Application.LoadLevel("LoginMenu");
        }
    }
}