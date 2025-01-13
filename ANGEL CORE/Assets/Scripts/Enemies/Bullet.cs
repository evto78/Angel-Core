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
    float DeathTimer = 20f;



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
        DeathTimer -= Time.deltaTime;
        if (DeathTimer <= 0) {Death();}
    }

    void followPlayer()
    {
        Vector3 dir = (target.position - transform.position);
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation,  85 * Time.deltaTime);
        transform.position += transform.forward * spd * Time.deltaTime;       
    }
    void Death()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other) 
    {
        Death();
    }
}
