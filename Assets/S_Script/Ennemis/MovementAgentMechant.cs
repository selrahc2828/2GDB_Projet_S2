using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementAgentMechant : MonoBehaviour
{
    [Header("Reference")]
    public NavMeshAgent _NavMeshAgent;
    public Transform _TowerToDestroy;
    public GameManager _GameManagerScript;

    public void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }

    void Start()
    {

        // take the NavMeshagent
        _NavMeshAgent = GetComponent<NavMeshAgent>();

        // ParameterValue For Game Manager
        _NavMeshAgent.speed = _GameManagerScript._SpeedEnemy;
        _NavMeshAgent.angularSpeed = _GameManagerScript._AngularSpeedEnemy;
        _NavMeshAgent.acceleration = _GameManagerScript._AccelerationEnemy;

    }

    
    void Update()
    {
        // Set Destination to the tranform tower
        _NavMeshAgent.SetDestination(_TowerToDestroy.position);
    }
}
