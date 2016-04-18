using UnityEngine;
using UnityEditor;
using System.Collections;

public class NiftyGrid : EditorWindow {
	
	bool snap = false;
	bool snapPos = true;
	bool snapScale = true;
	float gridSize = 0.1f;
	Vector3 prevPosition = Vector3.zero;
	Vector3 prevScale = Vector3.zero;
	
	[MenuItem("Tools/Nifty Tools/Nifty Grid", false, 1)]
	public static void Open () {
		NiftyGrid window = (NiftyGrid)GetWindow(typeof(NiftyGrid));
		window.position = new Rect(Screen.currentResolution.width / 2 - 125, Screen.currentResolution.height / 2- 51, 251, 102);
	}
	
	void OnGUI () {
		Color c = GUI.color;
		if(gridSize <= 0) GUI.color = Color.red; 
		gridSize = EditorGUILayout.FloatField("Grid Size", gridSize);
		GUI.color = c;
		
		c = GUI.color;
		if(snap)GUI.color = Color.green;
		else GUI.color = Color.gray;
		if(GUILayout.Button("Toogle Snap", GUILayout.Height(45))) snap = !snap;

		c = Color.gray;
		GUI.color = c;
		
		GUI.enabled = snap;
		EditorGUILayout.BeginHorizontal();
		
		if(snapPos) GUI.color = snap ? Color.green : Color.gray;
		if(GUILayout.Button("Snap Position")) snapPos = !snapPos;
		
		GUI.color = c;
		if(snapScale) GUI.color = snap ? Color.green : Color.gray;
		if(GUILayout.Button("Snap Scale")) snapScale = !snapScale;
		
		EditorGUILayout.EndHorizontal();
		GUI.enabled = true;
		GUI.color = c;
	}
	
	void Update () {
		if(gridSize <= 0) snap = false;
		
		if(Selection.transforms.Length > 0 && !EditorApplication.isPlaying)
			if(snap && (Selection.transforms[0].position != prevPosition || 
				Selection.transforms[0].localScale != prevScale)) Snap();
	}
	
	void Snap () {
		try{
			
			for(int i = 0; i < Selection.transforms.Length; i++){
				
				if(snapPos){
					Vector3 t = Selection.transforms[i].transform.position;
					
					t.x = Round(t.x);
					t.y = Round(t.y);
					t.z = Round(t.z);
					
					Selection.transforms[i].transform.position = t;
				}
				if(snapScale){
					Vector3 s = Selection.transforms[i].transform.localScale;
					
					s.x = Round(s.x);
					s.y = Round(s.y);
					s.z = Round(s.z);
					
					Selection.transforms[i].transform.localScale = s;
				}
			}
			prevPosition = Selection.transforms[0].position;
			prevScale = Selection.transforms[0].localScale;
			
		}
		catch(System.Exception e){
			
		}
	}
	
	float Round (float f) {
		float output = 0;
		output = gridSize * Mathf.Round((f / gridSize));
		return output;
	}
	
}
