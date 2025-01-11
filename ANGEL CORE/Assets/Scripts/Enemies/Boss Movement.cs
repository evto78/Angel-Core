using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    List<Vector3> points = new List<Vector3>();
    GameObject pointHolder;
    Vector3 targetLocation;
    bool hasTarget;
    float timer = 5f;
    public float speed;

    void Start()
    {
        hasTarget = false;

        pointHolder = GameObject.Find("RoamPoints");
        //fill the points list with all the positions inside "RoamPoints"
        //position 0 should be the bosses default location
        for(int i = 0; i < pointHolder.transform.childCount; i++)
        {
            points.Add(pointHolder.transform.GetChild(i).position);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        GetTarget();
        MoveToTar();
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
}
