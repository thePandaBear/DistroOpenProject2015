using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;

public class LobbyScreen : GameManager {
	string gamename;

    void Start() {
        Debug.Log("wee lobby entered");

        gamename = PlayerPrefs.GetString("gamename");

        /*
        PersistentData ps = GameObject.Find("notDestroyed").GetComponent("PersistentData") as PersistentData;
        gamename = ps.getGamename();
        */

        // TODO: Add difficulty specific lives

        // create new list for monsters
        monsterList = new List<GameObject>();

        // read the xml  level file
        levelData = Util.parseXML();

        // find game objects with the name "Waypoints"
        waypointsParent = GameObject.Find("Waypoints");

        // initialize the level using the given xml level file
        initLevelFromXml();

        // set current game state to playing
        gameState = GameState.Playing;

        // start new round
        executeChecks();

        // set game to running
        gameFinished = false;

        // add event handler to monster
        Monster.OnMonsterDeath += collectGold;

    }

    void Awake() {
        Instance = this;
    }

    // update the game.
    void Update()
    {

        switch (gameState)
        {
            // start of the game.
            case GameState.Start:
                if (Input.GetMouseButtonUp(0))
                {
                    gameState = GameState.Playing;
                    StartCoroutine(newRound());
                }
                break;
            case GameState.Playing:
                if (nOfLives == 0)
                {
                    // no lives left. game is lost!
                    StopCoroutine(newRound());
                    destroyAllMonsters();
                    gameState = GameState.Lost;
                }
                else if (gameFinished && monsterList.Where(x => x != null).Count() == 0)
                {
                    destroyAllMonsters();
                    gameState = GameState.Won;
                }
                break;
            case GameState.Won:
                if (Input.GetMouseButtonUp(0)) {
                    Application.LoadLevel(Application.loadedLevel);
                }
                break;
            case GameState.Lost:
                destroyAllMonsters();
                // do nothing
                break;
            default:
                break;
        }
    }

    void OnGUI()
    {
        int width = Screen.width;
        int height = Screen.height;

        // size of buttons
        int buttonWidth = width / 3;
        int buttonHeight = height / 10;
        GUIStyle labelFont = new GUIStyle("label");
        labelFont.fontSize = width / 30;
        GUIStyle buttonFont = new GUIStyle("button");
        buttonFont.fontSize = width / 30;
        GUIStyle buttonFontSmall = new GUIStyle("button");
        buttonFontSmall.fontSize = width / 60;

        // nr of lives left
        GUI.Label(new Rect(10, 10, buttonWidth / 2, buttonHeight), "Lives: " + nOfLives.ToString(), labelFont);
        // amount of gold left
        GUI.Label(new Rect(10, 10 + (int)(width / 50 * 1.5), buttonWidth / 2, buttonHeight), "Gold: " + goldAvailable.ToString(), labelFont);
        // upgrade tower attack button
        if (GUI.Button(new Rect(width - 10 - buttonWidth / 2, 10, buttonWidth / 2, buttonHeight), "Upgrade Range: " + rangeAddCost.ToString(), buttonFontSmall))
        {
            // increase range if there is enough gold
            payForRange();
        }

        if (GUI.Button(new Rect(width - 10 - buttonWidth / 2, 10 + (int)(width / 50 * 3), buttonWidth / 2, buttonHeight), "Upgrade Attack: " + attackAddCost.ToString(), buttonFontSmall)) {
            // increase attack if there is enough gold
            payForAttack();
        }

        // button to improve towers for gold

        if (gameState == GameState.Lost)
        {
            GUI.Label(new Rect(10, 100, buttonWidth / 2, buttonHeight), "Game Over", labelFont);
            if (GUI.Button(new Rect(10, 150, buttonWidth / 2, buttonHeight), "Back", buttonFont))
            {
                Application.LoadLevel("LoginMenu");
            }
        }
    }
}