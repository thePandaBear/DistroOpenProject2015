using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

    [HideInInspector]
    public static GameManager Instance { get; private set; }

    void Awake() {
        Instance = this;
    }

    //offset to hit center of a tile
	private Vector2 tileCenterOffset = new Vector2 (0.5f, 0.5f);
    //enemies on screen
    public List<GameObject> monsters;

    // castle gameObject
    GameObject castle;

    //file pulled from resources
    private LevelDataFromXML levelDataFromXML;

    //prefabs
    public GameObject monsterPrefab;
    public GameObject castlePrefab;

    //list of waypoints
    public Transform[] waypoints;
    private GameObject waypointsParent;

    //Game parameters
    [HideInInspector]
    public int moneyAvailable { get; private set; }
    public int lives = 10;
    [HideInInspector]
    public GameState currentGameState;
    public bool finalRoundFinished;

    // Use this for initialization
    void Start () {
        monsters = new List<GameObject>();

        levelDataFromXML = Utilities.ReadXMLFile();

        waypointsParent = GameObject.Find("Waypoints");

        CreateLevelFromXML();
        currentGameState = GameState.Playing;
        CheckAndStartNewRound();

        finalRoundFinished = false;
    }
	
	// Update is called once per frame
	void Update () {

        switch (currentGameState) {
            //start state, on tap, start the game and spawn carrots!
            case GameState.Start:
                if (Input.GetMouseButtonUp(0)) {
                    currentGameState = GameState.Playing;
                    StartCoroutine(NextRound());
                }
                break;
            case GameState.Playing:
                if (lives == 0) //we lost
                {
                    //no more rounds
                    StopCoroutine(NextRound());
                    DestroyExistingMonsters();
                    currentGameState = GameState.Lost;
                } else if (finalRoundFinished && monsters.Where(x => x != null).Count() == 0) {
                    DestroyExistingMonsters();
                    currentGameState = GameState.Won;
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

    private void CreateLevelFromXML() {

        /*foreach (var position in levelDataFromXML.Paths) {
            GameObject go = Instantiate(PathPrefab, position,
                Quaternion.identity) as GameObject;
            go.GetComponent<SpriteRenderer>().sortingLayerName = "Path";
            go.transform.parent = PathPiecesParent.transform;
        }*/

        for (int i = 0; i < levelDataFromXML.Waypoints.Count; i++) {
            GameObject go = new GameObject();
            go.transform.position = levelDataFromXML.Waypoints[i] + tileCenterOffset;
            go.transform.parent = waypointsParent.transform;
            go.tag = "Waypoint";
            go.name = "Waypoints" + i.ToString();
        }

        castle = Instantiate(castlePrefab, levelDataFromXML.Tower + tileCenterOffset,
            Quaternion.identity) as GameObject;
        castle.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";

        waypoints = GameObject.FindGameObjectsWithTag("Waypoint")
            .OrderBy(x => x.name).Select(x => x.transform).ToArray();

        moneyAvailable = levelDataFromXML.InitialMoney;
    }

    private void CheckAndStartNewRound() {
        /*if (currentRoundIndex < levelDataFromXML.Rounds.Count - 1) {
            currentRoundIndex++;
            StartCoroutine(NextRound());
        } else {
            FinalRoundFinished = true;
        }*/

        StartCoroutine(NextRound());
        finalRoundFinished = true;
    }

    private void DestroyExistingMonsters() {
        //get all the enemies
        foreach (var item in monsters) {
            if (item != null)
                Destroy(item.gameObject);
        }
    }

    IEnumerator NextRound() {
        //give the player 2 secs to do stuff
        yield return new WaitForSeconds(2f);
        //get a reference to the next round details
  
        for (int i = 0; i < 10; i++) {//spawn a new Monster
            GameObject Monster = Instantiate(monsterPrefab, waypoints[0].transform.position, Quaternion.identity) as GameObject;
            Monster MonsterComponent = Monster.GetComponent<Monster>();
            //add it to the list and wait till you spawn the next one
            monsters.Add(Monster);
            yield return new WaitForSeconds(1f / (1));
        }

    }

    public void takeDamage() {
        if (lives > 1)
        {
            lives--;
        }
        else {
            lives = 0;
            Destroy(castle);
        }
    }
}
