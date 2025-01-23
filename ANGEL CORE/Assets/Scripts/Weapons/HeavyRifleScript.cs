using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyRifleScript : MonoBehaviour
{
    List<Vector3> linePoints = new List<Vector3>();
    float lineTimer;
    LineRenderer lr;
    Animator animator;
    public Transform firePoint;

    public GameObject hitEffect;

    public GameObject spinnyBit;
    public GameObject chargeBlock;
    public GameObject bolt;

    public float minChargeZPOS;
    public float maxChargeZPOS;

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

    public float crossChargeMax;
    float crossCharge;
    public int crossDmg;
    bool crossReady;
    float spinSpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        curBul = magSize;
        crossReady = true;
        reloading = false;
    }
    void Update()
    {
        transform.GetComponentInParent<PlayerUI>().radialCharge.fillAmount = crossCharge / crossChargeMax;

        if (lineTimer > 0f) { lineTimer -= Time.deltaTime * atkSpeed; if (lineTimer < 0f) { lineTimer = 0f; } }
        lr.startWidth = lineTimer;
        lr.endWidth = lineTimer;

        chargeBlock.transform.localPosition = new Vector3(0, 0.02f, Mathf.Lerp(minChargeZPOS, maxChargeZPOS, crossCharge / crossChargeMax));
        spinnyBit.transform.Rotate(Vector3.right * spinSpeed * 400 * Time.deltaTime);

        //Manage UI
        transform.GetComponentInParent<PlayerUI>().curBullets = curBul;
        transform.GetComponentInParent<PlayerUI>().maxBullets = magSize;

        if (crossReady)
        {
            chargeBlock.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            chargeBlock.transform.GetChild(0).gameObject.SetActive(false);
        }

        //Manage Timers
        relTimer -= Time.deltaTime;
        if (relTimer < 0 && reloading == true) { crossReady = true; reloading = false; curBul = magSize; animator.SetBool("Reloading", false); }
        atkSpeedTimer -= Time.deltaTime;
        crossCharge -= Time.deltaTime;
        if(crossCharge < 0) { crossCharge = 0; }
        if(crossCharge > crossChargeMax) { crossCharge = crossChargeMax; }
        spinSpeed -= Time.deltaTime;
        if(spinSpeed < -1) { spinSpeed = -1; }
        if(spinSpeed > 2) { spinSpeed = 2; }
    }
    public void AttemptShoot()
    {

        crossCharge += Time.deltaTime / 2f;
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
    void Shoot()
    {
        spinSpeed += 2;
        crossCharge += 2;

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
        if (crossReady && !reloading)
        {
            crossReady = false;

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
            spawnedBolt.GetComponent<Rigidbody>().AddForce(spawnedBolt.transform.forward * crossCharge * 2500);
            spawnedBolt.GetComponent<BoltScript>().dmg = Mathf.RoundToInt(crossDmg * crossCharge);

            crossCharge = 0;
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
