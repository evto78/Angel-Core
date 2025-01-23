using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : MonoBehaviour
{
    public GameObject bomb;
    BombSpawner bombSpawner;
    float timer = 1f;
    // Start is called before the first frame update
    void Start()
    {
        bombSpawner = bomb.GetComponent<BombSpawner>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(bombSpawner.lightSwitch && timer > 0)
        {
            timer -= Time.deltaTime;
            transform.Rotate(-150 * Time.deltaTime, 0, 0);
        }
    }
    
}
