using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverScript : MonoBehaviour
{
    //Gun Stats
    static public float atkSpeed;
    static public float relSpeed;
    static public int magSize;
    static public int dmg;

    int curBul;
    bool reloading;
    float relTimer;
    float atkSpeedTimer;

    void Start()
    {
        curBul = magSize;
        reloading = false;
    }
    void Update()
    {

        //Manage Timers
        relTimer -= Time.deltaTime;
        if(relTimer < 0) { reloading = false; curBul = magSize; }
        atkSpeedTimer -= Time.deltaTime;
    }
    public void AttemptShoot()
    {
        if(relSpeed < 0 && atkSpeedTimer < 0 && curBul > 0)
        {
            Shoot();
            curBul--;
        }
    }
    void Shoot()
    {

    }
    public void AttemptReload()
    {

    }
    void Reload()
    {

    }
}
