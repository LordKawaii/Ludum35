using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class NiftyTiles : EditorWindow {

	List<Sprite> TileMap = new List<Sprite>();
	Texture2D spriteAtlas;
	int sliceX = 32, sliceY = 32;
	int currentSliceX = -1, currentSliceY = -1;
	bool showGrid = false;
	Vector3 oldTP;
	int depht = 0;
	Vector2 currentTile = Vector2.zero;
	Vector2 scollPos = Vector2.zero;

	Texture2D grid;
	Texture2D bg;
	GUIStyle gridStyle;

    [MenuItem("Tools/Nifty Tools/Nifty Tiles", false, 1)]
	static void OpenWindow () {
		NiftyTiles window = (NiftyTiles)EditorWindow.GetWindow(typeof(NiftyTiles));
	}

	void OnEnable () {
		grid = (Texture2D)EditorGUIUtility.Load("Icons/niftyTilesGrid.png");
		bg = (Texture2D)EditorGUIUtility.Load("Icons/niftyTilesBG.png");
		gridStyle = new GUIStyle(EditorStyles.label);
		gridStyle.normal.background = bg;
		gridStyle.active.background = bg;
	}

	void OnGUI () {

		//Atlas
		
		GUILayout.Label("Tile Map Atlas");
		spriteAtlas = (Texture2D)EditorGUILayout.ObjectField(spriteAtlas as Object, typeof(Texture2D));
		
		//Slice
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Slice X: ");
			sliceX = EditorGUILayout.IntField(sliceX);
			GUILayout.Label("Slice Y: ");
			sliceY = EditorGUILayout.IntField(sliceY);
		GUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();

		//Depht

		depht = EditorGUILayout.IntSlider("Depht:", depht, -10, 10);

		//Buttons
		
		GUILayout.BeginHorizontal();

		bool isSliced = true;
		if(TileMap == null || currentSliceX != sliceX || currentSliceY != sliceY) isSliced = false;

		if(!isSliced)GUI.color = Color.red;
		if(GUILayout.Button("Slice")){
			TileMap = new List<Sprite>();
			for(int x = 0;x < spriteAtlas.width;x+= sliceX){
				for(int y = 0;y < spriteAtlas.height;y+= sliceY){
					TileMap.Add(Sprite.Create(spriteAtlas, new Rect(x, y, sliceX, sliceY), new Vector2(0,0), sliceX));
				}
			}
			showGrid = true;
			currentSliceX = sliceX;
			currentSliceY = sliceY;
		}

		if(!isSliced){
			showGrid = false;
			GUI.enabled = false;
		}

		if(showGrid) GUI.color = Color.green;
		else GUI.color = Color.gray;
		if(GUILayout.Button("Toggle Edit Mode")) showGrid = !showGrid;
		GUI.color = Color.white;
		GUI.enabled = true;

		GUILayout.EndHorizontal();

		//Grid
		
		if(spriteAtlas != null && sliceX > 0 && sliceY > 0){
			
			FocusWindowIfItsOpen<NiftyTiles>();

			GUI.BeginGroup(new Rect(10, 100, spriteAtlas.width, spriteAtlas.height));
			scollPos = GUI.BeginScrollView(new Rect(0, 0, position.width - 8, position.height - 100), scollPos, new Rect(0,0,spriteAtlas.width, spriteAtlas.height));

			for(int i = 0;i < spriteAtlas.height;i+= sliceY){
					for(int j = 0;j < spriteAtlas.width;j+= sliceX){
						if(GUI.Button(new Rect(j, i, sliceX, sliceY),"", gridStyle))
							currentTile = new Vector2(j, i);
					}
			}
			
			GUI.DrawTexture(new Rect(0, 0, spriteAtlas.width, spriteAtlas.height), spriteAtlas);
			
			for(int i = 0;i < spriteAtlas.height;i+= sliceY){
					for(int j = 0;j < spriteAtlas.width;j+= sliceX){
						if(new Vector2(j, i) == currentTile) GUI.color = Color.red;
						GUI.DrawTexture(new Rect(j, i, sliceX, sliceY), grid);
						GUI.color = Color.white;
					}
			}
			
			GUI.EndScrollView();
			GUI.EndGroup();

		}
		
	}
	
	void OnFocus()
    {
       SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
       SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }
	void OnDestroy()
    {
       SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }


	
	void OnSceneGUI(SceneView sceneView){
		
		if(showGrid){
			
			Tools.current = Tool.None;
			
			Vector3 mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
			
				
			if(Event.current.type == EventType.mouseDown){
				if(Event.current.button == 1){
					RemoveTileFromScene(new Vector2(mousePosition.x, mousePosition.y));
				}
				else if(Event.current.button == 0) PlaceTileInScene(currentTile, new Vector2(mousePosition.x, mousePosition.y));
			}
			
			
			
			Vector3 tP = new Vector3(Mathf.FloorToInt(mousePosition.x), Mathf.FloorToInt(mousePosition.y), mousePosition.z);
			if(tP != oldTP){
				HandleUtility.Repaint();
			}
			
			Handles.color = Color.blue;
			Handles.DrawLine(tP, tP+Vector3.right);
			Handles.DrawLine(tP+new Vector3(1,1,0), tP+Vector3.right);
			Handles.DrawLine(tP+new Vector3(1,1,0), tP+Vector3.up);
			Handles.DrawLine(tP, tP+Vector3.up);
			
			
			oldTP = tP;
		}
	}
	
	
	void PlaceTileInScene(Vector2 tilePos, Vector2 pos){
		
		pos = new Vector2(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
		
		RemoveTileFromScene(pos);
		
		GameObject go = new GameObject("Tile_"+pos+"l"+depht);
		go.transform.position = new Vector3(pos.x, pos.y, depht);
		
		tilePos = new Vector2(tilePos.x, (spriteAtlas.height - tilePos.y) - sliceY);
		go.AddComponent<SpriteRenderer>().sprite = TileMap[(int)(tilePos.y / sliceY + tilePos.x / sliceX * (spriteAtlas.height / sliceY))];
		
		GameObject l = GameObject.Find("Tiles");
		if(l != null) go.transform.parent = l.transform;
		else{
			l = new GameObject("Tiles");
			go.transform.parent = l.transform;
		}

		
	}
	void RemoveTileFromScene(Vector2 pos){
		pos = new Vector2(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
		if(tileExists(pos)) DestroyImmediate(GetTile(pos));
		
	}
	bool tileExists(Vector2 pos){
		pos = new Vector2(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
		GameObject go = GameObject.Find("Tile_"+pos+"l"+depht);
		if(go != null) return true;
		else return false;
	}
	GameObject GetTile(Vector2 pos){
		pos = new Vector2(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
		return GameObject.Find("Tile_"+pos+"l"+depht);
	}
	
}
