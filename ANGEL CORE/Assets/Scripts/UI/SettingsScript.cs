using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsScript : MonoBehaviour
{
    public GameObject sensSlider;

    void Update()
    {
        PlayerPrefs.SetFloat("sens", sensSlider.GetComponent<SliderScript>().slider.value);
    }
}
