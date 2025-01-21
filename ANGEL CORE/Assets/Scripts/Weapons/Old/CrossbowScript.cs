using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowScript : MonoBehaviour
{
    Animator animator;
    public Transform firePoint;

    public GameObject spinnyBit;
    public GameObject chargeBlock;

    public GameObject bolt;

    //Gun Stats
    public float atkSpeed;
    public int magSize;
    public int dmg;
    int modifiedDmg;

    float charge;
    public float chargeEffectiveness;
    public float maxCharge;

    public float minChargeZPOS;
    public float maxChargeZPOS;

    int curBul;
    bool reloading;
    float atkSpeedTimer;

    void Start()
    {
        charge = maxCharge;
        animator = GetComponent<Animator>();
        curBul = magSize;
        reloading = false;
    }
    void Update()
    {
        //transform.GetComponentInParent<PlayerUI>().radialCharge.fillAmount = charge / maxCharge;

        modifiedDmg = Mathf.RoundToInt(dmg * charge);
        chargeBlock.transform.localPosition = new Vector3(0, 0.02f, Mathf.Lerp(minChargeZPOS, maxChargeZPOS, charge/maxCharge));
        if(charge > maxCharge) { charge = maxCharge; }
        if(charge < 0) { charge = 0; }

        //Manage UI
        //transform.GetComponentInParent<PlayerUI>().curBullets = curBul;
        //transform.GetComponentInParent<PlayerUI>().maxBullets = magSize;

        //Manage Timers
        atkSpeedTimer -= Time.deltaTime;
    }
    public void AttemptShoot()
    {
        if (!reloading && atkSpeedTimer < 0 && curBul > 0)
        {
            curBul--;
            Shoot();
        }
        else if (!reloading && atkSpeedTimer < 0 && curBul == 0)
        {
            AttemptReload();
        }
    }
    void Shoot()
    {
        atkSpeedTimer = 1 / atkSpeed;

        animator.speed = 1 * atkSpeed;
        animator.SetTrigger("Shoot");

        GameObject spawnedBolt = Instantiate(bolt);
        spawnedBolt.transform.position = firePoint.transform.position;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            spawnedBolt.transform.LookAt(hit.point);
        }
        else
        {
            spawnedBolt.transform.rotation = transform.rotation;
        }
        spawnedBolt.GetComponent<Rigidbody>().AddForce(spawnedBolt.transform.forward * charge * 2500);
        spawnedBolt.GetComponent<BoltScript>().dmg = modifiedDmg;

        charge = 0;
    }

    public void AttemptReload()
    {
        if (atkSpeedTimer <= 0f && curBul < magSize)
        {
            reloading = true;
            Reload();
        }
    }
    public void AttemptReloadUp()
    {
        if (reloading)
        {
            reloading = false;
            curBul = magSize;
        }
    }
    void Reload()
    {
        charge += Time.deltaTime;
        spinnyBit.transform.Rotate(Vector3.right * 70 * Time.deltaTime);
    }
}

