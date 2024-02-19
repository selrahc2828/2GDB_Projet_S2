using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeathTowerScript : MonoBehaviour
{
    [Header("Reference")]
    public Text _HealtHp;
    

    [Header("HealthSystem")]
    public int _MaxHealth;
    public int _CurrentHealth;


    void Start()
    {
        // Set CurrentHealth to Max Health
        _CurrentHealth = _MaxHealth;
    }

    private void Update()
    {
        // Update The Current Healt 
        GetCurrentHealth();
        // Change the text 
        _HealtHp.text = "HP : " + _CurrentHealth;

        // DEFEAT
        if (_CurrentHealth <= 0 )
        {
            Debug.Log("Defeat");
        }
    }

    // Fonction is called in DamageToTower Script 
    public void TakeDamage(int damageAmount)
    {
        // Take the current Health and substract the damage amount 
        _CurrentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took damage: " + damageAmount);
    }

    // call update to Current Health
    public int GetCurrentHealth()
    {
        return _CurrentHealth;
    }
}
