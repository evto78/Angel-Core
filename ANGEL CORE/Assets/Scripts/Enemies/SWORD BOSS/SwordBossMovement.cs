using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBossMovement : MonoBehaviour
{
    HealthManager healthman;
    Vector3 targetLocation;
    float timer = 5f;
    public float speed;
    Transform target;
    public int phase;
    float hp;
    public bool _swingLock;



    void Start()
    {
        phase = 1;
        healthman = GetComponent<HealthManager>();
        hp = healthman.curHealth;
        

        target = GameObject.Find("Phase 1 Target").transform;




    }

    // Update is called once per frame
    void Update()
    {
        if(_swingLock)
        {
            SwingLock();
        }

    }

    void defaultMvmt()
    {

    }
    void SwingLock()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed);

    }

    void spawnBabies()
    {

    }
}
