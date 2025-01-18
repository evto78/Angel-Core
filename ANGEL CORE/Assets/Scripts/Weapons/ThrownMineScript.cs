using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownMineScript : MonoBehaviour
{
    public GameObject explosion;
    public int dmg;
    public float force;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject spawnedExplosion = Instantiate(explosion);
        spawnedExplosion.transform.position = transform.position;
        spawnedExplosion.GetComponent<ExplosionScript>().explosionDmg = dmg;
        spawnedExplosion.GetComponent<ExplosionScript>().explosionForce = force;
        Destroy(gameObject);
    }
}
