using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

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
    float atkTimer = 1f;
    float atkCooldown = 3f;
    bool attacking = false;

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
        hitbox.SetActive(attacking);
        if(distance > 3f) {MoveTo();}
        if(distance <= 4f) {Attack();}
        LookTo();

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
        atkCooldown -= Time.deltaTime;

        if(atkCooldown <= 0) {attacking = true;}
        if(attacking) {atkTimer -= Time.deltaTime;}
        if(atkTimer <= 0) {attacking = false; atkCooldown = 1f; atkTimer = 0.5f;}
    }


}
