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
        // take the NavMeshagent
        _NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        // Set Destination to the tranform tower
        _NavMeshAgent.SetDestination(_TowerToDestroy.position);
    }
}
