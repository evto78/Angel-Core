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
        //moves forward for 5 seconds
        if(startUp && startUpTimer >= 0) 
        {

            transform.position += Vector3.up * spd * Time.deltaTime; startUpTimer -= Time.deltaTime;
        }
        else
        {
            followPlayer();
        }
        // dies after 20 seconds
        DeathTimer -= Time.deltaTime;
        if (DeathTimer <= 0) {Death();}
    }

    //chases the player
    void followPlayer()
    {
        Vector3 dir = (target.position - transform.position);
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation,  85 * Time.deltaTime);
        transform.position += transform.forward * spd * Time.deltaTime;       
    }
    //kils itself
    void Death()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other) 
    {
        bool tag = other.gameObject.CompareTag("boss ring");
        if(tag == true)
        {

        }
        else {Death();}
    }
}
