using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeathEnemy : MonoBehaviour
{
    [Header("Reference")]
    public GameManager _GameManagerScript;

    [Header("HealthSystem")]
    public int _MaxHealth;
    public int _CurrentHealth;
    public float _droppChance;



    public void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }


    void Start()
    {

        _droppChance = 0.2f;
        _MaxHealth = _GameManagerScript._HeathEnemy;

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
        //Debug.Log(gameObject.name + " took damage: " + damageAmount);

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
    private void OnDestroy()
    {
        // Generate a random number between 0 and 1
        float randomValue = Random.value;

        // Check if the random number is less than or equal to 0.2 (20% chance)
        if (randomValue <= _droppChance)
        {
            //Instantiate
        }
    }
}
