using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeathTowerScript : MonoBehaviour
{
    [Header("Reference")]
    public Text _HealtHp;
    public GameManager _GameManagerScript;
    public MeshRenderer _meshRenderer;
    

    [Header("HealthSystem")]
    public int _MaxHealth;
    public int _CurrentHealth;

    [Header("Color")]
    [ColorUsage(false, true)]
    public Color _initialColor;
    [ColorUsage(false, true)]
    public Color _finalColor;


    public void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }

    void Start()
    {

        _MaxHealth = _GameManagerScript._HeathTower;

        // Set CurrentHealth to Max Health
        _CurrentHealth = _MaxHealth;
    }

    private void Update()
    {
        ChangeColorOnHP();

        // Update The Current Healt 
        GetCurrentHealth();
        // Change the text 
        _HealtHp.text = "HP : " + _CurrentHealth;

        // DEFEAT
        if (_CurrentHealth <= 0 )
        {
            _GameManagerScript._gameLose = true;
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

    private void ChangeColorOnHP()
    {
        if (_MaxHealth <= 0)
            return;

        float healthPercentage = (float)_CurrentHealth / _MaxHealth;

        Color lerpedColor;
        float intensity;

        if (_CurrentHealth <= 0)
        {
            lerpedColor = _finalColor; 
            intensity = 0f;
        }
        else
        {
            lerpedColor = Color.Lerp(_initialColor, _finalColor, healthPercentage);
            intensity = 1f;
        }

        lerpedColor *= intensity;
        _meshRenderer.material.SetColor("_EmissionColor", lerpedColor);
    }    
}
