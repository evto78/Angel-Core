using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int dmg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision) 
    {
        if(collision.gameObject.name == "Player")
        {
            Debug.Log("hit");
            collision.gameObject.GetComponent<HealthManager>().DealDamage(dmg);
        }
    }
}
