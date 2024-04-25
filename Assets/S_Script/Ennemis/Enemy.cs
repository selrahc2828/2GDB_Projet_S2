using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Reference")]
    public GameManager _GameManager;
    public Niveau1 _Niveau1;
    public FeedBack _FeedbackScript;
    public NavMeshAgent _NavMeshAgent;
    public Transform _TowerToDestroy;
    public NavMeshAgent _thisAgent;

    [Header("Health System")]
    public int _MaxHealth;
    public int _CurrentHealth;

    [Header("Slow System")]
    public float _slowDuration;
    public float _slowPower;
    public float _slowTimer;

    [ColorUsage(false, true)]
    public Color _initialOutlineColor;
    [ColorUsage(false, true)]
    public Color _SlowedOutlineColor;

    [Header("This Agent Data")]
    public float _thisAgentBaseSpeed;


    [Header("HP Feedback")]
    public MeshRenderer _meshRenderer;
    [ColorUsage(false, true)]
    public Color _finalColor;
    [ColorUsage(false, true)]
    public Color _initialColor;


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
        _slowTimer = _slowDuration;
    }

    public void ApplySlow()
    {
        _slowTimer -= Time.deltaTime;
        if(_slowTimer >= 0)
        {
            _thisAgent.speed = _slowPower * _thisAgentBaseSpeed;
            SlowedOutlineColor();
        }
        else
        {
            _thisAgent.speed = _thisAgentBaseSpeed;
            InitialOutlineColor();
        }
    }

    // call update to Current Health
    public int GetCurrentHealth()
    {
        return _CurrentHealth;
    }

    public void Die()
    {
        _FeedbackScript.DeathBubble(transform);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        _GameManager._numberOfEnemyOnScreen--;
        _Niveau1.CheckForNewtWave();
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            Die();
        }
    }

    public void ChangeColorOnHP()
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

    public void SlowedOutlineColor()
    {
        if (_meshRenderer.materials.Length >= 2)
        {      
            Material outlineMaterial = _meshRenderer.materials[1]; 
            // Changer la couleur de l'outline
            outlineMaterial.SetColor("_Color", _SlowedOutlineColor);
        }
    }

    public void InitialOutlineColor()
    {
        if (_meshRenderer.materials.Length >= 2)
        {
            Material outlineMaterial = _meshRenderer.materials[1];
            // Changer la couleur de l'outline a l'état initial
            outlineMaterial.SetColor("_Color", _initialOutlineColor);
        }
    }

}
