using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public Sprite heartSprite;
    public Sprite deadHeartSprite;

    public Image radialCharge;

    public GameObject pauseMenu;
    public GameObject deathScreen;
    float deathscreenfade;
    float storedTimeScale;

    HealthManager healthman;

    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI ammoText;
    public int curBullets; // modified from the gunscripts
    public int maxBullets; // modified from the gunscripts
    Rigidbody rb;

    void Start()
    {
        Time.timeScale = 1f;
        deathScreen.SetActive(false);
        healthman = GetComponent<HealthManager>();
        rb = GetComponent<Rigidbody>();

        //lock cursor in game and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        velocityText.text = "Vel: " + Mathf.RoundToInt(rb.velocity.magnitude).ToString();
        ammoText.text = curBullets.ToString() + " / " + maxBullets.ToString();

        if(healthman.curHealth > 0) { heart1.GetComponent<Image>().sprite = heartSprite; } else { heart1.GetComponent<Image>().sprite = deadHeartSprite; }
        if(healthman.curHealth > 1) { heart2.GetComponent<Image>().sprite = heartSprite; } else { heart2.GetComponent<Image>().sprite = deadHeartSprite; }
        if(healthman.curHealth > 2) { heart3.GetComponent<Image>().sprite = heartSprite; } else { heart3.GetComponent<Image>().sprite = deadHeartSprite; }

        if (Input.GetKeyDown(KeyCode.Escape) && deathScreen.activeSelf == false)
        {
            if (pauseMenu.activeSelf) { UnPause(); }
            else { Pause(); }
        }
        if(deathScreen.activeSelf == true)
        {
            Time.timeScale = deathscreenfade;
            deathScreen.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, deathscreenfade);
            deathscreenfade -= Time.deltaTime;
            if (deathscreenfade < 0.2f) { deathscreenfade = 0.2f; }
        }
    }
    public void PlayerDied()
    {
        if(deathScreen.activeSelf == false)
        {
            //player death state

            UnPause();
            Time.timeScale = 1f;
            deathScreen.SetActive(true);
            deathScreen.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 1f);
            deathscreenfade = 1f;


            //unlock cursor in game and show it
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void Pause()
    {
        //unlock cursor in game and show it
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenu.SetActive(true);
        storedTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }
    public void UnPause()
    {
        //lock cursor in game and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenu.SetActive(false);
        Time.timeScale = storedTimeScale;
    }
    public void ExitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
    public void Respawn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Boss Testing");
    }
}
