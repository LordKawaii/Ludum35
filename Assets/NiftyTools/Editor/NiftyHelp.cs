using UnityEngine;
using UnityEditor;
using System.Collections;

public class NiftyHelp : EditorWindow {

    [MenuItem("Tools/Nifty Tools/Help", false, 100)]
	public static void OpenWindow () {
		NiftyHelp window = (NiftyHelp)GetWindowWithRect<NiftyHelp>(
			new Rect(Screen.currentResolution.width/2-150,
		         Screen.currentResolution.height/2, 300, 200),
			true, "Nifty Tools Help");
	}

	void OnGUI () {



		EditorGUILayout.HelpBox("Check out the forum thread for help!", MessageType.Info);

		Link("Forum Thread:", "Unity3d Forums", "http://forum.unity3d.com/threads/nifty-tools-for-unity.276962/");
		//Link("Asset Store:", "    Unity3d Asset Store", "https://www.assetstore.unity3d.com/en/#!/search/Nifty%20Tools%20for%20Unity");
		CustomGUI.Splitter(2);

		GUIStyle s = new GUIStyle(EditorStyles.label);
		s.wordWrap = true;
		GUILayout.Label("Feel free to leave a reply on the forum thread if you run into problems.", s);

	}

	void Link (string t1, string t2, string link) {
		GUIStyle linkStyle = new GUIStyle(EditorStyles.label);
		linkStyle.normal.textColor = Color.blue;
		linkStyle.active.textColor = Color.blue;

		EditorGUILayout.BeginHorizontal();

		GUILayout.Label(t1);
		if(GUILayout.Button(t2, linkStyle))
			Application.OpenURL(link);

		EditorGUILayout.EndHorizontal();

	}

}

public class NiftyAbout : EditorWindow {

    [MenuItem("Tools/Nifty Tools/About", false, 101)]
	public static void OpenWindow () {
		NiftyAbout window = (NiftyAbout)GetWindowWithRect<NiftyAbout>(
			new Rect(Screen.currentResolution.width/2-150,
		         Screen.currentResolution.height/2, 300, 160),
			true, "About Nifty Tools");
	}
	
	void OnGUI () {

		GUIStyle s = new GUIStyle(EditorStyles.label);
		s.wordWrap = true;
		GUILayout.Label("Nifty Tools v1", s);
		CustomGUI.Splitter();
		GUILayout.Label("Nifty Tools is a collection of small tools that should make your work with Unity easier. It is my attempt to make Unity an even better piece of software.", s);
		GUILayout.Label("");
		EditorGUILayout.HelpBox("Nifty Tools is NOT a collection of high quallity Unity extensions but a collection of small 'hacks'.", MessageType.Warning);
	}

	
}
