using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class StarButtonCon : MonoBehaviour {

    public GameObject startButton;

    AudioSource auSource;
    public AudioClip start;
    void Start ()
    {
        auSource = GetComponent<AudioSource>();
    }

    public void OnClick()
    {
        Time.timeScale = 1f;
        auSource.clip = start;
        auSource.Play();
        
        if (startButton != null)
            Destroy(startButton);
    }

}
