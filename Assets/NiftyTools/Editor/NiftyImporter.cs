using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NiftyImporter : EditorWindow {

	public static bool enabled = true;
	public static List<string> fileExtensions = new List<string>();
	public static List<string> filePaths = new List<string>();

	private Vector2 scrollPos = Vector2.zero;
	private bool removeMode = false;

    [MenuItem("Tools/Nifty Tools/Nifty Importer", false, 1)]
	public static void OpenWindow () {
		NiftyImporter window = (NiftyImporter)GetWindow<NiftyImporter>(true, "Nifty Importer");
		NiftyImporter.LoadRules ();
	}

	void OnGUI () {

		GUI.color = Color.gray;
		if(enabled) GUI.color = Color.green;
		if(GUILayout.Button("Toggle Importer", GUILayout.Height(25))) enabled = !enabled;
		GUI.color = Color.white;

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

		List<int> idsToRemove = new List<int>();
		for(int i = 0; i < fileExtensions.Count; i++){
			CustomGUI.Splitter(3);
			EditorGUILayout.LabelField("File extension (seperated with , no spaces) or keys (no dot):");
			fileExtensions[i] = EditorGUILayout.TextField(fileExtensions[i]);
			EditorGUILayout.BeginHorizontal();
			filePaths[i] = EditorGUILayout.TextField("Folder Path:", filePaths[i]);

			if(!removeMode){
				if(GUILayout.Button("...", GUILayout.Width(20))){
					string s = EditorUtility.OpenFolderPanel("Choose Folder", Application.dataPath, "");
					if(!string.IsNullOrEmpty(s)) filePaths[i] = s;
				}
			}
			else{
				GUI.color = Color.red;
				if(GUILayout.Button("X", GUILayout.Width(20))){
					idsToRemove.Add(i);
				}
				GUI.color = Color.white;
			}
			EditorGUILayout.EndHorizontal();
		}
		if(idsToRemove.Count > 0){
			foreach(int i in idsToRemove){
				fileExtensions.RemoveAt(i);
				filePaths.RemoveAt(i);
			}
		}

		EditorGUILayout.EndScrollView();

		if(!removeMode){
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Add")){
				fileExtensions.Add("");
				filePaths.Add("");
			}
			if(GUILayout.Button("Remove")){
				removeMode = true;
			}
			GUILayout.EndHorizontal();
		}
		else{
			if(GUILayout.Button("Ok")){
				removeMode = false;
			}
		}
	}

	public void OnDisable () {
		NiftyImporter.SaveRules ();
	}

	public static void SaveRules () {

		if(fileExtensions.Count <= 0){
			EditorPrefs.SetString ("NiftyImporterRules", "");
			return;
		}

		string s1 = "";
		string s2 = "";
		string output = "";
		foreach (string s in fileExtensions) {
			s1+=s+":";
		}
		s1 = s1.Remove(s1.Length-1, 1);
		foreach (string s in filePaths) {
			s2+=s+"§";
		}
		s2 = s2.Remove(s2.Length-1, 1);

		output = s1 + "|" + s2;

		EditorPrefs.SetString ("NiftyImporterRules", output);

	}


	public static void LoadRules () {

		if(!EditorPrefs.HasKey("NiftyImporterRules")) return;

		fileExtensions.Clear ();
		filePaths.Clear ();

		string input = EditorPrefs.GetString ("NiftyImporterRules");
		if(string.IsNullOrEmpty(input)) return;

		string s1 = input.Split(new string[]{"|"}, System.StringSplitOptions.None)[0];
		string s2 = input.Split(new string[]{"|"}, System.StringSplitOptions.None)[1];

		foreach (string s in s1.Split(new string[]{":"}, System.StringSplitOptions.None)) {
			fileExtensions.Add(s);
		}
		foreach (string s in s2.Split(new string[]{"$"}, System.StringSplitOptions.None)) {
			filePaths.Add(s);
		}
	}
}

public class NiftyImporterProcessor : AssetPostprocessor {

	public static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedPaths) {
		Process(importedAssets);
	}

	static void Process (string[] paths) {
		if(paths.Length <= 0) return;
		if(NiftyImporter.enabled == false) return;

		foreach(string p in paths){
			if(p.EndsWith(".unity")) continue;

			string path = Application.dataPath.Replace("/Assets", "/") + p;
			string extension = Path.GetExtension(path);

			for(int i = 0; i < NiftyImporter.fileExtensions.Count; i++){

				string[] e = NiftyImporter.fileExtensions[i].Split(new string[]{","}, System.StringSplitOptions.None);
				List<string> eL = new List<string>();
				List<string> keys = new List<string>();
				foreach(string s in e){
					if(s.StartsWith("."))
						eL.Add(s);
					else keys.Add(s);
				}

				if(eL.Contains(extension)){
					if(keys.Count > 0){
						string fileName = Path.GetFileNameWithoutExtension(path);
						bool containsOne = false;
						foreach(string s in keys)
							if(fileName.Contains(s)) containsOne = true;
						if(!containsOne) break;
					}

					MoveFile(path, NiftyImporter.filePaths[i]);
					break;
				}
			}

		}

	}

	static void MoveFile (string filePath, string path){
		try{
			File.Move(filePath, Path.Combine(path, Path.GetFileName(filePath)));
		}
		catch(System.Exception e){

		}
	} 
}

