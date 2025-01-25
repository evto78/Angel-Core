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
    float DeathTimer = 10f;

    public int dmg;

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
        Destroy(gameObject, DeathTimer);
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
        string tag = other.gameObject.tag;
        if(tag == "boss ring")
        {

        }
        else if (tag == "Player")
        {
            other.gameObject.GetComponent<HealthManager>().DealDamage(dmg);
            Death();
        }
        else {Death();}
    }
}
