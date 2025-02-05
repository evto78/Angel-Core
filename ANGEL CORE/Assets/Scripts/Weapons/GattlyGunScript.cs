using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GattlyGunScript : MonoBehaviour
{
    List<Vector3> linePoints = new List<Vector3>();
    float lineTimer;
    LineRenderer lr;
    Animator animator;
    public Transform firePoint;
    public GameObject spinnyTubes;

    public GameObject hitEffect;
    public GameObject hitBox;

    //Gun Stats
    public float atkSpeed;
    public float relSpeed;
    public int magSize;
    public int dmg;
    public float spread;
    float modifiedSpread;

    int curBul;
    bool reloading;
    float relTimer;
    float atkSpeedTimer;

    float charge;
    public float maxCharge;
    public float chargeEffectiveness;
    float modifiedAtkSpd;

    bool revving;
    public int meleeDmg;
    public float meleeKnock;
    public float hitboxSpawnRate;
    float hitboxSpawnTimer;
    void Start()
    {
        animator = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        curBul = magSize;
        reloading = false;

        charge = 1;
        modifiedAtkSpd = atkSpeed;
    }
    void Update()
    {
        
        transform.GetComponentInParent<PlayerUI>().radialCharge.fillAmount = (charge - 1f) / (maxCharge - 1f);

        if (lineTimer > 0f) { lineTimer -= Time.deltaTime * atkSpeed; if (lineTimer < 0f) { lineTimer = 0f; } }
        lr.startWidth = lineTimer;
        lr.endWidth = lineTimer;

        //Manage UI
        transform.GetComponentInParent<PlayerUI>().curBullets = curBul;
        transform.GetComponentInParent<PlayerUI>().maxBullets = magSize;

        //Manage Timers
        relTimer -= Time.deltaTime;
        if (relTimer < 0 && reloading == true) { reloading = false; curBul = magSize; animator.SetBool("Reloading", false); }
        atkSpeedTimer -= Time.deltaTime;

        charge -= Time.deltaTime;
        if (reloading) { charge -= Time.deltaTime; }
        if(charge < 1) { charge = 1; }
        if(charge > maxCharge) { charge = maxCharge; }
        modifiedSpread = spread * (charge * chargeEffectiveness);
        modifiedAtkSpd = atkSpeed * (charge * chargeEffectiveness);
        spinnyTubes.transform.Rotate(Vector3.forward * charge * 200f * Time.deltaTime);
        hitboxSpawnTimer -= Time.deltaTime * charge;
    }
    public void AttemptShoot()
    {
        if (!revving)
        {
            //charge up gattlygun
            if (charge < maxCharge && !reloading)
            {
                charge += Time.deltaTime * 2;
                modifiedAtkSpd = atkSpeed * (charge * chargeEffectiveness);
            }

            if (relTimer < 0 && atkSpeedTimer < 0 && curBul > 0)
            {
                curBul--;
                Shoot();
            }
            else if (relTimer < 0 && atkSpeedTimer < 0 && curBul == 0)
            {
                AttemptReload();
            }
        }
    }
    void Shoot()
    {
        atkSpeedTimer = 1 / modifiedAtkSpd;

        animator.speed = 1 * modifiedAtkSpd;
        animator.SetTrigger("Shoot");

        lr.positionCount = 1;
        lr.SetPosition(0, firePoint.position);
        linePoints.Clear();
        linePoints.Add(firePoint.position);

        Vector3 direction = GetDir();

        direction.Normalize();

        if (curBul < magSize)
        {
            direction += new Vector3(Random.Range(-modifiedSpread, modifiedSpread), Random.Range(-modifiedSpread, modifiedSpread), Random.Range(-modifiedSpread, modifiedSpread));
        }

        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, float.MaxValue))
        {
            linePoints.Add(hit.point);

            if (hit.transform.gameObject.tag == "boss core" || hit.transform.gameObject.tag == "grunt core")
            {
                GameObject spawnedEffect = Instantiate(hitEffect);
                spawnedEffect.transform.position = hit.point;
                Destroy(spawnedEffect, 3f);

                hit.transform.gameObject.GetComponent<HealthManager>().DealDamage(dmg);
            }
            if (hit.transform.gameObject.tag == "boss ring")
            {
                linePoints.Add(new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), Random.Range(-4, 4)) * 1000f);
            }

            RenderLine();
        }
        else
        {
            linePoints.Add(firePoint.position + direction * 9999);
            RenderLine();
        }
    }
    public void AttemptAltShoot()
    {
        revving = true;
        charge += Time.deltaTime * 4f;
        animator.SetBool("Revving", true);
        animator.speed = (charge + 1f) / (maxCharge);

        if(hitboxSpawnTimer < 0)
        {
            GameObject spawnedHitbox = Instantiate(hitBox);
            spawnedHitbox.transform.position = transform.position;
            spawnedHitbox.transform.rotation = Camera.main.transform.rotation;
            //spawnedHitbox.transform.Translate(spawnedHitbox.transform.forward*2);

            spawnedHitbox.GetComponent<HitBoxGattly>().dmg = meleeDmg;
            spawnedHitbox.GetComponent<HitBoxGattly>().knockback = meleeKnock;
            hitboxSpawnTimer = hitboxSpawnRate;
        }
    }
    public void AttemptAltShootUp()
    {
        revving = false;
        animator.SetBool("Revving", false);
    }
    Vector3 GetDir()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point - transform.position;
        }

        return Camera.main.transform.forward;
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
    void RenderLine()
    {
        lineTimer = 0.3f;

        lr.positionCount = linePoints.Count;

        for (int i = 0; i < linePoints.Count; i++)
        {
            Debug.DrawLine(linePoints[i], linePoints[i] + Vector3.up * 3f, Color.yellow, 10f);
            lr.SetPosition(i, linePoints[i]);
        }
    }
}

