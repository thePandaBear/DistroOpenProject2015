using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMapVisual))]
public class TileMapMouse : MonoBehaviour {
	
    // private object of TileMap
	TileMapVisual _tileMap;
	
    // will hold the position of the tile mouse is on 
	Vector2 currentTileCoord;
	
    // object for highlighting cube 
	public Transform selectionCube;

    // tower prefeb
    public Tower towerPrefab;

    //offset to hit center of tile
    private Vector2 tileCenterOffset = new Vector2(0.5f, 0.5f);

    private TileMapData mapData;

    //The game manager
    private GameManager gameManager;

	void Start() {
		_tileMap = GetComponent<TileMapVisual>();
        mapData = _tileMap.getMapData();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

	// Update is called once per frame
	void Update () {
        bool preventBuild = false;
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hitInfo;
		
		if( GetComponent<Collider>().Raycast( ray, out hitInfo, Mathf.Infinity ) ) {
            // get coordinates of tile 
            int x = Mathf.FloorToInt( hitInfo.point.x / _tileMap.tileSize);
			int y = Mathf.FloorToInt( hitInfo.point.y / _tileMap.tileSize);
			Debug.Log ("Tile: " + x + ", " + y);
			
			currentTileCoord.x = x;
			currentTileCoord.y = y;

            selectionCube.transform.position = currentTileCoord;
		}
		else {
            preventBuild = true;
		}
		
		if(Input.GetMouseButtonDown(0)){
			Debug.Log ("Click!");

            //check if tile is free and not on path
            if (!mapData.checkIsPath((int)currentTileCoord.x, (int)currentTileCoord.y) && !mapData.checkForTower((int)currentTileCoord.x, (int)currentTileCoord.y) && !preventBuild) {

                //check if enough gold is available and if so pay for the tower
                if (gameManager.payForTower()) {
                    //update TileMapData
                    mapData.setTowerBool((int)currentTileCoord.x, (int)currentTileCoord.y, true);
                    //build Tower
                    Tower t = Instantiate(towerPrefab);
                    t.transform.position = currentTileCoord + tileCenterOffset;
                }
            }
		}

    }
}
