using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeathEnemy : MonoBehaviour
{
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

        // DEFEAT
        if (_CurrentHealth <= 0)
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

        if (_CurrentHealth < 0)
        {
            Die();
        }
    }

    // call update to Current Health
    public int GetCurrentHealth()
    {
        return _CurrentHealth;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
