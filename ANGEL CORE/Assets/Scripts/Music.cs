using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource aS;

    public AudioClip intro;
    public AudioClip firstRun;
    public AudioClip middleLoop;

    float volume;

    bool introPlayed = false;
    bool firstRunPlayed = false;
    bool loopPlaying = false;

    void Start()
    {
        if (PlayerPrefs.GetFloat("Music Volume") != 0f)
        {
            volume = PlayerPrefs.GetFloat("Music Volume") / 100f;
        }
        else
        {
            PlayerPrefs.SetFloat("Music Volume", 33f);
            volume = PlayerPrefs.GetFloat("Music Volume") / 100f;
        }

        aS.volume = volume;

        aS.clip = intro;
        aS.loop = false;
        aS.Play();
        introPlayed = true;
    }

    private void FixedUpdate()
    {
        volume = PlayerPrefs.GetFloat("Music Volume") / 100f;
        aS.volume = volume;
    }

    void Update()
    {
        volume = PlayerPrefs.GetFloat("Music Volume") / 100f;
        aS.volume = volume;
        if (introPlayed && !aS.isPlaying && !firstRunPlayed)
        {
            aS.Stop();
            aS.clip = firstRun;
            aS.Play();
            firstRunPlayed = true;
        }
        if (firstRunPlayed && !aS.isPlaying && !loopPlaying)
        {
            aS.Stop();
            aS.clip = middleLoop;
            aS.loop = true;
            aS.Play();
            loopPlaying = true;
        }
    }
}
