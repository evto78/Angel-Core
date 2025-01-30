using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    float deathTimer = 3f;
    bool goDown;
    GameObject Player;
    Ray ray;
    public bool finished;
    // Start is called before the first frame update
    void Start()
    {
        goDown = true;
        finished = false;
        Player = GameObject.Find("Player");     

    }

    // Update is called once per frame
    void Update()
    {

        if(goDown)
        {transform.position += Vector3.down * 4 * Time.deltaTime;}
        else
        {
            Debug.Log("touchdown");
            Vector3 dir = Player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(dir);
            transform.rotation = rotation;
            ray = new Ray(transform.position, transform.forward); 
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.gameObject.tag == "Player")
                {
                    Debug.Log("yay you died");
                    Destroy(gameObject, 0.2f);
                    Player.GetComponent<HealthManager>().DealDamage(100);
                }
                else
                {
                    Debug.Log("you survived");
                    finished = true;
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
