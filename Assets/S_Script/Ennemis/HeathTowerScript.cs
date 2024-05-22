using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeathTowerScript : MonoBehaviour
{
    [Header("Reference")]
    public GameManager _GameManagerScript;
    public MeshRenderer _meshRenderer1;
    public MeshRenderer _meshRenderer2;
    public MeshRenderer _meshRenderer3;
    public Animator _AnimationHP;
    public Text _healthPercentageText;

    [Header("Particul")]
    public GameObject _ParticulUp;
    public GameObject _ParticulMiddle;
    public GameObject _ParticulDown;
    public GameObject _ParticulRipple;



    [Header("HealthSystem")]
    public int _MaxHealth;
    public int _CurrentHealth;
    public float healthPercentageVariable;

    [Header("Color")]
    [ColorUsage(false, true)]
    public Color _initialColor;
    [ColorUsage(false, true)]
    public Color _finalColor;


    [Header("Sound")]
    public EventReference _FMODDamag;


    public void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }

    void Start()
    {
        _MaxHealth = _GameManagerScript._HeathTower;

        // Set CurrentHealth to Max Health
        _CurrentHealth = _MaxHealth;

        _ParticulUp.SetActive(false);
        _ParticulMiddle.SetActive(false);
        _ParticulDown.SetActive(false);
    }

    private void Update()
    {
        ChangeColorOnHP();

        // Update The Current Healt 
        GetCurrentHealth();
        // Change the text 
        // _HealtHp.text = "HP : " + _CurrentHealth;

        // DEFEAT
        if (_CurrentHealth <= 0 )
        {
            _GameManagerScript._gameLose = true;
            Debug.Log("Defeat");
        }

        if (_CurrentHealth <= 400f)
        {
            _ParticulDown.SetActive(true);
        }

        if (_CurrentHealth <= 300f)
        {
            _ParticulMiddle.SetActive(true);
        }

        if (_CurrentHealth <= 200f)
        {
            _ParticulUp.SetActive(true);
        }

        healthPercentageVariable = (float)_CurrentHealth / _MaxHealth * 100;
        _healthPercentageText.text = "HP : " + Mathf.RoundToInt(healthPercentageVariable) + "%";
    }

    // Fonction is called in DamageToTower Script 
    public void TakeDamage(int damageAmount)
    {
        // Take the current Health and substract the damage amount 
        _CurrentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took damage: " + damageAmount);

        GameObject rippleParticle = Instantiate(_ParticulRipple, transform.position, Quaternion.identity);
        Destroy(rippleParticle, 1f);


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

        if (_CurrentHealth <= 0)
        {
            lerpedColor = _finalColor; 
        }
        else
        {
            lerpedColor = Color.Lerp(_initialColor, _finalColor, healthPercentage);
        }

        _meshRenderer1.material.SetColor("_EmissionColor", lerpedColor);
        _meshRenderer2.material.SetColor("_EmissionColor", lerpedColor);
        _meshRenderer3.material.SetColor("_EmissionColor", lerpedColor);

        _meshRenderer1.materials[1].SetColor("_Color", lerpedColor);
        _meshRenderer2.materials[1].SetColor("_Color", lerpedColor);
        _meshRenderer3.materials[1].SetColor("_Color", lerpedColor);

        _AnimationHP.SetFloat("Blend", healthPercentage);
    }    
}
