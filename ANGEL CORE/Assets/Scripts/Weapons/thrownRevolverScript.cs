using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrownRevolverScript : MonoBehaviour
{
    public GameObject hitEffect;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject spawnedEffect = Instantiate(hitEffect);
        spawnedEffect.transform.position = transform.position;
        Destroy(spawnedEffect, 5f);
        Destroy(gameObject);
    }
}
