using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
    public Text healthText;
    public GameObject player;
	public Text gameOverText;
	public Text winText;
	[HideInInspector]
	public bool hasWon = false;

    PlayerController playerCon;

    // Use this for initialization
    void Start () {
	    playerCon = player.GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (healthText != null)
            UpDateHealth();

		if (gameOverText != null)
		{
			gameOverText.enabled = true;
		}

		if (winText != null && hasWon == true)
			winText.enabled = true;
	}

    void UpDateHealth ()
    {
        if (player != null)
        {
            healthText.text = "Health: " + playerCon.playerState.health;
        }
    }
}
