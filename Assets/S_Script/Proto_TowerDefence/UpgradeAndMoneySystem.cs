using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeAndMoneySystem : MonoBehaviour
{
    

    [Header("Reference")]
    [SerializeField] private List<AgentFonction> _allAgents;
    public Text _ActualDamage;
    public Text _FireRate;
    public Text _Range;
    public GameObject _PanelMenu;

    [Header("Limit")]
    public float _MaxDamageValue;
    public float _MaxFireRateValue;
    public float _MaxRangeValue;


    [Header("Money")]
    public int _moneyNumber;
    public Text _MoneyText;

    private bool _isMenuOpen = false;
    public GameManager _GameManagerScript;

    public void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }
    void Start()
    {
        _moneyNumber = 0;
        _allAgents = new List<AgentFonction>(FindObjectsOfType<AgentFonction>());
    }

    private void Update()
    {
        _MoneyText.text = "Money  : " + _moneyNumber.ToString();
    }

    public void OpenMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        _GameManagerScript._gamePaused = !_GameManagerScript._gamePaused;
        _PanelMenu.SetActive(_isMenuOpen);
    }

    #region ShootDamage
    public void UpgradeShootDamage1()
    {
        foreach (AgentFonction agent in _allAgents)
        {
            agent._damageAmount = (int)Mathf.Clamp(agent._damageAmount + 5f, 0f, _MaxDamageValue);
            _ActualDamage.text = "Actual Damage = " + agent._damageAmount;
        }
    }
    #endregion

    #region ShootRange
    public void UpgradeShootRange1()
    {
        foreach (AgentFonction agent in _allAgents)
        {
            agent._ShootRange = Mathf.Clamp(agent._ShootRange + 5f, 0f, _MaxRangeValue);
            _Range.text = "Actual Range = " + agent._ShootRange;
        }
    }
    #endregion

    #region ShootCadence
    public void UpgradeShootFireRate1()
    {
        foreach (AgentFonction agent in _allAgents)
        {
            agent._fireRate = Mathf.Clamp(agent._fireRate + 0.5f, 0f, _MaxFireRateValue);
            _FireRate.text = "Actual FireRate = " + agent._fireRate;
        }
    }
    #endregion
}
