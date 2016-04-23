using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class TransformationAnimation : MonoBehaviour {
    public AudioClip alteredHowl;
    public List<Sprite> redTransfromSprites;
    public List<Sprite> grayTransfromSprites;
    public float frameTime = .5f;
    public float length = 2f;

    PlayerController playerCon;
    Image animationImage;
    AudioSource playerAudio;
    GameController gameCon;
    AudioSource cameraAudio;
	// Use this for initialization
	void Start () {
        playerCon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        animationImage = GetComponent<Image>();
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        gameCon = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        cameraAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (playerCon.playerState.isTransformingRed)
        {
            StartCoroutine(AnimateMorph(redTransfromSprites, frameTime, length));
            playerCon.playerState.isTransformingRed = false;
            animationImage.enabled = true;
        }

        if (playerCon.playerState.isTransformingGray)
        {
            StartCoroutine(AnimateMorph(grayTransfromSprites, frameTime, length));
            playerCon.playerState.isTransformingGray = false;
            animationImage.enabled = true;
        }
    }

    IEnumerator AnimateMorph (List<Sprite> spriteList, float animationSpeed, float animationLength)
    {
        cameraAudio.Pause();
        if (playerAudio.isPlaying)
            playerAudio.Stop();
        playerAudio.clip = alteredHowl;
        playerAudio.Play();
        int i = 0;
        int j = 0;
        bool isPlayingForward = true;
        while (playerAudio.isPlaying)
        {
            animationImage.sprite = spriteList[j];
            if (isPlayingForward)
                j++;
            else
                j--;

            if (j > spriteList.Count - 2)
                isPlayingForward = false;
            else if (j == 0)
                isPlayingForward = true;

            print(j);
            i++;
            yield return new WaitForSeconds(animationSpeed);

        }
        cameraAudio.Play();
        animationImage.enabled = false;
        yield return null;
    }
}
