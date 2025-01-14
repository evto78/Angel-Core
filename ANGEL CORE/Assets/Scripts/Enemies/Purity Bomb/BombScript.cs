using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    float deathTimer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
        transform.position += Vector3.down * 4 * Time.deltaTime;
        
    }
    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            
        }
    }
}
