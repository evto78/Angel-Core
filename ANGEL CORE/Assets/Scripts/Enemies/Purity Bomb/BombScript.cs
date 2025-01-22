using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    float deathTimer = 3f;
    bool goDown;
    GameObject bomba;
    // Start is called before the first frame update
    void Start()
    {
        bomba = GameObject.Find("PurityBomb");
    }

    // Update is called once per frame
    void Update()
    {
        
        if(goDown)
        {transform.position += Vector3.down * 4 * Time.deltaTime;}
        
    }
    private void OnCollisionEnter(Collision collision) 
    {
        goDown = false;
    }
}
