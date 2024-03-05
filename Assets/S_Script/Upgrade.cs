using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [Header("Reference")]
    public List<AgentFonction> _allAgents;


    [Header("Limit")]
    public int _MaxValue;
    public int _MinValue;


    void Start()
    {
        _allAgents = new List<AgentFonction>(FindObjectsOfType<AgentFonction>());
    }

    #region ShootDamage
    public void UpgradeShootDamage1()
    {
        foreach (AgentFonction agent in _allAgents)
        {
            agent._damageAmount = Mathf.Clamp(agent._damageAmount + 5, 0, _MaxValue);
        }
    }
    #endregion


    #region ShootRange
    public void UpgradeShootRange1()
    {
        foreach (AgentFonction agent in _allAgents)
        {
            agent._ShootRange = Mathf.Clamp(agent._ShootRange + 5, 0, _MaxValue);
        }
    }
    #endregion



    #region ShootCadence
    public void UpgradeShootFireRate1()
    {
        foreach (AgentFonction agent in _allAgents)
        {
            agent._fireRate = Mathf.Clamp(agent._fireRate - 0.5f, _MinValue, _MaxValue); 
        }
    }
    #endregion
}
