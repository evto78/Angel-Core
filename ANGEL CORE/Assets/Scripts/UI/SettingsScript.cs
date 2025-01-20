using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsScript : MonoBehaviour
{
    public GameObject sensSlider;
    public GameObject musicSlider;
    public bool leftHanded;

    private void Start()
    {
        if (PlayerPrefs.GetString("lefthanded") == "")
        {
            PlayerPrefs.SetString("lefthanded", "false");
            leftHanded = false;
        }
        if (PlayerPrefs.GetFloat("sens") <= 0)
        {
            PlayerPrefs.SetFloat("sens", 50);
        }
        sensSlider.GetComponent<SliderScript>().slider.value = PlayerPrefs.GetFloat("sens");
        if (PlayerPrefs.GetFloat("Music Volume") <= 0)
        {
            PlayerPrefs.SetFloat("Music Volume", 1f);
        }
        musicSlider.GetComponent<SliderScript>().slider.value = PlayerPrefs.GetFloat("Music Volume");
    }

    void Update()
    {
        PlayerPrefs.SetFloat("sens", sensSlider.GetComponent<SliderScript>().slider.value);
        PlayerPrefs.SetFloat("Music Volume", musicSlider.GetComponent<SliderScript>().slider.value);
        if (leftHanded) { PlayerPrefs.SetString("lefthanded", "true"); }
        else { PlayerPrefs.SetString("lefthanded", "false"); }
    }

    public void LeftHanded(bool input)
    {
        leftHanded = input;
    }
}
