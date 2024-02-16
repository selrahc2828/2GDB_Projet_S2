using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentFonction : MonoBehaviour
{
    [Header("Choise Comportement")]
    public bool _ShootEnemy;
    public bool _SlowEnemy;

    [Header("Slow Parameters")]
    public float _slowdownSpeed;
    public float _InitialSpeed;


    public SphereCollider _slowSphereCollider;


    private void Update()
    {
        ShootToEnemy();
    }


    private void OnTriggerEnter(Collider other)
    {
  
        if (_SlowEnemy == true && other.CompareTag("AgentMechant"))
        {
            NavMeshAgent enemyAgent = other.GetComponent<NavMeshAgent>();

            if (enemyAgent != null)
            {
                enemyAgent.speed = _slowdownSpeed;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
     
        if (!_SlowEnemy && other.CompareTag("AgentMechant"))
        {
     
            NavMeshAgent enemyAgent = other.GetComponent<NavMeshAgent>();

      
            if (enemyAgent != null)
            {
                enemyAgent.speed = _InitialSpeed;
            }
        }
    }

    public void ShootToEnemy()
    {


    }
}
