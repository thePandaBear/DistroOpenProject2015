using UnityEngine;
using System.Collections;


[RequireComponent (typeof (MeshFilter), typeof (MeshRenderer))]
public class Grid : MonoBehaviour {

    public int xSize, ySize;
    private Vector3[] vertices;

    private Mesh mesh;

    // Geneare vertices with wait time 
    private IEnumerator Generate() {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] texturePos = new Vector2[vertices.Length];
        for (int i = 0, y=0; y < ySize; y++) {
            for (int x = 0; x < xSize; x++,i++) {
                vertices[i] = new Vector3(x, y);
                texturePos[i] = new Vector2((float) x / xSize, (float) y / ySize);
                
            }
        }
        mesh.vertices = vertices;
        // uv = main texture mesh position
        mesh.uv = texturePos;

        // triangle stuff
        int[] triangles = new int[xSize*ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++) {
            for (int x = 0; x < xSize; x++, ti += 6, vi++){
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize ;
                triangles[ti + 5] = vi + xSize + 1;
                mesh.triangles = triangles;
                yield return wait;
            }

            mesh.RecalculateNormals();
        }

        

       
    }


    // Draw on each field a Sphere
    private void OnDrawGizmos() {
        if (vertices == null) {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++) {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    // when in play create map 
    private void Awake() {
        StartCoroutine( Generate());
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
