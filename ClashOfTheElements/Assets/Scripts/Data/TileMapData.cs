using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMapData: MonoBehaviour {

    /* ID 0  := Grass
       ID 1  := Path
     */

    // One Tile on the Tile Map 
    protected class DTile {
        protected int pos_x;
        protected int pos_y;  
        public  bool Iswalkable;
        public  bool has_tower_on_it;
        protected  int tileGraphicId;

        public int getGraphicID(){
            return tileGraphicId;
        }

    
    }

    // Data of walkable tile 
    protected class DWalkableTile : DTile {
        public DWalkableTile(int x_pos, int y_pos) {
            this.pos_x = x_pos;
            this.pos_y = y_pos;
            Iswalkable = true;
            has_tower_on_it = false;
            tileGraphicId = 1;
        }
    }

    // Data of Buildplaces tile 
    protected class DBuildTile : DTile {
        public DBuildTile(int x_pos, int y_pos) {
            this.pos_x = x_pos;
            this.pos_y = y_pos;
            Iswalkable = false;
            has_tower_on_it = false;
            tileGraphicId = 0;
        }
    }



    // gameManager
    GameObject go;
    GameManager gameManagerScript;

    int size_x;
    int size_y;
   
    // all tiles of the map 
    DTile [,] map_tiles;

    // tile map
    public TileMapData(int x_size, int y_size)
    {

        this.size_x = x_size;
        this.size_y = y_size;


        map_tiles = new DTile[size_x, size_y];

        // gameManager Stuff
        go = GameObject.Find("GameManager(Clone)");
        gameManagerScript = go.GetComponent<GameManager>();
        
     //   Debug.Log("ITs not null");
       

        // Construct Data map with Tiles from XML 

        // Example without XML
        for (int x = 0; x < size_x; x++)
        {
            for (int y = 0; y < size_y; y++)
            {
                map_tiles[x, y] = new DBuildTile(x, y);
            }
        }

        if (gameManagerScript.getWaypoints() != null) {
            List<Vector2> wayPointsList = gameManagerScript.getWaypoints();
            // add path to tile 
        //    Debug.Log("Starting Pathing");
            int mini;
            int maxi;
            for (int i = 0; i < wayPointsList.Count - 1; i++)
            {
                Vector2 current = wayPointsList[i];
                Vector2 next = wayPointsList[i + 1];
                // vertical
                if (current.x == next.x)
                {
                    // loop from start to next mark path tile
                    int x = (int)current.x;
                    mini = Mathf.Min((int)current.y, (int)next.y);
                    maxi = Mathf.Max((int)current.y, (int)next.y);
                    for (int y = mini; y < maxi + 1; y++)
                    {
                        map_tiles[x, y] = new DWalkableTile(x, y);
                    }
                }
                // horizontal 
                else {
                    int y = (int)current.y;
                    mini = Mathf.Min((int)current.x, (int)next.x);
                    maxi = Mathf.Max((int)current.x, (int)next.x);
                    // loop from start to next mark
                    for (int x = mini; x < maxi + 1; x++)
                    {
                        map_tiles[x, y] = new DWalkableTile(x, y);
                    }
                }
            }
        }
    }
 
    // return ID of Tile 
    public int GetTileID(int x, int y)
    {
        return map_tiles[x, y].getGraphicID();
    }

    // checks if a tower is build 
    public bool checkForTower(int x, int y) {
        return map_tiles[x, y].has_tower_on_it;
    }

    // if tile is a path
    public bool checkIsPath(int x, int y)
    {
        return map_tiles[x, y].Iswalkable;
    }

    // informs tile that is has a tower
    public void setTowerBool(int x, int y, bool theBool) {
        if(!map_tiles[x, y].Iswalkable) {
            map_tiles[x, y].has_tower_on_it = theBool;
        }
    }
}
