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
    public float quickEndLag;
    float quickEndLagTimer;
    bool quickSlashing;
    public KeyCode heavySlash;
    public float heavyLeadUp;
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

        //manage timers
        quickEndLagTimer -= Time.deltaTime;
        if(quickEndLagTimer <= 0 && quickSlashing) { quickSlashing = false; }
        heavyEndLagTimer -= Time.deltaTime;
        if(heavyEndLagTimer <= 0 && heavySlashing) { heavySlashing = false; }
        detachTimer -= Time.deltaTime;
        if(detachTimer <= 0 && detached) { detached = false; }
    }
    void AttemptQuickSlash()
    {
        quickSlashing = true;
        animator.SetTrigger("QuickSwing");
        quickEndLagTimer = quickEndLag;
    }
    void AttemptHeavySlash()
    {
        quickSlashing = false;
        heavySlashing = true;
        animator.SetTrigger("HeavySwing");
        heavyEndLagTimer = heavyEndLag;
    }
    void AttemptDetach()
    {

    }
}

