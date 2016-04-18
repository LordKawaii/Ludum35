using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NiftyPrefabPlacer : EditorWindow {

	private GameObject currentPrefab;
	private Rotation currentRotation;
	private bool togglePlace;
	private Tool oldTool = Tool.None;

	private enum Rotation{
		Prefab,
		Normal,
		Identity
	}

    [MenuItem("Tools/Nifty Tools/Nifty Prefab Placer", false, 1)]
	public static void OpenWindow () {
		NiftyPrefabPlacer window = (NiftyPrefabPlacer)GetWindow<NiftyPrefabPlacer>("Nifty Placer");
		window.position = new Rect(Screen.currentResolution.width / 2 - 150, Screen.currentResolution.height / 2- 61, 300, 123);
	}

	void OnEnable(){
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}
	void OnDisable(){
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
		if(togglePlace)
			Tools.current = oldTool;
	}

	void OnGUI () {

		EditorGUILayout.LabelField("Prefab:");
		currentPrefab = (GameObject)EditorGUILayout.ObjectField(currentPrefab, typeof(GameObject), true);
		if(currentPrefab == null){
			EditorGUILayout.HelpBox("Select a Prefab.", MessageType.Info);
			return;
		}

		currentRotation = (Rotation)EditorGUILayout.EnumPopup("Rotation:", currentRotation);
		if(togglePlace) GUI.color = Color.green; else GUI.color = Color.grey;
		if(GUILayout.Button("Toggle Place", GUILayout.Height(45))) togglePlace = !togglePlace;
		GUI.color = Color.white;

	}

	void Update () {

		if(oldTool != Tool.None){
			Tools.current = oldTool;
			oldTool = Tool.None;
		}

		if(!togglePlace) return;
		if(oldTool == Tool.None) oldTool = Tools.current;

		Tools.current = Tool.View;
	}

	public void OnSceneGUI (SceneView view) {

		if(Event.current.type == EventType.mouseDown && Event.current.button == 0 && togglePlace){
			PlacePrefab(Event.current.mousePosition, view);
		}

	}

	public void OnInspectorUpdate () {
		Repaint();
	}

	void PlacePrefab (Vector2 mousePos, SceneView v) {

		mousePos.y = v.position.height - mousePos.y;

		Ray ray = Camera.current.ScreenPointToRay(mousePos);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit)){

			Vector3 position = hit.point;
			Quaternion rotation = currentPrefab.transform.rotation;

			switch(currentRotation){
			case Rotation.Normal:
				rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				break;
			case Rotation.Identity:
				rotation = Quaternion.identity;
				break;
			}

			GameObject go =(GameObject) Instantiate(currentPrefab, hit.point, rotation);
			go.name = go.name.Replace("(Clone)", "");

		}

	}

}
