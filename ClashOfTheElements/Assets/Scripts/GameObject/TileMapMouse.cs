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
	
	void Start() {
		_tileMap = GetComponent<TileMapVisual>();
	}

	// Update is called once per frame
	void Update () {
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
            //Debug.Log("Coordinate x: " + selectionCube.transform.position.x + ", y: " + selectionCube.transform.position.y);
		}
		else {
			// Hide selection cube?
		}
		
		if(Input.GetMouseButtonDown(0)) {
			Debug.Log ("Click!");
            Tower t = Instantiate(towerPrefab);
            t.transform.position = currentTileCoord + tileCenterOffset;
		}
	}
}
