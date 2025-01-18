using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverScript : MonoBehaviour
{
    List<Vector3> linePoints = new List<Vector3>();
    float lineTimer;
    LineRenderer lr;
    public LineRenderer rr;
    Animator animator;
    public Transform firePoint;
    public GameObject player;

    public GameObject hitEffect;
    public GameObject thrownGun;

    //Gun Stats
    public float atkSpeed;
    public float relSpeed;
    public int magSize;
    public int dmg;
    public float spread;

    public float throwForce;
    public float maxDistance;
    int curBul;
    bool reloading;
    float relTimer;
    float atkSpeedTimer;
    bool throwing;
    bool thrown;
    float thrownInvisTimer;
    GameObject spawnedAxe;
    Vector3 spawnedAxeLastPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        curBul = magSize;
        reloading = false;
    }
    void Update()
    {
        if(thrown && thrownInvisTimer <= 0)
        {
            if (spawnedAxe != null) 
            { 
                spawnedAxeLastPos = spawnedAxe.transform.position; 
                if(Vector3.Distance(player.transform.position, spawnedAxeLastPos) > maxDistance)
                {
                    spawnedAxeLastPos = player.transform.position;
                    Destroy(spawnedAxe);
                }
            }

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            if(spawnedAxe == null && rr.enabled == true)
            {
                thrown = false;
                throwing = false;
                animator.SetBool("throwing", false);

                player.GetComponent<Rigidbody>().AddForce(Vector3.up * 15f * Vector3.Distance(player.transform.position, spawnedAxeLastPos));
                player.GetComponent<Rigidbody>().AddForce(Vector3.Normalize((spawnedAxeLastPos - player.transform.position)) * 50f * Vector3.Distance(player.transform.position, spawnedAxeLastPos));
            }
            else
            {
                if (spawnedAxe == null) { SpawnAxe(); }
                else { rr.enabled = true; RenderRope(); }
            }
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;

            rr.enabled = false;
        }

        transform.GetComponentInParent<PlayerUI>().radialCharge.fillAmount = 0;

        if (lineTimer > 0f) { lineTimer -= Time.deltaTime * atkSpeed; if (lineTimer < 0f) { lineTimer = 0f; } }
        lr.startWidth = lineTimer;
        lr.endWidth = lineTimer;

        //Manage UI
        transform.GetComponentInParent<PlayerUI>().curBullets = curBul;
        transform.GetComponentInParent<PlayerUI>().maxBullets = magSize;

        //Manage Timers
        relTimer -= Time.deltaTime;
        if(relTimer < 0 && reloading == true) { reloading = false; curBul = magSize; animator.SetBool("Reloading", false); }
        atkSpeedTimer -= Time.deltaTime;
        thrownInvisTimer -= Time.deltaTime;
    }
    public void AttemptShoot()
    {
        if(!thrown && !throwing && relTimer < 0 && atkSpeedTimer < 0 && curBul > 0)
        {
            curBul--;
            Shoot();
        }
        else if(relTimer < 0 && atkSpeedTimer < 0 && curBul == 0)
        {
            AttemptReload();
        }
    }
    void Shoot()
    {
        atkSpeedTimer = 1 / atkSpeed;

        animator.speed = 1 * atkSpeed;
        animator.SetTrigger("Shoot");

        lr.positionCount = 1;
        lr.SetPosition(0, firePoint.position);
        linePoints.Clear();
        linePoints.Add(firePoint.position);

        Vector3 direction = GetDir();

        direction.Normalize();

        if (curBul < magSize)
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
        if (!thrown && !throwing)
        {
            throwing = true;
            animator.SetBool("Throwing", true);
        }
        
    }

    public void AttemptAltShootUp()
    {
        if (!thrown && throwing)
        {
            throwing = false;
            thrown = true;
            thrownInvisTimer = 0.1f;
            animator.SetBool("Throwing", false);
            animator.SetTrigger("Throw");
        }
        
    }

    public void SpawnAxe()
    {
        spawnedAxe = Instantiate(thrownGun);
        spawnedAxe.transform.position = transform.position;
        spawnedAxe.transform.rotation = transform.rotation;
        Vector3 throwDir = GetDir();
        throwDir.Normalize();
        spawnedAxe.GetComponent<Rigidbody>().velocity = Vector3.zero;
        spawnedAxe.GetComponent<Rigidbody>().AddForce(throwDir * throwForce);
        spawnedAxe.GetComponent<Rigidbody>().AddTorque(spawnedAxe.transform.right * 90f);
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
        if (!thrown && !throwing && atkSpeedTimer <= 0f && relTimer <= 0f && curBul < magSize)
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

    void RenderRope()
    {
        rr.SetPosition(0, firePoint.position);
        rr.SetPosition(1, spawnedAxe.transform.position);
    }
}
