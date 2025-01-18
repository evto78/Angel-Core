using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public int explosionDmg;
    public float explosionForce;

    void Start()
    {
        Destroy(gameObject, 0.05f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<HealthManager>(out HealthManager healthman))
        {
            if (!healthman.player)
            {
                healthman.DealDamage(explosionDmg);
            }
        }
        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddForce(Vector3.Normalize(other.transform.position - transform.position) * explosionForce, ForceMode.Impulse);
        }
    }
}
