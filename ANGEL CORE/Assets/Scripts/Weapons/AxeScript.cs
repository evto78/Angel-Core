using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    PlayerMovement movementScript;

    Animator animator;
    LineRenderer lr;
    GameObject blade;
    GameObject pully;
    GameObject handle;
    Transform firepoint;

    public GameObject quickSlashHitbox;
    public GameObject heavySlashHitbox;
    public GameObject heavyAttachHitbox;

    public KeyCode quickSlash;
    public int quickDmg;
    public float quickKnockback;
    public float quickSelfKnockback;
    public float quickActiveTime;
    float quickActiveTimer;
    public float quickEndLag;
    float quickEndLagTimer;
    bool quickSlashing;
    public KeyCode heavySlash;
    public int heavyDmg;
    public float heavyActiveTime;
    float heavyActiveTimer;
    public float heavyLeadUp;
    float heavyLeadUpTimer;
    public float heavyEndLag;
    float heavyEndLagTimer;
    bool stuck;
    bool heavySlashing;
    public float maxDetachTime;
    float detachTimer;
    float ropeLegnth;
    bool detached;

    void Start()
    {
        movementScript = GetComponentInParent<PlayerMovement>();
        heavySlashing = false;
        quickSlashing = false;
        stuck = false;
        detached = false;
        lr = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        blade = transform.GetChild(0).gameObject;
        pully = transform.GetChild(1).gameObject;
        handle = transform.GetChild(2).gameObject;
        firepoint = transform.GetChild(3);
    }
    void Update()
    {
        if (Input.GetKeyDown(quickSlash))
        {
            if (!stuck && !heavySlashing && (heavyEndLagTimer <= 0 && quickEndLagTimer <= 0))
            {
                AttemptQuickSlash();
            }
            else if (stuck && !heavySlashing)
            {
                AttemptDetach();
            }
        }
        if (Input.GetKeyDown(heavySlash))
        {
            if (heavyEndLagTimer <= 0 && quickEndLagTimer <= 0)
            {
                AttemptHeavySlash();
            }
        }

        CheckForImpact();

        //manage timers
        quickEndLagTimer -= Time.deltaTime;
        if(quickEndLagTimer <= 0 && quickSlashing) { quickSlashing = false; }
        quickActiveTimer -= Time.deltaTime;
        if(quickActiveTimer <= 0) { quickSlashHitbox.SetActive(false); }

        heavyEndLagTimer -= Time.deltaTime;
        if(heavyEndLagTimer <= 0 && heavySlashing) { heavySlashing = false; }
        heavyLeadUpTimer -= Time.deltaTime;
        if(heavyLeadUpTimer <= 0 && heavySlashing) { heavyAttachHitbox.SetActive(true); heavySlashHitbox.SetActive(true); heavyActiveTimer = heavyActiveTime; }
        heavyActiveTimer -= Time.deltaTime;
        if(heavyActiveTimer <= 0) { heavyAttachHitbox.SetActive(false); heavySlashHitbox.SetActive(false); }
        detachTimer -= Time.deltaTime;
        if(detachTimer <= 0 && detached) { detached = false; }
    }
    void AttemptQuickSlash()
    {
        quickSlashing = true;
        animator.SetTrigger("QuickSwing");
        quickEndLagTimer = quickEndLag;
        quickSlashHitbox.SetActive(true);
        quickActiveTimer = quickActiveTime;
    }
    void AttemptHeavySlash()
    {
        quickSlashing = false;
        heavySlashing = true;
        animator.SetTrigger("HeavySwing");
        heavyEndLagTimer = heavyEndLag;
        heavyLeadUpTimer = heavyLeadUp;

    }
    void AttemptDetach()
    {

    }
    void CheckForImpact()
    {
        if (quickSlashing && quickSlashHitbox.activeSelf)
        {
            List<GameObject> collisions = quickSlashHitbox.GetComponent<HitboxScript>().collidedObjects;
            for(int i = 0; i < collisions.Count; i++)
            {
                if (collisions[i].tag == "grunt core")
                {
                    collisions[i].GetComponent<HealthManager>().DealDamage(quickDmg);
                    collisions[i].GetComponent<Rigidbody>().AddForce(Vector3.Normalize(collisions[i].transform.position - transform.parent.parent.parent.position) * quickKnockback);

                }
                if (collisions[i].tag == "boss ring")
                {
                    transform.parent.parent.parent.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(transform.parent.parent.parent.position - collisions[i].transform.position) * quickSelfKnockback);
                }
                if (collisions[i].tag == "boss core")
                {
                    collisions[i].GetComponent<HealthManager>().DealDamage(quickDmg);
                }
                if (collisions[i].tag == "enemy projectile")
                {
                    Destroy(collisions[i]);
                    //TODO add reflecting 
                }
            }
        }
        if (heavySlashing && heavySlashHitbox.activeSelf)
        {
            //TODO heavyslash stuff
        }
    }
}

