using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TommySound : MonoBehaviour
{
    public AudioSource aS;

    public AudioClip shot1;
    public AudioClip shot2;
    public AudioClip reload;
    public AudioClip throwMag;
    public AudioClip explode;

    float volume;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetFloat("Effect Volume") != 0f)
        {
            volume = PlayerPrefs.GetFloat("Effect Volume") / 100f;
        }
        else
        {
            PlayerPrefs.SetFloat("Effect Volume", 33f);
            volume = PlayerPrefs.GetFloat("Effect Volume") / 100f;
        }

        aS.volume = volume * (PlayerPrefs.GetFloat("Master Volume") / 100f);
    }

    // Update is called once per frame
    void Update()
    {
        volume = PlayerPrefs.GetFloat("Effect Volume") / 100f;
        aS.volume = volume * (PlayerPrefs.GetFloat("Master Volume") / 100f);
    }

    public void Shoot()
    {
        aS.Stop();
        if (Random.Range(0, 1) == 0)
        {
            aS.clip = shot1;
        }
        else
        {
            aS.clip = shot2;
        }
        aS.Play();
    }
    public void Reload()
    {
        aS.Stop();
        aS.clip = reload;
        aS.Play();
    }
    public void Throw()
    {
        aS.Stop();
        aS.clip = throwMag;
        aS.Play();
    }
    public void Explode()
    {
        aS.Stop();
        aS.clip = explode;
        aS.Play();
    }
}
