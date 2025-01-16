using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    HealthManager Player;
    // Start is called before the first frame update
    void Start()
    {
        Player.GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision) 
    {
        if(gameObject.CompareTag("Player"))
        {
            Player.DealDamage(1);
        }
    }
}
