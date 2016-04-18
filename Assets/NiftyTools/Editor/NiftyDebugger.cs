using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

public class NiftyDebugger : EditorWindow {

	Vector2 scrollPos = new Vector2();
	List<bool> foldouts = new List<bool>();
	List<List<bool>> componetFoldouts = new List<List<bool>>();

	[MenuItem("Tools/Nifty Tools/Nifty Debugger", false, 1)]
	public static void OpenWindow () {
		NiftyDebugger window = (NiftyDebugger)GetWindow<NiftyDebugger>("Nifty Debugger");
	}

	void Update () {

		if(Selection.gameObjects.Length <= 0){
			return;
		}


	}

	public void OnInspectorUpdate () {
		Repaint();
	}

	void OnGUI () {

		if(Selection.gameObjects.Length <= 0){
			EditorGUILayout.HelpBox("Please select an Object from the hierachy.", MessageType.Info, true);
			return;
		}

		GUIStyle fold = new GUIStyle(EditorStyles.foldout);
		fold.fontStyle = FontStyle.Bold;

		scrollPos = GUILayout.BeginScrollView(scrollPos);
		for(int i = 0; i < Selection.gameObjects.Length; i++){
			GameObject go = Selection.gameObjects[i];
			if(foldouts.Count <= i){ foldouts.Add(false); componetFoldouts.Add(new List<bool>());}

			CustomGUI.Splitter();
			foldouts[i] = EditorGUILayout.Foldout(foldouts[i], go.name, fold);
			if(!foldouts[i]) continue;

			GUILayout.Label("Position: " + go.transform.position.ToString());
			GUILayout.Label("Rotation: " + go.transform.rotation.ToString());
			GUILayout.Label("Scale: " + go.transform.localScale.ToString());
			GUILayout.Label("Tag: " + go.tag);
			GUILayout.Label("Layer: " + LayerMask.LayerToName(go.layer) + "(" + go.layer.ToString() + ")");
			GUILayout.Label("Active: " + go.activeSelf.ToString());
			EditorGUILayout.Separator();

			GUILayout.Label("Components:", new GUIStyle(EditorStyles.boldLabel));
			Component[] components = go.GetComponents(typeof(Component));
			for(int j = 0; j < components.Length; j++) {
				Component c = components[j];
				if(componetFoldouts[i].Count <= j) componetFoldouts[i].Add(false);

				componetFoldouts[i][j] = EditorGUILayout.Foldout(componetFoldouts[i][j], c.GetType().ToString());
				if(!componetFoldouts[i][j]) continue;

				BindingFlags bindingFlags = BindingFlags.Public |
						BindingFlags.NonPublic |
						BindingFlags.Instance |
						BindingFlags.Static;

				bool content = false;
				foreach(FieldInfo field in c.GetType().GetFields(bindingFlags)){
					if(field == null) EditorGUILayout.HelpBox("Field is null.", MessageType.Error, true);
					object obj = null;
					try{
						obj = field.GetValue(c);
					}
					catch(System.Exception e){

					}
					string v = "null";
					if(obj != null) v = obj.ToString(); 
					EditorGUILayout.SelectableLabel(field.Name + ": " + v);
					content = true;
				}
				foreach(PropertyInfo prop in c.GetType().GetProperties(bindingFlags)){
					if(prop == null) EditorGUILayout.HelpBox("Property is null.", MessageType.Error, true);
					object obj = null;
					bool error = false;
					try{
						obj = prop.GetValue(c, null);
					}
					catch(System.Exception e){
						error = true;
					}
					string v = "null";
					if(obj != null) v = obj.ToString();

					if(!error)
						EditorGUILayout.SelectableLabel(prop.Name + ": " + v);
					else EditorGUILayout.HelpBox("Error for field '" + prop.Name + "'. Value is null.", MessageType.Error, true);

					content = true;
				}
				if(!content)
					EditorGUILayout.HelpBox("Nothing to display.", MessageType.Info, true);

			}
		}
		GUILayout.EndScrollView();
		
	}
	
}

static class CustomGUI {
	public static readonly GUIStyle splitter;
	static CustomGUI() {
		GUISkin skin = GUI.skin;
		splitter = new GUIStyle();
		splitter.normal.background = EditorGUIUtility.whiteTexture;
		splitter.stretchWidth = true;
		splitter.margin = new RectOffset(0, 0, 7, 7);
	}
	private static readonly Color splitterColor = EditorGUIUtility.isProSkin ? new Color(0.157f, 0.157f, 0.157f) : new Color(0.5f, 0.5f, 0.5f);
	// GUILayout Style
	public static void Splitter(Color rgb, float thickness = 1) {
		Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitter, GUILayout.Height(thickness));
		if (Event.current.type == EventType.Repaint) {
			Color restoreColor = GUI.color;
			GUI.color = rgb;
			splitter.Draw(position, false, false, false, false);
			GUI.color = restoreColor;
		}
	}
	public static void Splitter(float thickness, GUIStyle splitterStyle) {
		Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitterStyle, GUILayout.Height(thickness));
		if (Event.current.type == EventType.Repaint) {
			Color restoreColor = GUI.color;
			GUI.color = splitterColor;
			splitterStyle.Draw(position, false, false, false, false);
			GUI.color = restoreColor;
		}
	}
	public static void Splitter(float thickness = 1) {
		Splitter(thickness, splitter);
	}
	// GUI Style
	public static void Splitter(Rect position) {
		if (Event.current.type == EventType.Repaint) {
			Color restoreColor = GUI.color;
			GUI.color = splitterColor;
			splitter.Draw(position, false, false, false, false);
			GUI.color = restoreColor;
		}
	}
}
