using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimationManager : MonoBehaviour
{
    [SerializeField] ParticleSystem gunCharge;
    [SerializeField] ParticleSystem gunFire;
    public GameObject gunSpinner;

    float charge;
    public float maxCharge;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Spinner();
    }

    void Spinner()
    {
        gunSpinner.transform.Rotate(Vector3.right * charge * 500f * Time.deltaTime);
        gunSpinner.transform.Rotate(Vector3.right * 50f * Time.deltaTime);

        if (Input.GetMouseButton(0))
        {
            gunCharge.Emit(Mathf.FloorToInt(charge * 100f * Time.deltaTime));
            charge += Time.deltaTime;
            if(charge > maxCharge) { charge = maxCharge; }
        }
        else
        {

        }

        if (Input.GetMouseButtonUp(0))
        {
            //gunFire.Play();
            charge = 0;
        }
    }
}
