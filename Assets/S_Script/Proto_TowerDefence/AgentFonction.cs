using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentFonction : MonoBehaviour
{
    [Header("Reference")]
    public AgentToTrace _AgentDispo;
    public AgentChoise _AgentUsable;

    [Header("Choise Comportement")]
    public bool _ShootEnemy;
    public bool _SlowEnemy;

    [Header("Slow Parameters")]
    public float _slowdownSpeed;
    public float _InitialSpeed;

    [Header("Weapon Reference")]
    public GameObject _gun;
    public Transform _BulletSpawnPosition;
    public Transform _TowerPosition;

    [Header("Weapon Parameter")]
    public int _damageAmount = 10;
    public float _fireRate = 0.5f;
    public float _shootDistance = 50f;
    public LayerMask _shootableLayer;
    private float _nextFireTime = 0f;

    private void Start()
    {
        _AgentDispo = GameObject.FindObjectOfType<AgentToTrace>();
        _AgentUsable = GameObject.FindObjectOfType<AgentChoise>();
    }


    private void Update()
    {
        if (!IsAgentUsable(GetComponent<NavMeshAgent>()) && _ShootEnemy && Time.time >= _nextFireTime)
        {
            ShootToEnemy();

            _nextFireTime = Time.time + 1f / _fireRate;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_SlowEnemy && other.CompareTag("AgentMechant"))
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
        RaycastHit hit;
        Vector3 _directionOpposer = transform.position - _TowerPosition.position;

        if (Physics.Raycast(_BulletSpawnPosition.position, _directionOpposer, out hit, _shootDistance))
        {
            HeathEnemy _EnemyHealth = hit.collider.GetComponent<HeathEnemy>();
            
            if (_EnemyHealth != null)
            {
                _EnemyHealth.TakeDamage(_damageAmount);
                Debug.DrawRay(_BulletSpawnPosition.position, hit.point - _BulletSpawnPosition.position, Color.red, 1);
                if (_EnemyHealth.GetCurrentHealth() <= 0)
                {
                    _EnemyHealth.Die();
                }
            }
            
        }

        
    }

    public bool IsAgentUsable(NavMeshAgent agent)
    {
        if (_AgentDispo._listeAgent.ContainsKey(agent))
        {
            return _AgentDispo._listeAgent[agent];
        }
        else
        {
            return false;
        }
    }
}
