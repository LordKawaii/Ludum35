using UnityEngine;
using System.Collections;

public class WinFlagController : MonoBehaviour {
    public Sprite winSprite;
    public AudioClip winSound;

    GameController gameCon;
    AudioSource gameConAuSource;
	// Use this for initialization
	void Start () {
        gameCon = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gameConAuSource = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.tag == "Player" && !gameCon.hasWon)
        { 
            gameCon.hasWon = true;
            if (winSprite != null)
                GetComponent<SpriteRenderer>().sprite = winSprite;
            gameConAuSource.clip = winSound;
            gameConAuSource.Play();
            GetComponentInChildren<ParticleSystem>().Play();
            Time.timeScale = .5f;
        }
    }
}
