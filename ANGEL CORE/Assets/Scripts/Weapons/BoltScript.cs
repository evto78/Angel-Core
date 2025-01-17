using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltScript : MonoBehaviour
{
    public int dmg;
    public GameObject hitEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.gameObject.tag == "boss core" || collision.gameObject.transform.gameObject.tag == "grunt core")
        {
            collision.gameObject.GetComponent<HealthManager>().DealDamage(dmg);
            GameObject spawnedEffect = Instantiate(hitEffect);
            spawnedEffect.transform.position = transform.position;
            Destroy(spawnedEffect, 5f);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "boss ring")
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
