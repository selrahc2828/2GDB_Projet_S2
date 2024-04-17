using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HealthEnemy : MonoBehaviour
{
    [Header("Reference")]
    public GameManager _GameManager;
    public UpgradeAndMoneySystem _UpgradeAndMoneySystemScript;

    [Header("Health System")]
    public int _MaxHealth;
    public int _CurrentHealth;
    public float _droppChance;

    [Header("Slow System")]
    public float _slowDuration;
    public float _slowPower;
    public float _slowTimer;

    [Header("This Agent Data")]
    public NavMeshAgent _thisAgent;
    public float _thisAgentBaseSpeed;


    [Header("HP Feedback")]
    public MeshRenderer _meshRenderer;
    [ColorUsage(false, true)]
    public Color _finalColor;
    [ColorUsage(false, true)]
    public Color _initialColor;

    public void Awake()
    {
        _UpgradeAndMoneySystemScript = FindAnyObjectByType<UpgradeAndMoneySystem>();
        _GameManager = FindAnyObjectByType<GameManager>();
        _thisAgent = this.GetComponentInParent<NavMeshAgent>();
    }


    void Start()
    {
        _slowPower = _GameManager._slowPower;
        _slowDuration = _GameManager._slowDuration;
        _droppChance = 0.2f;
        _MaxHealth = _GameManager._HeathEnemy;
        _GameManager._numberOfEnemyOnScreen++;
        // Set CurrentHealth to Max Health
        _CurrentHealth = _MaxHealth;
        _thisAgentBaseSpeed = _thisAgent.speed;
    }

    private void Update()
    {
        // Update The Current Healt 
        GetCurrentHealth();
        ApplySlow();
        ChangeColorOnHP();
    }

    // Fonction is called in DamageToTower Script 
    public void TakeDamage(int damageAmount)
    {
        // Take the current Health and substract the damage amount 
        _CurrentHealth -= damageAmount;
        //Debug.Log(gameObject.name + " took damage: " + damageAmount);

        if (_CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void GetSlowed()
    {
        Debug.Log(_thisAgent.speed);
        _slowTimer = _slowDuration;
        Debug.Log(_thisAgent.speed);
    }

    public void ApplySlow()
    {
        _slowTimer -= Time.deltaTime;
        if(_slowTimer >= 0)
        {
            _thisAgent.speed = _slowPower * _thisAgentBaseSpeed;
        }
        else
        {
            _thisAgent.speed = _thisAgentBaseSpeed;
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
        _GameManager._numberOfEnemyOnScreen--;

        // Check if the random number is less than or equal to 0.2 (20% chance)
        if (randomValue <= _droppChance)
        {
            _UpgradeAndMoneySystemScript._moneyNumber += 1;
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            Die();
        }
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
