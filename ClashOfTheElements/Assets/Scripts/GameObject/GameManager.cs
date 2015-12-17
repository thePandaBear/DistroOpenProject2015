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

    NetworkView nView;
	
	void Awake() {
		Instance = this;
		DontDestroyOnLoad (this);
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
    public int NrOfDiffMonster;
    public GameObject monsterPrefab1;
    public GameObject monsterPrefab2;
    public GameObject monsterPrefab3;
    public GameObject monsterPrefab4;

	public GameObject castlePrefab;
	
	// list for waypoints
	public Transform[] waypoints;
	public GameObject waypointsParent;
	
	/** parameters for the gameplay **/
	// currently available gold

	public int goldAvailable; 
    // number of lives available to the player
	public int nOfLives = 10;

    // cost to build a tower
    public int towerCost;

    // bounty for killing monster
    public int monsterReward;

    // the index of the current round
    public int roundNumber = 0;

    // the current state of the game
    public GameState gameState;

    // a bool to check if a round has been launched completely
    public bool finishedSpawning;

    // initialization method
    void Start () {


        nView = GetComponent<NetworkView>();

        Debug.Log("Starting");
		
		// TODO: Add difficulty specific lives
		
		// create new list for monsters
		monsterList = new List<GameObject>();
		
		// read the xml  level file
		levelData = Util.parseXML();
		
		// find game objects with the name "Waypoints"
		waypointsParent = new GameObject();
		waypointsParent.name = "Waypoints";
		
		// initialize the level using the given xml level file
		initLevelFromXml();
		
		// set current game state to playing
		gameState = GameState.Playing;
		
		// start new round
		executeChecks();

        // nothing spawned yet
        finishedSpawning = true;

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
			} else if (finishedSpawning && monsterList.Where(x => x != null).Count() == 0) {
                    // player defeated all monsters in current round
                    executeChecks();
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
        waypoints = new Transform[levelData.waypointList.Count];
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

            waypoints[run] = gameObject.transform;

			run++;
		}
		
		playerCastle = Instantiate(castlePrefab, levelData.castlePosition + fieldOffset,
		                           Quaternion.identity) as GameObject;
		playerCastle.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
		
		/*waypoints = GameObject.FindGameObjectsWithTag("Waypoint")
			.OrderBy(x => x.name).Select(x => x.transform).ToArray();*/
		
		goldAvailable = levelData.gold;
	}
	

    protected void executeChecks() {
        if (finishedSpawning) {
            StartCoroutine(newRound());
            finishedSpawning = false;
        }
    }

    protected void destroyAllMonsters() {
		//get all the enemies
		foreach (var item in monsterList) {
			if (item != null)
				Destroy(item.gameObject);
		}
	}
	
	protected IEnumerator newRound() {

		if (Network.isServer) {
			// player has 5 seconds to do something.
			yield return new WaitForSeconds (5f);

			// create new monsters
			int difficulty = PlayerPrefs.GetInt ("difficulty");
			if (difficulty == 0) {
				difficulty = 1;
			}

			roundNumber++;
            // change monster 
            GameObject currentMonster;
            switch (roundNumber % NrOfDiffMonster)
            {
                case 0:
                    currentMonster = monsterPrefab1;
                    break;
                case 1:
                    currentMonster = monsterPrefab2;
                    break;
                case 2:
                    currentMonster = monsterPrefab3;
                    break;
                case 3:
                    currentMonster = monsterPrefab4;
                    break;
                default:
                    currentMonster = monsterPrefab1;
                    break;
            }
            // create 10 monsters
            for (int n = 0; n < 10 && gameState != GameState.Lost; n++) {

				// create a new monster.
				if (waypoints == null) {
					Debug.Log ("xxxx");
				}
				GameObject monster = Network.Instantiate (currentMonster, waypoints[0].transform.position, Quaternion.identity, 0) as GameObject;
				Monster monsterComponent = monster.GetComponent<Monster> ();

				// set health of new monster.
				monsterComponent.health = roundNumber * difficulty;

				// add monster to the monster list.
				monsterList.Add (monster);

				// wait a short period of time until the next monster is spawned
				yield return new WaitForSeconds (0.75f / (1));
			}

			finishedSpawning = true;
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
        if (goldAvailable>=towerCost) {
            nView.RPC("payForTowerRemote", RPCMode.OthersBuffered);
            goldAvailable -= towerCost;
            return true;
        } else {
            return false;
        }
    }

    public Boolean payForRange() {
        if(goldAvailable >= rangeAddCost) {
            nView.RPC("payForRangeRemote", RPCMode.OthersBuffered);
            goldAvailable -= rangeAddCost;
            rangeAddCost = rangeAddCost * 2;
            rangeAdd++;
            return true;
        } else {
            return false;
        }
    }

    [RPC]
    void payForRangeRemote()
    {
        goldAvailable -= rangeAddCost;
        rangeAddCost = rangeAddCost * 2;
        rangeAdd++;
    }

    [RPC]
    void payForTowerRemote()
    {
        goldAvailable -= towerCost;
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
        if(Network.isServer)
            nView.RPC("collectGoldRemote", RPCMode.OthersBuffered);
        goldAvailable += monsterReward;
    }

    [RPC]
    void collectGoldRemote()
    {
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
        GUI.Label(new Rect(10, 10, buttonWidth/2, buttonHeight), "    " + nOfLives.ToString(), labelFont);
        // amount of gold left
        GUI.Label(new Rect(10, 10 + (int)(width / 50 * 1.5), buttonWidth / 2, buttonHeight), "    " + goldAvailable.ToString(), labelFont);

        // Tower Build Cost
        GUI.Label(new Rect(10, 10 + (int)(width/ 50 * 3), buttonWidth, buttonHeight), "Tower Cost: " + towerCost.ToString(), labelFont);

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
            GUI.Label(new Rect(10, 10 + (int)(width / 50 * 4.5), buttonWidth/2, buttonHeight), "Game Over", labelFont);
            if (GUI.Button(new Rect(10, 10 + (int)(width / 50 * 6), buttonWidth/2, buttonHeight), "Back", buttonFont))
            {
                Application.LoadLevel("LoginMenu");
            }
        }
    }
}
