using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxGattly : MonoBehaviour
{
    GameObject player;
    public int dmg;
    public float knockback;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<HealthManager>(out HealthManager healthMan))
        {
            healthMan.DealDamage(dmg);
        }
        player.GetComponent<Rigidbody>().AddForce(Camera.main.transform.up * knockback);
    }
}
