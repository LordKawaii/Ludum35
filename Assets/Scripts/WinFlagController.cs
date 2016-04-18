using UnityEngine;
using System.Collections;

public class WinFlagController : MonoBehaviour {
    public Sprite winSprite;

    GameController gameCon;

	// Use this for initialization
	void Start () {
        gameCon = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.tag == "Player")
        { 
            gameCon.hasWon = true;
            if (winSprite != null)
                GetComponent<SpriteRenderer>().sprite = winSprite;
            GetComponentInChildren<ParticleSystem>().Play();
            Time.timeScale = .5f;
        }
    }
}
