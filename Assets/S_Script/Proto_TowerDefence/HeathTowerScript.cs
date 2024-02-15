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
        _CurrentHealth = _MaxHealth;

    }

    private void Update()
    {
        GetCurrentHealth();
        
        _HealtHp.text = "HP : " + _CurrentHealth;

        if (_CurrentHealth <= 0 )
        {
            Debug.Log("Defeat");
        }
    }

    
    public void TakeDamage(int damageAmount)
    {
        _CurrentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took damage: " + damageAmount);
    }



    public int GetCurrentHealth()
    {
        Debug.Log(_CurrentHealth);
        return _CurrentHealth;
    }
}
