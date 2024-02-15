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
    public int _MaxHealth = 100;
    public int _CurrentHealth;


    void Start()
    {
        _CurrentHealth = _MaxHealth;

    }

    private void Update()
    {
        GetCurrentHealth();

        _HealtHp.text = "HP : " + _CurrentHealth;
    }

    
    public void TakeDamage(int damageAmount)
    {
        _CurrentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took damage: " + damageAmount);

        if (_CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
    }


    public int GetCurrentHealth()
    {
        return _CurrentHealth;
    }
}
