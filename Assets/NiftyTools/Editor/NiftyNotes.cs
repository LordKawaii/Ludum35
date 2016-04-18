using UnityEngine;
using UnityEditor;
using System.Collections;

public class NiftyNotes : EditorWindow {

	public string text;
	Vector2 scollPos = Vector2.zero;

    [MenuItem("Tools/Nifty Tools/Nifty Notes %T", false, 1)]
	public static void OpenWindow () {
		NiftyNotes window = (NiftyNotes)GetWindow<NiftyNotes>(true, "Nifty Notes");
		window.OnEnable();
	}

	void OnGUI () {

		GUIStyle style = new GUIStyle(EditorStyles.textArea);
		style.wordWrap = true;

		scollPos = EditorGUILayout.BeginScrollView(scollPos);
		text = EditorGUILayout.TextArea(text, style, GUILayout.ExpandHeight(true));
		EditorGUILayout.EndScrollView();
	}

	void OnDisable () {
		EditorPrefs.SetString("NiftyNotesText", text);
	}

	void OnEnable () {
		if(!EditorPrefs.HasKey("NiftyNotesText")) return;
		text = EditorPrefs.GetString("NiftyNotesText");
	}

}
