using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject gunHolder;

    public GameObject leftHand;
    public GameObject rightHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetString("lefthanded") == "true") { gunHolder.transform.SetParent(leftHand.transform); if (gunHolder.transform.position != leftHand.transform.position) { gunHolder.transform.position = leftHand.transform.position; gunHolder.transform.rotation = leftHand.transform.rotation; } }
        else if(PlayerPrefs.GetString("lefthanded") == "false") { gunHolder.transform.SetParent(rightHand.transform); if (gunHolder.transform.position != rightHand.transform.position) { gunHolder.transform.position = rightHand.transform.position; gunHolder.transform.rotation = leftHand.transform.rotation; } }

        if (Input.GetMouseButton(0))
        {
            gunHolder.transform.GetChild(0).gameObject.SendMessage("AttemptShoot", SendMessageOptions.DontRequireReceiver);
            
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gunHolder.transform.GetChild(0).gameObject.SendMessage("AttemptReload", SendMessageOptions.DontRequireReceiver);
        }
    }
}
