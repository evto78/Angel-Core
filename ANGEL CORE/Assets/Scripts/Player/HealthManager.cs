using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    PlayerUI uiMan;

    public bool player = false;
    public int maxHealth;
    public int curHealth;

    public float invTime;
    float invTimer;

    void Awake()
    {
        curHealth = maxHealth;
        uiMan = GetComponent<PlayerUI>();
    }

    private void Update()
    {
        // for debugging, feel free to remove if needed
        if (Input.GetKeyDown(KeyCode.T) && player) { DealDamage(1); }

        // Timers
        invTimer -= Time.deltaTime;
    }

    public void DealDamage(int dmgAmt)
    {
        if(invTimer < 0)
        {
            curHealth -= dmgAmt;
            if (curHealth < 1) { Death(); }
            invTimer = invTime;
            uiMan.OnHurt();
        }
    }
    public void Heal(int healAmt)
    {
        curHealth += healAmt;
        if(curHealth > maxHealth) {curHealth = maxHealth;}
    }

    public void Death()
    {
        if (!player){Destroy(gameObject); return; }
        //player death state 
        gameObject.SendMessage("PlayerDied", SendMessageOptions.DontRequireReceiver);
    }

}
