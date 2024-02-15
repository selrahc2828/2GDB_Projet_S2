using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementAgentMechant : MonoBehaviour
{

    [Header("Reference")]
    public NavMeshAgent _NavMeshAgent;
    public Transform _TowerToDestroy;


    void Start()
    {
        _NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        _NavMeshAgent.SetDestination(_TowerToDestroy.position);
    }
}
