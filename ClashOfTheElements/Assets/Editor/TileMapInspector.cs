using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(TileMapVisual))]
public class TileMapInspector : Editor {
	
	public override void OnInspectorGUI() {
		//base.OnInspectorGUI();
		DrawDefaultInspector();
		
		if(GUILayout.Button("Regenerate")) {
			TileMapVisual tileMap = (TileMapVisual)target;
			tileMap.BuildMesh();
		}
	}
}
