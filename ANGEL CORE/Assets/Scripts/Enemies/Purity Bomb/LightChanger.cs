using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : MonoBehaviour
{
    public GameObject bomb;
    public GameObject purBomb;
    BombSpawner bombSpawner;
    BombScript bombScript;
    float timer = 1f;
    float revTimer = 1f;
    float waitTimer = 5f;
    // Start is called before the first frame update
    void Start()
    {
        bombSpawner = bomb.GetComponent<BombSpawner>();
        bombScript = purBomb.GetComponent<BombScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bombSpawner.lightSwitch && timer > 0)
        {
            timer -= Time.deltaTime;
            transform.Rotate(-150 * Time.deltaTime, 0, 0);
        }
        if(timer<= 0 && waitTimer >= 0)
        {
            waitTimer -= Time.deltaTime;
        }
        if(waitTimer <=0 && revTimer >= 0)
        {
            revTimer -= Time.deltaTime;
            transform.Rotate(100 * Time.deltaTime, 0, 0);
        }
    }
    
}
