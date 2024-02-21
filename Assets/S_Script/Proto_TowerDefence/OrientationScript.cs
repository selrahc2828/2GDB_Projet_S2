using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrientationScript : MonoBehaviour
{

    [Header("Reference")]
    public Transform _towerTransform;
    public AgentToTrace _AgentTraceScript;


    void Update()
    {
        
    }

    public void FirstandLastAgent(List<NavMeshAgent> _AgentListe)
    {
        if (_AgentListe.Count > 0)
        {
            NavMeshAgent _firstAgent = _AgentListe[0];
            NavMeshAgent _lastAgent = _AgentListe[_AgentListe.Count - 1];
        }

    }

    public void AgentShoot()
    {

    }
}
