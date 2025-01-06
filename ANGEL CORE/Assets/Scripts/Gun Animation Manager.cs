using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimationManager : MonoBehaviour
{
    [SerializeField] Animator SpinnerAnimator;
    [SerializeField] ParticleSystem gunCharge;
    [SerializeField] ParticleSystem gunFire;

    float charge;
    public float maxCharge;

    // Start is called before the first frame update
    void Start()
    {
        SpinnerAnimator.SetBool("Spinning", false);
    }

    // Update is called once per frame
    void Update()
    {
        Spinner();
    }

    void Spinner()
    {
        SpinnerAnimator.ResetTrigger("Fire");

        if (Input.GetMouseButton(0))
        {
            SpinnerAnimator.SetBool("Spinning", true);
            gunCharge.Emit(Mathf.FloorToInt(charge * 100f * Time.deltaTime));
            charge += Time.deltaTime;
            if(charge > maxCharge) { charge = maxCharge; }
        }
        else
        {
            SpinnerAnimator.SetBool("Spinning", false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            SpinnerAnimator.SetTrigger("Fire");
            gunFire.Play();
        }
    }
}
