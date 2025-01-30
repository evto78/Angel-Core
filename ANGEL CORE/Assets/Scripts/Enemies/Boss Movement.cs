
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class BossMovement : MonoBehaviour
{
    public Image healthBar;
    HealthManager healthman;

    List<Vector3> points = new List<Vector3>();
    List<Vector3> bulletPoints = new List<Vector3>();
    GameObject bulHolder;
    GameObject pointHolder;
    public GameObject GruntPrefab;
    public GameObject WatcherPrefab;
    float babyTimer = 3f;
    Vector3 targetLocation;
    bool hasTarget;
    float timer = 5f;
    public float speed;
    Transform target;
    public bool Bombing = false;
    public int phase = 1;
    float hp;
    bool p2 = true;
    public bool death = false;



    void Start()
    {
        healthman = GetComponent<HealthManager>();
        hp = healthman.curHealth;
        hasTarget = false;

        pointHolder = GameObject.Find("RoamPoints");
        bulHolder = GameObject.Find("Ring holder");
        //fill the points list with all the positions inside "RoamPoints"
        //position 0 should be the bosses default location
        for(int i = 0; i < pointHolder.transform.childCount; i++)
        {
            points.Add(pointHolder.transform.GetChild(i).position);
        }

        for(int i = 1; i < bulHolder.transform.childCount; i++)
        {
            bulletPoints.Add(bulHolder.transform.GetChild(i).position);
        }

        target = GameObject.Find("Player").transform;




    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = healthman.curHealth * 1f / healthman.maxHealth * 1f;
        if(healthman.curHealth <= 500 && p2)
        {
            Debug.Log("phase 2");
            phase = 2;
            p2 = false;
        }
        if(healthman.curHealth <= 150)
        {
            phase = 3;
        }


        
        if(phase == 1)
        {
            MoveToTar();
            GetTarget();
        }
        if(phase == 2)
        {
            MoveToTar();
            GetTarget();
            spawnBabies();

        }
        if(phase == 3)
        {
            PurityBomb();
        }
        if(healthman.curHealth <= 32)
        {
            death = true;
        }



    }

    void MoveToTar()
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
    void spawnBabies()
    {
        babyTimer -= Time.deltaTime;
        if(babyTimer <= 0)
        {    
            if(Random.Range(1,3) == 1)
            {
                GameObject watcher = Instantiate(WatcherPrefab, new Vector3(transform.position.x, (transform.position.y -25), transform.position.z), transform.rotation);
                babyTimer = 5f;
            }
            else
            {
                GameObject grunt = Instantiate(GruntPrefab, new Vector3(transform.position.x, (transform.position.y -30), transform.position.z), transform.rotation);
                babyTimer = 5f;
            }
        }
    }
    void PurityBomb()
    {
        targetLocation = new Vector3(0,transform.position.y,0);
        if(Vector3.Distance(targetLocation, transform.position) > 1f)
        {
            MoveToTar();
            Debug.Log("moving to bomb");
        }
        else { Bombing = true;}
    }

    
}
