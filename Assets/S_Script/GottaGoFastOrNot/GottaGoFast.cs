using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GottaGoFast : MonoBehaviour
{
    [Header("paramater")]
    public float _AgentSpeedFast;
    public float _AgentSpeedSlow;
    public float _InitialSpeed;

    [Header("Booléen")]
    public bool _Fast;
    public bool _Slow;




    private void OnTriggerStay(Collider other)
    {
        if (_Fast == true)
        {
            NavMeshAgent _Agent = other.GetComponent<NavMeshAgent>();
            _Agent.speed = _AgentSpeedFast;
        }

        if (_Slow == true)
        {
            NavMeshAgent _Agent = other.GetComponent<NavMeshAgent>();
            _Agent.speed = _AgentSpeedSlow;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NavMeshAgent _Agent = other.GetComponent<NavMeshAgent>();
        _Agent.speed = _InitialSpeed; 
    }
}
