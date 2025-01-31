using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
    GameObject Player;
    public GameObject PopeBoss;
    BossMovement BossScript;
    public int phase;
    float hp;
    public bool _swingLock;
    bool slashing;
    bool hasTarget;
    float AtkTimer = 2f;
    public bool stuck;
    public bool thrust;
    float StuckTimer;
    public bool normal = true;

    public Vector3 lastPos;
    bool active = false;
    bool started;



    void Start()
    {
        started = false;
        slashing = false;
        phase = 1;
        healthman = GetComponent<HealthManager>();
        hp = healthman.curHealth;
        pointHolder = GameObject.Find("PowerPoints");
        for(int i = 0; i < pointHolder.transform.childCount; i++)
        {
            points.Add(pointHolder.transform.GetChild(i).position);
        }


        Player = GameObject.Find("Player");

        target = GameObject.Find("Phase 1 Target").transform;




    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Player.transform.position, transform.position);
        if(distance <= 100f){started = true;}
        if(normal && !_swingLock && !stuck && started)
        {
            defaultMvmt();
            GetTarget();
            Point();
        }

        if(_swingLock)
        {
            lastPos = (Player.transform.position - transform.position);
            normal = false;
            SwingLock();
            Point();
            AtkTimer -= Time.deltaTime;
            if(AtkTimer <= 0) {thrust = true; _swingLock = false; AtkTimer = 1f;}
        }
        if(thrust)
        {
            AtkTimer -= Time.deltaTime;
            Poke(lastPos);
            if(AtkTimer <= 0)
            {
                stuck = true; thrust = false; AtkTimer = 2f; StuckTimer = 4f;
            }
        }
        if(stuck)
        {

            StuckTimer -= Time.deltaTime;
            if(StuckTimer <= 0)
            {
                AtkTimer -= Time.deltaTime;
                transform.position += Vector3.back * 10 * Time.deltaTime;
                if(AtkTimer <= 0)
                {
                    normal = true;
                    stuck = false;
                }
            }
        }



    }

    void defaultMvmt()
    {
        if (hasTarget)
        {
            transform.position = transform.position + (Vector3.Normalize(targetLocation - transform.position) * 50 * Time.deltaTime);

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
    void Point()
    {
        Vector3 dir = (Player.transform.position - transform.position);
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation,  85 * Time.deltaTime);
        transform.position += transform.forward * 3 * Time.deltaTime;
    }

    void Poke(Vector3 dir)
    {
        transform.position += (Vector3.Normalize(lastPos) * 100 * Time.deltaTime);
        AtkTimer -= Time.deltaTime;
    }

    void spawnBabies()
    {

    }
}
