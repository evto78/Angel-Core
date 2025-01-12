using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
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
    Vector3 targetLocation;
    bool hasTarget;
    float timer = 5f;
    public float speed;
    Transform target;

    void Start()
    {
        healthman = GetComponent<HealthManager>();

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
        
        GetTarget();
        MoveToTar();
        LookAtPlayer();
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
    void LookAtPlayer()
    {
        Vector3 dir = (target.position - transform.position);
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = rotation;
    }
    
}
