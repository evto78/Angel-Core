using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBossMovement : MonoBehaviour
{
    GameObject pointHolder;
    List<Vector3> points = new List<Vector3>();
    HealthManager healthman;
    Vector3 targetLocation;
    float timer = 5f;
    public float speed;
    Transform target;
    public int phase;
    float hp;
    public bool _swingLock;
    bool slashing;
    bool hasTarget;



    void Start()
    {
        slashing = false;
        phase = 1;
        healthman = GetComponent<HealthManager>();
        hp = healthman.curHealth;
        pointHolder = GameObject.Find("PowerPoints");
        for(int i = 0; i < pointHolder.transform.childCount; i++)
        {
            points.Add(pointHolder.transform.GetChild(i).position);
        }


        target = GameObject.Find("Player").transform;

        target = GameObject.Find("Phase 1 Target").transform;




    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        
        if(!_swingLock)
        {
            defaultMvmt();
            GetTarget();
        }

        if(_swingLock)
        {
            SwingLock();
        }

    }

    void defaultMvmt()
    {
        if (hasTarget)
        {
            transform.position = transform.position + (Vector3.Normalize(targetLocation - transform.position) * speed * Time.deltaTime);
        }
    }
    void GetTarget()
    {
        if (!hasTarget)
        {
            targetLocation = points[Random.Range(0, points.Count)];
            hasTarget = true;
        }
        if (Vector3.Distance(targetLocation, transform.position) < 1f)
        {
            timer -= Time.deltaTime;

            if(timer <= 0) {hasTarget = false; timer = 5f;}
        }       
    }
    void SwingLock()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed);

    }
    void Poke()
    {

    }

    void spawnBabies()
    {

    }
}
