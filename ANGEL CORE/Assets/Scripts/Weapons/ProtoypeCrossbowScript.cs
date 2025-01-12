using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoypeCrossbowScript : MonoBehaviour
{
    Animator animator;
    public Transform firePoint;
    public List<GameObject> spinnyBits;
    public GameObject disc;

    //Gun Stats
    public float atkSpeed;
    public float relSpeed;
    public int magSize;
    public int dmg;
    int modifiedDmg;

    float charge;
    public float chargeEffectiveness;
    public float maxCharge;

    int curBul;
    bool reloading;
    float atkSpeedTimer;
    float relTimer;

    void Start()
    {
        charge = 0;
        animator = GetComponent<Animator>();
        curBul = magSize;
        reloading = false;
    }
    void Update()
    {
        transform.GetComponentInParent<PlayerUI>().radialCharge.fillAmount = charge / maxCharge;

        modifiedDmg = Mathf.RoundToInt(dmg * charge * chargeEffectiveness);

        if (charge > maxCharge) { charge = maxCharge; }
        if (charge < 0) { charge = 0; }

        for (int i = 0; i < spinnyBits.Count; i++)
        {
            spinnyBits[i].transform.Rotate(Vector3.up * charge * -10f);
        }

        //Manage UI
        transform.GetComponentInParent<PlayerUI>().curBullets = curBul;
        transform.GetComponentInParent<PlayerUI>().maxBullets = magSize;

        //Manage Timers
        atkSpeedTimer -= Time.deltaTime;
        relTimer -= Time.deltaTime;
        if (relTimer < 0 && reloading == true) { reloading = false; curBul = magSize; animator.SetBool("Reloading", false); }
    }
    public void AttemptShoot()
    {
        if (relTimer < 0 && atkSpeedTimer < 0 && curBul > 0)
        {
            charge += Time.deltaTime;
        }
        else if (relTimer < 0 && atkSpeedTimer < 0 && curBul == 0)
        {
            AttemptReload();
        }
    }
    public void AttemptShootUp()
    {
        if (relTimer < 0 && atkSpeedTimer < 0 && curBul > 0)
        {
            Shoot();
        } 
    }
    void Shoot()
    {
        curBul--;
        atkSpeedTimer = 1 / atkSpeed;

        animator.speed = 1 * atkSpeed;
        animator.SetTrigger("Shoot");

        GameObject spawnedDisc = Instantiate(disc);
        if(spawnedDisc == null) { Debug.LogError("disc could not spawn"); }
        if(spawnedDisc == null) { Debug.Log("disc spawned"); }
        spawnedDisc.transform.position = firePoint.transform.position;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            spawnedDisc.transform.LookAt(hit.point);
        }
        else
        {
            spawnedDisc.transform.rotation = transform.rotation;
        }
        spawnedDisc.GetComponent<Rigidbody>().AddForce(spawnedDisc.transform.forward * charge * 2500);
        spawnedDisc.GetComponent<BoltScript>().dmg = modifiedDmg;

        charge = 0;
    }

    public void AttemptReload()
    {
        if (atkSpeedTimer <= 0f && relTimer <= 0f && curBul < magSize)
        {
            reloading = true;
            Reload();
        }
    }
    void Reload()
    {
        relTimer = 1 / relSpeed;

        animator.speed = 1 * relSpeed;
        animator.SetBool("Reloading", true);
    }
}


