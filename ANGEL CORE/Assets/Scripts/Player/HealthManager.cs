using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    void Start()
    {
        curHealth = maxHealth;
    }

    void DealDamage(int dmgAmt)
    {
        curHealth -= dmgAmt;
        if(curHealth < 0) {curHealth = 0;}
    }
    void Heal(int healAmt)
    {
        curHealth += healAmt;
        if(curHealth > maxHealth) {curHealth = maxHealth;}
    }

    public void Death()
    {
        Destroy(gameObject);
    }

}
