using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{

    public Slider slider;
    public TextMeshProUGUI valueText;

    void Update()
    {
        //update the text
        valueText.text = slider.value.ToString();
    }
}
