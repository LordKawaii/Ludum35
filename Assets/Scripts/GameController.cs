using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
    public Text healthText;
    public GameObject player;

    PlayerController playerCon;

    // Use this for initialization
    void Start () {
	    playerCon = player.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (healthText != null)
            UpDateHealth();
	}

    void UpDateHealth ()
    {
        if (player != null)
        {
            healthText.text = "Health: " + playerCon.playerState.health;
        }
    }
}
