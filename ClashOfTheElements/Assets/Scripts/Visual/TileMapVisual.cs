using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMapVisual : MonoBehaviour {
	
    // amount of tiles 
	public int size_x;
	public int size_y;
	public float tileSize = 1.0f;


    // Texture for Tiles
    public Texture2D terrainTiles;
    public int tileResolution;


	
	// Use this for initialization
	void Start () {
		BuildMesh();
	}

    // get data from sprite sheet
    Color[][] ChopUpTiles()
    {
        int numTilesPerRow = terrainTiles.width / tileResolution;
        int numRows = terrainTiles.height / tileResolution;

        Color[][] tiles = new Color[numTilesPerRow * numRows][];

        for (int y = 0; y < numRows; y++){
            for (int x = 0; x < numTilesPerRow; x++){
                tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution);
            }
        }

        return tiles;
    }



    void BuildTexture() {

        // get Data TileMap 
        TileMapData map = new TileMapData(size_x, size_y);

        int texWidth = size_x*tileResolution;
        int texHeight = size_y*tileResolution;

        // object for our texture
        Texture2D texture = new Texture2D(texWidth,texHeight);
        
        // stores texture of tiles
        Color[][] tiles = ChopUpTiles();
        // set texture for one tile 
        for (int y = 0; y < size_y; y++) {
            for(int x = 0; x < size_x; x++){
                Color[] p = tiles[map.GetTileAt(x,y)];
                texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, p);
           
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        // Apply pixel to texture
        texture.Apply();

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        mesh_renderer.sharedMaterials[0].mainTexture = texture; 

        Debug.Log("Done Texture!");
    } 

	public void BuildMesh() {
      //  transform.GetComponent<Transform>().position = new Vector3(-size_x / 2, size_y / 2, 0);
		int numTiles = size_x * size_y;
		int numTris = numTiles * 2;
		
		int vsize_x = size_x + 1;
		int vsize_y = size_y + 1;
		int numVerts = vsize_x * vsize_y;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numTris * 3 ];

		int x, y;
		for(y=0; y < vsize_y; y++) {
			for(x=0; x < vsize_x; x++) {
				vertices[ y * vsize_x + x ] = new Vector3( x*tileSize,y*tileSize,0);
				normals[ y * vsize_x + x ] = Vector3.forward;//maybe opposite
				uv[ y * vsize_x + x ] = new Vector2( (float)x / size_x, (float)y / size_y );
			}
		}
        	Debug.Log ("Done Verts!");
        // local variables for the loop initialized with 0
        int squareIndex = 0;
        int triOffset = 0;
        int temp = 0;
        int temp2 = 0;

        // initializing triangle 
        for (y=0; y < size_y; y++) {
			for(x=0; x < size_x; x++) {
				squareIndex = y * size_x + x;
				triOffset = squareIndex * 6;
                temp = y * vsize_x + x;
                temp2 = temp + vsize_x;
                triangles[triOffset + 0] = temp  + 0;
				triangles[triOffset + 1] = temp2 + 0;
				triangles[triOffset + 2] = temp2 + 1;
				
				triangles[triOffset + 3] = temp  +  0;
				triangles[triOffset + 4] = temp2 +  1;
				triangles[triOffset + 5] = temp  +  1;
			}
		}
		
		Debug.Log ("Done Triangles!");
		
		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;

        Debug.Log ("Done Mesh!");

        BuildTexture();	
	}
	
	
}
