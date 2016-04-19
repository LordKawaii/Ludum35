using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class StarButtonCon : MonoBehaviour {

    public GameObject startButton;

    GameController gameCon;
    AudioSource auSource;
    public AudioClip start;
    void Start ()
    {
        gameCon = GetComponent<GameController>();
        auSource = GetComponent<AudioSource>();
    }

    void Update ()
    {
        if (Input.GetButtonDown("Fire1") && !gameCon.gameHasStarted)
        {
            OnClick(); 
        }
    }

    public void OnClick()
    {
        Time.timeScale = 1f;
        auSource.clip = start;
        auSource.Play();
        gameCon.gameHasStarted = true;
        
        if (startButton != null)
            Destroy(startButton);
    }

}
