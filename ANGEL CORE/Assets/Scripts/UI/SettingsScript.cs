using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsScript : MonoBehaviour
{
    public GameObject sensSlider;

    private void Start()
    {
        sensSlider.GetComponent<SliderScript>().slider.value = PlayerPrefs.GetFloat("sens");
    }

    void Update()
    {
        PlayerPrefs.SetFloat("sens", sensSlider.GetComponent<SliderScript>().slider.value);
    }
}
