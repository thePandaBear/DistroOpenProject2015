using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {
	
	public static GameManager Instance { get; protected set; }
	
	public static Vector2 getWaypointPosition(int index) {
		return Instance.waypoints[index].transform.position;
	}
	
	void Awake() {
		Instance = this;
	}

    // tower range control
    public int rangeAdd = 0;
    public int rangeAddCost = 5;

    // tower attack control
    public int attackAdd = 0;
    public int attackAddCost = 5;

    // tile offset to calculate center of tile
    public Vector2 fieldOffset = new Vector2 (0.5f, 0.5f);
	
	// list to store monsters present in the game
	public List<GameObject> monsterList;
	
	// gameobject for castle to defend
	public GameObject playerCastle;
	
	// xml file which stores level data
	public XMLParser levelData;
	
	// create gameobjects for previously fabricatet monster/castles
	public GameObject monsterPrefab;
	public GameObject castlePrefab;
	
	// list for waypoints
	public Transform[] waypoints;
	public GameObject waypointsParent;
	
	/** parameters for the gameplay **/
	// currently available gold
	public int goldAvailable { get; protected set; }

    // number of lives available to the player
	public int nOfLives = 10;

    // cost to build a tower
    public int towerCost;

    // bounty for killing monster
    public int monsterReward;

    // the current state of the game
    public GameState gameState;
	
	// a bool to check if the game already finished
	public bool gameFinished;
	
	// initialization method
	void Start () {

        Debug.Log("Starting");
		
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
	
	// update the game.
	void Update () {

        switch (gameState) {
			// start of the game.
		case GameState.Start:
			if (Input.GetMouseButtonUp(0)) {
				gameState = GameState.Playing;
				StartCoroutine(newRound());
			}
			break;
		case GameState.Playing:
			if (nOfLives == 0) {
				// no lives left. game is lost!
				StopCoroutine(newRound());
				destroyAllMonsters();
				gameState = GameState.Lost;
			} else if (gameFinished && monsterList.Where(x => x != null).Count() == 0) {
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
	
	protected void initLevelFromXml() {
		
		// create gameobject for each waypoint
		int run = 0;
		foreach (var waypoint in levelData.waypointList) {
			
			// create new game object
			GameObject gameObject = new GameObject();
			
			// set object tag
			gameObject.tag = "Waypoint";
			
			// set object name
			string name = "Waypoints" + run.ToString();
			gameObject.name = name;
			
			// set parent object
			gameObject.transform.parent = waypointsParent.transform;
			
			// set object position to position from waypoint
			gameObject.transform.position = waypoint + fieldOffset;
			
			run++;
		}
		
		playerCastle = Instantiate(castlePrefab, levelData.castlePosition + fieldOffset,
		                           Quaternion.identity) as GameObject;
		playerCastle.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
		
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint")
			.OrderBy(x => x.name).Select(x => x.transform).ToArray();
		
		goldAvailable = levelData.gold;
	}
	
	protected void executeChecks() {
        if(!gameFinished)
		    StartCoroutine(newRound());
		gameFinished = true;
	}

    protected void destroyAllMonsters() {
		//get all the enemies
		foreach (var item in monsterList) {
			if (item != null)
				Destroy(item.gameObject);
		}
	}
	
	protected IEnumerator newRound() {
		
		// player has 2 seconds to do something.
		yield return new WaitForSeconds(5f);
		
		// create new monsters
		int difficulty = PlayerPrefs.GetInt ("difficulty");
		if (difficulty == 0) {
			difficulty = 1;
		}
		
		
		// create monsters endless
		for (int n = 0; n > -1 && !gameFinished && gameState != GameState.Lost; n++) {

            // never stop this wave! only 1 wave needed.
			
			// create a new monster.
			GameObject monster = Instantiate(monsterPrefab, waypoints[0].transform.position, Quaternion.identity) as GameObject;
			
			
			Monster monsterComponent = monster.GetComponent<Monster>();

            // set health of new monster.
            monsterComponent.health = 2 + (n / 5)*difficulty;
		
			// add monster to the monster list.
			monsterList.Add(monster);
			
			// wait a short period of time until the next monster is spawned
			yield return new WaitForSeconds(1f / (1));
		}
	}
	
	public void doDamage() {
		if (nOfLives <= 1){
			// game is lost
			nOfLives = 0;
            gameState = GameState.Lost;
			
			// destroy the castle.
			Destroy(playerCastle);
			
		} else {
			
			// game still ongoing.
			// one live is lost
			nOfLives--;
		}
	}

    public Boolean payForTower() {
        if(goldAvailable>=towerCost) {
            goldAvailable -= towerCost;
            return true;
        } else {
            return false;
        }
    }

    public Boolean payForRange() {
        if(goldAvailable >= rangeAddCost) {
            goldAvailable -= rangeAddCost;
            rangeAddCost = rangeAddCost * 2;
            rangeAdd++;
            return true;
        } else {
            return false;
        }
    }

    public Boolean payForAttack() {
        if(goldAvailable >= attackAddCost) {
            Debug.Log("Attack add Cost now: " + attackAddCost.ToString());
            goldAvailable -= attackAddCost;
            attackAddCost = attackAddCost * 2;
            attackAdd++;
            return true;
        } else {
            return false;
        }
    }
	
	public List<Vector2> getWaypoints() {
        if (levelData!= null)
        {
            return levelData.waypointList;
        }
        else {
            return null;
        }
	}

    // method for monsterDeath event
    protected void collectGold() {
        goldAvailable += monsterReward;
    }

    void OnGUI(){
        int width = Screen.width;
        int height = Screen.height;

        // size of buttons
        int buttonWidth = width / 3;
        int buttonHeight = height / 10;
        GUIStyle labelFont = new GUIStyle("label");
        labelFont.fontSize = width/30;
        GUIStyle buttonFont = new GUIStyle("button");
        buttonFont.fontSize = width/30;
        GUIStyle buttonFontSmall = new GUIStyle("button");
        buttonFontSmall.fontSize = width / 60;

        // nr of lives left
        GUI.Label(new Rect(10, 10, buttonWidth/2, buttonHeight), "Lives: " + nOfLives.ToString(), labelFont);
        // amount of gold left
        GUI.Label(new Rect(10, 10 + (int)(width / 50 * 1.5), buttonWidth / 2, buttonHeight), "Gold: " + goldAvailable.ToString(), labelFont);

        // Tower Build Cost
        GUI.Label(new Rect(10, 10 + (buttonHeight + buttonHeight)/2, buttonWidth, buttonHeight), "Tower Cost: " + towerCost.ToString(), labelFont);

        // upgrade tower attack button
        if(GUI.Button(new Rect(width - 10 - buttonWidth / 2, 10, buttonWidth / 2, buttonHeight), "Upgrade Range: " + rangeAddCost.ToString(), buttonFontSmall)) {
            // increase range if there is enough gold
            payForRange();
        }

        if(GUI.Button(new Rect(width - 10 - buttonWidth/2, 10+ (int)(width/50*3), buttonWidth/2, buttonHeight), "Upgrade Attack: " + attackAddCost.ToString(), buttonFontSmall)) {
            // increase attack if there is enough gold
            payForAttack();
        }

        // button to improve towers for gold

        if (gameState == GameState.Lost) {
            GUI.Label(new Rect(10, 100, buttonWidth/2, buttonHeight), "Game Over", labelFont);
            if (GUI.Button(new Rect(10, 150, buttonWidth/2, buttonHeight), "Back", buttonFont))
            {
                Application.LoadLevel("LoginMenu");
            }
        }
    }
}
