using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    float deathTimer = 3f;
    bool goDown;
    Transform Player;
    Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        goDown = true;
        Player = GameObject.Find("Player").transform;     

    }

    // Update is called once per frame
    void Update()
    {

        if(goDown)
        {transform.position += Vector3.down * 4 * Time.deltaTime;}
        else
        {
            Debug.Log("touchdown");
            Vector3 dir = Player.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(dir);
            transform.rotation = rotation;
            ray = new Ray(transform.position, transform.forward); 
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("yay you died");
                    Destroy(gameObject, 0.2f);
                }
                else
                {
                    Debug.Log("you survived");
                    Destroy(gameObject, 0.2f);
                }
            }
            Debug.DrawRay(transform.position, transform.forward, Color.black);

        }

        if(transform.position.y <= 0)
        {
            goDown = false;
        }
        
    }

}
