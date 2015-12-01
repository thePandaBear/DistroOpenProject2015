using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMapData{
    // Caution Sprite Types is hard coded
    /*  0 = Water 
        1 = Buildplace
        2 = Sand
        3 = Mountain
    */
 
    // Data for one Tile on the map
    protected class DTile {
        protected int pos_x;
        protected int pos_y;  
        protected  bool walkable;
        protected  bool has_tower_build;
        protected  int tileGraphicId;

        public int getGraphicID(){
            return tileGraphicId;
        }
       
    }

    // Data of walkable tile 
    protected class DWalkableTile : DTile {
        // Constructor
        public DWalkableTile(int x_pos, int y_pos) {
            this.pos_x = x_pos;
            this.pos_y = y_pos;
            walkable = true;
            has_tower_build = false;
            tileGraphicId = 2;
        }
    }

    // Data of Buildplaces tile 
    protected class DBuildTile : DTile {

        // constructur
        public DBuildTile(int x_pos, int y_pos) {
            this.pos_x = x_pos;
            this.pos_y = y_pos;
            walkable = false;
            has_tower_build = false;
            tileGraphicId = 1;
        }
    }


    int size_x;
    int size_y;

    // for better performance use 1D array
    DTile [,] map_tiles;

    //List of all Tiles

    public TileMapData(int x_size, int y_size) {
    
        this.size_x = x_size;
        this.size_y = y_size;

        map_tiles = new DTile[size_x, size_y];

        // Construct Data map with Tiles from XML 

        // Example without XML
     
        for (int x = 0; x < size_x; x++) {
            for (int y = 0; y < size_y; y++) {
                map_tiles[x,y] = new DWalkableTile(x, y);
            }
        }                    
    }

    // return graphic ID for texture 
    public int GetTileAt(int x, int y)
    {
        return map_tiles[x, y].getGraphicID();
    }

    // add more tile logic and stuff 
}
