using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
    public Text healthText;
    public GameObject player;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (healthText != null)
            UpDateHealth();
	}

    void UpDateHealth ()
    {
        
    }
}
