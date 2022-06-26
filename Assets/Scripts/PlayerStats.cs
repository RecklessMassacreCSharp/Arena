using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 1000;
    public int curHealth;

    public int maxMana = 1000;
    public int curMana;

    public BaseBar healthBar;
    public BaseBar manaBar;

    void Start()
    {
        curHealth = maxHealth;
        curMana = maxMana;

        healthBar.SetMaxBar(maxHealth);
        manaBar.SetMaxBar(maxMana);
    }

    public void TakeDamage(int damage) {
        curHealth -= damage;
        if (curHealth < 0)
            curHealth = 0;
        
        healthBar.SetBar(curHealth);
    }

    public void SpendMana(int mana) {
        curMana -= mana;
        if (curMana < 0)
            curMana = 0;

        manaBar.SetBar(curMana);
    }
}
