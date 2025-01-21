using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private bool Bombing;
    public GameObject PurityBombPrefab;
    bool sendIt; 
    UnityEvent ev_Purification;
    Transform Spawner;
    // Start is called before the first frame update
    void Start()
    {
        Spawner = GameObject.Find("BombSpawner").transform;
        sendIt = true;
        ev_Purification = new UnityEvent();
        ev_Purification.AddListener(ConfirmBombing);
    }

    // Update is called once per frame
    void Update()
    {
        //pre check so it only sends 1 bomb
        if(Bombing && sendIt)
        {
            ev_Purification.Invoke();
            Debug.Log("bah");
            sendIt = false;
        }
    }  
    // the actual bomb
    void ConfirmBombing()
    {
            GameObject bullet = Instantiate(PurityBombPrefab, Spawner.position, transform.rotation);
            Debug.Log("BEAR WITNESS!!");
    }      
}

