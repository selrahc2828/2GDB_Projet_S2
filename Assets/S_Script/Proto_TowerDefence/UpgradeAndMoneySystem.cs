using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeAndMoneySystem : MonoBehaviour
{
    public int _moneyNumber;

    [Header("Reference")]
    [SerializeField] private List<AgentFonction> _allAgents;
    public Text _ActualDamage;
    public Text _FireRate;
    public Text _Range;

    [Header("Limit")]
    public int _MaxDamageValue;
    public int _MaxFireRateValue;
    public int _MaxRangeValue;
    public float _MinValue;

    void Start()
    {
        _moneyNumber = 0;
        _allAgents = new List<AgentFonction>(FindObjectsOfType<AgentFonction>());
    }

    #region ShootDamage
    public void UpgradeShootDamage1()
    {
        foreach (AgentFonction agent in _allAgents)
        {
            agent._damageAmount = Mathf.Clamp(agent._damageAmount + 5, 0, _MaxDamageValue);
            _ActualDamage.text = "Actual Damage = " + agent._damageAmount;
        }
    }
    #endregion


    #region ShootRange
    public void UpgradeShootRange1()
    {
        foreach (AgentFonction agent in _allAgents)
        {
            agent._ShootRange = Mathf.Clamp(agent._ShootRange + 5, 0, _MaxRangeValue);
            _Range.text = "Actual Range = " + agent._ShootRange;
        }
    }
    #endregion



    #region ShootCadence
    public void UpgradeShootFireRate1()
    {
        foreach (AgentFonction agent in _allAgents)
        {
            agent._fireRate = Mathf.Clamp(agent._fireRate - 0.5f, _MinValue, _MaxFireRateValue);
            _FireRate.text = "Actual FireRate = " + agent._fireRate;
        }
    }
    #endregion


   
}
