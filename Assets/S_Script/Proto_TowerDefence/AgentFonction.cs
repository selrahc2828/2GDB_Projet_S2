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
    private float _nextFireTime = 0f;

    [Header("Particle System")]
    public ParticleSystem _projectileParticleSystem;

    private void Start()
    {
        _AgentDispo = GameObject.FindObjectOfType<AgentToTrace>();
        _AgentUsable = GameObject.FindObjectOfType<AgentChoise>();
        _projectileParticleSystem= GetComponentInChildren<ParticleSystem>();

        _projectileParticleSystem.Stop();
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
        if (_ShootEnemy == true)
        {
            _projectileParticleSystem.Play();
        }
        else
        {
            _projectileParticleSystem.Stop();
        }
  
                //HeathEnemy enemyHealth = collisionEvents[i].colliderComponent.GetComponent<HeathEnemy>();

                //if (enemyHealth != null)
                //{
                //    enemyHealth.TakeDamage(_damageAmount);
                //    if (enemyHealth.GetCurrentHealth() <= 0)
                //    {
                //        enemyHealth.Die();
                //    }
                //}
            
   
    }

    public bool IsAgentUsable(NavMeshAgent agent)
    {
        if (_AgentDispo._dictionnaireAgent.ContainsKey(agent))
        {
            return _AgentDispo._dictionnaireAgent[agent];
        }
        else
        {
            return false;
        }
    }
}
