using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject gunHolder;

    public GameObject leftHand;
    public GameObject rightHand;

    int currentGun;
    //number is the hierarchy order under the gunholder AKA:
    //0 is Axe
    //1 is Circular Saw
    //2 is Chainsaw
    //3 is Revolver
    //4 is TommyGun
    //5 is Gattlygun
    //6 is Crossbow
    //7 is Heavy Rifle
    //8 is Prototype Heavy Crossbow

    // Start is called before the first frame update
    void Start()
    {
        //just set to revolver as default
        currentGun = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetString("lefthanded") == "true") { gunHolder.transform.SetParent(leftHand.transform); if (gunHolder.transform.position != leftHand.transform.position) { gunHolder.transform.position = leftHand.transform.position; gunHolder.transform.rotation = leftHand.transform.rotation; } }
        else if(PlayerPrefs.GetString("lefthanded") == "false") { gunHolder.transform.SetParent(rightHand.transform); if (gunHolder.transform.position != rightHand.transform.position) { gunHolder.transform.position = rightHand.transform.position; gunHolder.transform.rotation = leftHand.transform.rotation; } }
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            if (Input.GetMouseButton(0))
            {
                gunHolder.transform.GetChild(currentGun).gameObject.SendMessage("AttemptShoot", SendMessageOptions.DontRequireReceiver);
            }
            if (Input.GetMouseButtonUp(0))
            {
                gunHolder.transform.GetChild(currentGun).gameObject.SendMessage("AttemptShootUp", SendMessageOptions.DontRequireReceiver);
            }
            if (Input.GetMouseButton(1))
            {
                gunHolder.transform.GetChild(currentGun).gameObject.SendMessage("AttemptAltShoot", SendMessageOptions.DontRequireReceiver);
            }
            if (Input.GetMouseButtonUp(1))
            {
                gunHolder.transform.GetChild(currentGun).gameObject.SendMessage("AttemptAltShootUp", SendMessageOptions.DontRequireReceiver);
            }
            if (Input.GetKey(KeyCode.R))
            {
                gunHolder.transform.GetChild(currentGun).gameObject.SendMessage("AttemptReload", SendMessageOptions.DontRequireReceiver);
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                gunHolder.transform.GetChild(currentGun).gameObject.SendMessage("AttemptReloadUp", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    public void SetGun(int gun)
    {
        currentGun = gun;
        for(int i = 0; i < gunHolder.transform.childCount; i++)
        {
            if(i == currentGun)
            {
                gunHolder.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                gunHolder.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
