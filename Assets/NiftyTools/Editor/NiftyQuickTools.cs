using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NiftyQuickTools : EditorWindow {

	Vector2 scrollPos = Vector2.zero;
	List<string> apps = new List<string>();
	bool removeMode = false;

	Texture2D up_icon;
	Texture2D down_icon;
	float default_w;

    [MenuItem("Tools/Nifty Tools/Nifty Quick Tools", false, 1)]
	public static void OpenWindow () {
		NiftyQuickTools window = (NiftyQuickTools)GetWindow<NiftyQuickTools>("Nifty Quick Tools");
		window.OnEnable();
	}

	void OnGUI () {

		scrollPos = GUILayout.BeginScrollView(scrollPos);

		GUIStyle appStyle = new GUIStyle(GUI.skin.button);
		appStyle.fontSize = 15;

		Color c = GUI.color;

		List<string> appsToRemove = new List<string>();
		Color c1 = removeMode ? new Color(1, 0.5f, 0.5f) : Color.white;
		Color c2 = removeMode ? Color.red : Color.gray;

		foreach(string app in apps){

			GUI.color = Color.Lerp(c1, c2, (1.0f / apps.Count) * apps.IndexOf(app));
			string name = Path.GetFileName(app).Replace(".exe", "");
			GUILayout.BeginHorizontal();

			Rect r = new Rect();
			if(!removeMode)
				r = GUILayoutUtility.GetRect(new GUIContent(name), appStyle, GUILayout.Height(30));
			else
				r = GUILayoutUtility.GetRect(new GUIContent(name), appStyle, GUILayout.Height(30), GUILayout.Width(default_w-22));
			if(GUI.Button(r, name, appStyle)){
				if(!removeMode)
					System.Diagnostics.Process.Start(app);
				else appsToRemove.Add(app);
			}
			if(removeMode){
				GUILayout.BeginVertical();
				if(GUILayout.Button(up_icon, GUILayout.MaxWidth(18), GUILayout.MaxHeight(13))){ MoveElement(app, -1); break;}
				if(GUILayout.Button(down_icon, GUILayout.MaxWidth(18), GUILayout.MaxHeight(13))){ MoveElement(app, 1); break;}
				GUILayout.EndVertical();
			}
			GUILayout.EndHorizontal();
			if(!removeMode)
				default_w = r.width;
		}
		GUI.color = c;
		if(removeMode)
			foreach(string s in appsToRemove)
				apps.Remove(s);

		GUILayout.EndScrollView();

		if(!removeMode){
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Add")){

				string programmFiles = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
				string path = EditorUtility.OpenFilePanel("Choose Application", programmFiles, "exe");
				if(!string.IsNullOrEmpty(path))
					apps.Add(path);

			}
			if(GUILayout.Button("Edit")) removeMode = true;
			GUILayout.EndHorizontal();
		}
		else
			if(GUILayout.Button("Ok")) removeMode = false;


	}

	void OnEnable () {
		LoadConfig();

		up_icon = (Texture2D)EditorGUIUtility.Load("Icons/up_icon.png");
		down_icon = (Texture2D)EditorGUIUtility.Load("Icons/down_icon.png");
	}
	
	void OnDisable () {
		SaveConfig();
	}
	
	void OnDestroy () {
		SaveConfig();
	}
	
	void SaveConfig() {
		if(apps.Count > 0){
			string a = "";
			foreach(string s in apps){
				a+=s + ",";
			}
			a = a.Remove(a.Length-1, 1);
			
			EditorPrefs.SetString("NifftyQuickToolsApps", a);
		}
		else EditorPrefs.SetString("NifftyQuickToolsApps", "");
	}
	
	void LoadConfig () {
		apps.Clear();
		if(EditorPrefs.HasKey("NifftyQuickToolsApps")){
			string a = EditorPrefs.GetString("NifftyQuickToolsApps");
			if(a == "") return;
			string[] programms = a.Split(new string[]{","}, System.StringSplitOptions.None);
			foreach(string s in programms)
				apps.Add(s);
		}
	}

	void MoveElement(string app, int offset) {
		if(!apps.Contains(app)) return;

		int index = apps.IndexOf(app);
		int newIndex = index + offset;

		if(newIndex < 0 || newIndex >= apps.Count) return;

		apps.RemoveAt(index);
		apps.Insert(newIndex, app);

	}

}
