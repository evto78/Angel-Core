using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public bool player = false;
    public int maxHealth;
    public int curHealth;
    void Start()
    {
        curHealth = maxHealth;
    }

    public void DealDamage(int dmgAmt)
    {
        curHealth -= dmgAmt;
        if(curHealth < 1) { Death(); }
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
