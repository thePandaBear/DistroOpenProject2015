using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {
	
	public static GameManager Instance { get; private set; }
	
	public static Vector2 getWaypointPosition(int index) {
		return Instance.waypoints[index].transform.position;
	}
	
	void Awake() {
		Instance = this;
	}
	
	// tile offset to calculate center of tile
	private Vector2 fieldOffset = new Vector2 (0.5f, 0.5f);
	
	// list to store monsters present in the game
	public List<GameObject> monsterList;
	
	// gameobject for castle to defend
	GameObject playerCastle;
	
	// xml file which stores level data
	private XMLParser levelData;
	
	// create gameobjects for previously fabricatet monster/castles
	public GameObject monsterPrefab;
	public GameObject castlePrefab;
	
	// list for waypoints
	public Transform[] waypoints;
	private GameObject waypointsParent;
	
	/** parameters for the gameplay **/
	// amount of money that is available
	public int moneyAvailable { get; private set; }
	
	// number of lives available to the player
	public int nOfLives = 10;
	
	// the current state of the game
	public GameState gameState;
	
	// a bool to check if the game already finished
	public bool gameFinished;
	
	// initialization method
	void Start () {
		
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
		CheckAndStartNewRound();
		
		// set game to running
		gameFinished = false;
		
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
				DestroyExistingMonsters();
				gameState = GameState.Lost;
			} else if (gameFinished && monsterList.Where(x => x != null).Count() == 0) {
				DestroyExistingMonsters();
				gameState = GameState.Won;
			}
			break;
		case GameState.Won:
			if (Input.GetMouseButtonUp(0)) {//restart
				Application.LoadLevel(Application.loadedLevel);
			}
			break;
		case GameState.Lost:
			if (Input.GetMouseButtonUp(0)) {//restart
				Application.LoadLevel(Application.loadedLevel);
			}
			break;
		default:
			break;
		}
	}
	
	private void initLevelFromXml() {
		
		/*foreach (var position in levelDataFromXML.Paths) {
            GameObject go = Instantiate(PathPrefab, position,
                Quaternion.identity) as GameObject;
            go.GetComponent<SpriteRenderer>().sortingLayerName = "Path";
            go.transform.parent = PathPiecesParent.transform;
        }*/
		
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
		
		// create castle object
		//playerCastle = new GameObject ();
		// TODO!!
		
		playerCastle = Instantiate(castlePrefab, levelData.castlePosition + fieldOffset,
		                           Quaternion.identity) as GameObject;
		playerCastle.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
		
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint")
			.OrderBy(x => x.name).Select(x => x.transform).ToArray();
		
		moneyAvailable = levelData.money;
	}
	
	private void CheckAndStartNewRound() {
		/*if (currentRoundIndex < levelDataFromXML.Rounds.Count - 1) {
            currentRoundIndex++;
            StartCoroutine(NextRound());
        } else {
            FinalRoundFinished = true;
        }*/
		
		StartCoroutine(newRound());
		gameFinished = true;
	}
	
	private void DestroyExistingMonsters() {
		//get all the enemies
		foreach (var item in monsterList) {
			if (item != null)
				Destroy(item.gameObject);
		}
	}
	
	IEnumerator newRound() {
		
		// player has 2 seconds to do something.
		yield return new WaitForSeconds(2f);
		
		// create new monsters
		int difficulty = PlayerPrefs.GetInt ("difficulty");
		if (difficulty == 0) {
			difficulty = 1;
		}
		
		// number of monsters depends on difficulty level
		int amount = 6 + difficulty * 4;
		// TODO: Use WAVE properties!
		
		
		// create "amount" of monsters
		for (int n = 0; n < amount; n++) {
			
			// create a new monster.
			GameObject monster = Instantiate(monsterPrefab, waypoints[0].transform.position, Quaternion.identity) as GameObject;
			
			
			Monster monsterComponent = monster.GetComponent<Monster>();
			
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
			
			// destroy the castle.
			Destroy(playerCastle);
			
		} else {
			
			// game still ongoing.
			// one live is lost
			nOfLives--;
		}
	}
	
	public List<Vector2> getWaypoints() {
		return levelData.waypointList;
	}
	
	
}
