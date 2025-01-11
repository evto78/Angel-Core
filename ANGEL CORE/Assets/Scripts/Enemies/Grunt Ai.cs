using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntAi : MonoBehaviour

{
    float spd = 1;
    Rigidbody rb;
    Transform target;
    Vector3 moveDirection;
    HealthManager hp;
    public GameObject bulletPrefab;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hp = GetComponent<HealthManager>();
        hp.maxHealth = 1;
    }

    void Start()
    {
        
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTo();
        if (Input.GetMouseButtonDown(1)) {hp.Death();}

    }

    void MoveTo()
    {
        Vector3 dir = (target.position - transform.position);
        Quaternion rotation = Quaternion.LookRotation(dir/2);
        transform.rotation = rotation;
        transform.position += transform.forward * spd * Time.deltaTime;       
    }
    

}
