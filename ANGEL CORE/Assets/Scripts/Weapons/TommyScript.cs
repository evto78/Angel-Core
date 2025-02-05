using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TommyScript : MonoBehaviour
{
    List<Vector3> linePoints = new List<Vector3>();
    float lineTimer;
    LineRenderer lr;
    Animator animator;
    public Transform firePoint;

    public GameObject hitEffect;

    //Gun Stats
    public float atkSpeed;
    public float relSpeed;
    public int magSize;
    public int dmg;
    public float spread;

    int curBul;
    bool reloading;
    float relTimer;
    float atkSpeedTimer;
    public int explosionDmg;
    public float explosionForce;
    public float throwForce;
    bool bulletChambered;
    public GameObject thrownMine;
    GameObject spawnedMine;

    void Start()
    {
        animator = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        curBul = magSize;
        reloading = false;
    }
    void Update()
    {
        transform.GetComponentInParent<PlayerUI>().radialCharge.fillAmount = 0;

        if (lineTimer > 0f) { lineTimer -= Time.deltaTime * atkSpeed; if (lineTimer < 0f) { lineTimer = 0f; } }
        lr.startWidth = lineTimer;
        lr.endWidth = lineTimer;

        //Manage UI
        transform.GetComponentInParent<PlayerUI>().curBullets = curBul;
        transform.GetComponentInParent<PlayerUI>().maxBullets = magSize;

        //Manage Timers
        relTimer -= Time.deltaTime;
        if (relTimer < 0 && reloading == true) { bulletChambered = false; reloading = false; curBul = magSize; animator.SetBool("Reloading", false); }
        atkSpeedTimer -= Time.deltaTime;
    }
    public void AttemptShoot()
    {
        if (relTimer < 0 && atkSpeedTimer < 0 && curBul > 0)
        {
            curBul--;
            Shoot(false);
        }
        else if(relTimer > 0 && atkSpeedTimer < 0 && bulletChambered)
        {
            Shoot(true);
            bulletChambered = false;
        }
        else if (relTimer < 0 && atkSpeedTimer < 0 && curBul == 0)
        {
            AttemptReload();
        }
    }
    void Shoot(bool wasChambered)
    {
        if (!wasChambered)
        {
            atkSpeedTimer = 1 / atkSpeed;

            animator.speed = 1 * atkSpeed;
        }
        animator.SetTrigger("Shoot");

        lr.positionCount = 1;
        lr.SetPosition(0, firePoint.position);
        linePoints.Clear();
        linePoints.Add(firePoint.position);

        Vector3 direction = GetDir();

        direction.Normalize();

        if(curBul < magSize && !wasChambered)
        {
            direction += new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
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
            if (hit.transform.gameObject.tag == "player projectile")
            {
                hit.transform.gameObject.GetComponent<ThrownMineScript>().Explode();
            }

            RenderLine();
        }
        else
        {
            linePoints.Add(firePoint.position + direction * 9999);
            RenderLine();
        }
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
        if(curBul > 0)
        {
            ThrowMine((curBul / 2f) / (magSize / 2f));
            bulletChambered = true;
        }

        relTimer = 1 / relSpeed;

        animator.speed = 1 * relSpeed;
        animator.SetBool("Reloading", true);
    }
    void ThrowMine(float bulLeft)
    {
        spawnedMine = Instantiate(thrownMine);
        spawnedMine.transform.position = Camera.main.transform.position;
        spawnedMine.GetComponent<Rigidbody>().AddForce((Camera.main.transform.forward * throwForce) + (Vector3.up * (throwForce/6f)));
        spawnedMine.GetComponent<ThrownMineScript>().dmg = Mathf.RoundToInt(bulLeft) * explosionDmg;
        spawnedMine.GetComponent<ThrownMineScript>().force = bulLeft * explosionForce;
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
