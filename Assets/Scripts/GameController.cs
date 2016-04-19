using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
    public Text healthText;
    public GameObject player;
	public Text gameOverText;
	public Text winText;


    [HideInInspector]
	public bool hasWon = false;
    [HideInInspector]
    public bool gameHasStarted = false;

    PlayerController playerCon;

    // Use this for initialization
    void Start () {
	    playerCon = player.GetComponent<PlayerController>();
        Time.timeScale = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (healthText != null)
            UpDateHealth();

        if (gameOverText != null && player == null)
		{
			gameOverText.enabled = true;
		}

		if (winText != null && hasWon == true)
			winText.enabled = true;

        if (Input.GetKey(KeyCode.R) && (hasWon || player == null))
            Application.LoadLevel(Application.loadedLevel);
	}

    void UpDateHealth ()
    {
        if (player != null)
        {
            healthText.text = "Health: " + playerCon.playerState.health;
        }
    }
}
