using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsScript : MonoBehaviour
{
    public GameObject sensSlider;
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
    }

    void Update()
    {
        PlayerPrefs.SetFloat("sens", sensSlider.GetComponent<SliderScript>().slider.value);
        if (leftHanded) { PlayerPrefs.SetString("lefthanded", "true"); }
        else { PlayerPrefs.SetString("lefthanded", "false"); }
    }

    public void LeftHanded(bool input)
    {
        leftHanded = input;
    }
}
