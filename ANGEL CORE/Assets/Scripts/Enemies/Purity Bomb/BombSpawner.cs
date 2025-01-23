using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BombSpawner : MonoBehaviour
{
    BossMovement Bombing;
    public GameObject bomber;
    public GameObject PurityBombPrefab;
    bool sendIt; 
    UnityEvent ev_Purification;
    Transform Spawner;
    public bool lightSwitch;
    
    // Start is called before the first frame update
    void Start()
    {
        Bombing = bomber.GetComponent<BossMovement>();
        Spawner = GameObject.Find("BombSpawner").transform;
        sendIt = true;
        ev_Purification = new UnityEvent();
        ev_Purification.AddListener(ConfirmBombing);
        ev_Purification.AddListener(ChangeLight);
    }


    // Update is called once per frame
    void Update()
    {
        
        //pre check so it only sends 1 bomb
        if(Bombing.Bombing && sendIt)
        {
            ev_Purification.Invoke();
            Debug.Log("bah");
            sendIt = false;
        }
    }  
    // the actual bomb
    void ConfirmBombing()
    {
            Instantiate(PurityBombPrefab, Spawner.position, transform.rotation);
            Debug.Log("Bomb spawned");
    }      
    void ChangeLight()
    {
        lightSwitch = true;
    }
}

