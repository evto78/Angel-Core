using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntAi : MonoBehaviour

{
    float spd = 15;
    Rigidbody rb;
    Transform target;
    Vector3 moveDirection;
    HealthManager hp;
    public GameObject bulletPrefab;
    float distance;
    GameObject hitbox;
    float atkTimer = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hp = GetComponent<HealthManager>();
        hp.maxHealth = 1;
    }

    void Start()
    {

        hitbox = GameObject.Find("Hitbox");
        hitbox.SetActive(false);
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(target.position, transform.position);

        if(distance > 5f) {MoveTo();}
        if(distance <= 5f) {Attack();}
        LookTo();
        atkTimer -= Time.deltaTime;

    }

    void MoveTo()
    {
        transform.position += transform.forward * spd * Time.deltaTime;       
    }

    void LookTo()
    {
        Vector3 dir = (target.position - transform.position);
        Quaternion rotation = Quaternion.LookRotation(dir);
        if (distance > 20f) 
        { 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 900 * Time.deltaTime); 
        }

        else 
        { 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 250 * Time.deltaTime); 
        }
        
    }
    void Attack()
    {
        if (atkTimer <= 0)
        {
            hitbox.SetActive(true);

            atkTimer = 5f;
        } 

        
    }


}
