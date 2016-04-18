using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;


public class NiftyScenes : EditorWindow {
	
	Vector2 scrollPos = new Vector2();
	List<string> paths = new List<string>();
	List<string> names = new List<string>();
	List<string> hiddenScenes = new List<string>();

	Texture2D hide_icon;
	Texture2D unhide_icon;

    [MenuItem("Tools/Nifty Tools/Nifty Scenes", false, 1)]
	public static void OpenWindow () {
		NiftyScenes window = (NiftyScenes)GetWindow<NiftyScenes>("Nifty Scenes");
		window.OnEnable();
	}

	void OnEnable () {
		RefreshScenes();

		LoadConfig();

		hide_icon = (Texture2D)EditorGUIUtility.Load("Icons/hide_icon.png");
		unhide_icon = (Texture2D)EditorGUIUtility.Load("Icons/unhide_icon.png");
	}

	void OnDisable () {
		SaveConfig();
	}

	void OnDestroy () {
		SaveConfig();
	}

	void SaveConfig() {
		if(hiddenScenes.Count > paths.Count){
			EditorPrefs.SetString("NifftyScenesHide", "");
			return;
		}

		if(hiddenScenes.Count > 0){
			string hide = "";
			foreach(string s in hiddenScenes){
				hide+=s + ",";
			}
			hide = hide.Remove(hide.Length-1, 1);

			EditorPrefs.SetString("NifftyScenesHide", hide);
		}
		else EditorPrefs.SetString("NifftyScenesHide", "");
	}

	void LoadConfig () {
		hiddenScenes.Clear();
		if(EditorPrefs.HasKey("NifftyScenesHide")){
			string hide = EditorPrefs.GetString("NifftyScenesHide");
			if(hide == "") return;
			string[] scenes = hide.Split(new string[]{","}, System.StringSplitOptions.None);
			foreach(string s in scenes)
				hiddenScenes.Add(s);
		}
	}

	void OnGUI () {

		GUILayout.BeginHorizontal();
		Color c = GUI.color;
		GUI.color = Color.gray;
		if(GUILayout.Button("Refresh")) RefreshScenes();
		GUI.color = c;
		if(GUILayout.Button(unhide_icon, GUILayout.Width(20), GUILayout.Height(20))) hiddenScenes.Clear();
		GUILayout.EndHorizontal();

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		
		for(int i = 0; i < paths.Count; i++){
			c = GUI.color;
			
			string p = EditorApplication.currentScene;
			p = p.Replace(@"/", @"\");
			if(p == paths[i]){
				GUI.color = Color.blue;
				GUI.enabled = false;
			}

			if(hiddenScenes.Contains(paths[i])) continue;

			GUILayout.BeginHorizontal();
			if(GUILayout.Button(names[i])){
				EditorApplication.SaveCurrentSceneIfUserWantsTo();
				EditorApplication.OpenScene(paths[i]);
				RefreshScenes();
			}
			if(GUILayout.Button(hide_icon, GUILayout.Width(20), GUILayout.Height(20))) hiddenScenes.Add(paths[i]);
			GUILayout.EndHorizontal();

			GUI.enabled = true;
			GUI.color = c;
		}
		
		EditorGUILayout.EndScrollView();
	}
	
	void RefreshScenes () {
		paths = new List<string>();
		names = new List<string>();
		
		DirectoryInfo dir = new DirectoryInfo(Application.dataPath);
		FileInfo[] files = dir.GetFiles("*.unity", SearchOption.AllDirectories);
		foreach(FileInfo file in files) {
			string name = file.Name.Replace(".unity", "");
			int i = file.FullName.IndexOf("Assets");
			if(i < 0) i = 0;
			string path = file.FullName.Substring(i, file.FullName.Length - i);
			
			paths.Add(path);
			names.Add(name);
		}
	}
	
}
