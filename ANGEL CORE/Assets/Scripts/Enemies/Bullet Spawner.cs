using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    float bulletTimer = 3f;
    public GameObject BulletPrefab;


    // Update is called once per frame
    void Update()
    {
        bulletTimer -= Time.deltaTime;
        if (bulletTimer <= 0) 
        {
            GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
            bulletTimer = 3;

        }
    }
}
