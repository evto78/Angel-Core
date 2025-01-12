using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool startUp = true;
    float startUpTimer = 2f;
    int spd = 20;
    Transform target;
    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(startUp && startUpTimer >= 0) {transform.position += Vector3.forward * spd * Time.deltaTime; startUpTimer -= Time.deltaTime;}
        else
        {
            followPlayer();
        }
    }

    void followPlayer()
    {
        Vector3 dir = (target.position - transform.position);
        Quaternion rotation = Quaternion.LookRotation(dir/5);
        transform.rotation = rotation;
        transform.position += transform.forward * spd * Time.deltaTime;       
    }
}
