using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Item : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }
    

}
