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
    public int _damageAmount;
    public float _fireRate;
    public float _shootDistance;
    

    [Header("Particle System & Trail Renderer")]
    public ParticleSystem _projectileParticleSystem;
    public TrailRenderer _TrailBullet;
    

    private void Start()
    {
        _AgentDispo = GameObject.FindObjectOfType<AgentToTrace>();
        _AgentUsable = GameObject.FindObjectOfType<AgentChoise>();
        _projectileParticleSystem= GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (!IsAgentUsable(GetComponent<NavMeshAgent>()) && _ShootEnemy)
        {
            ShootToEnemy();
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
            TrailRenderer _Trail = Instantiate(_TrailBullet, _BulletSpawnPosition.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(_Trail, hit));

            HeathEnemy _EnemyHealth = hit.collider.GetComponent<HeathEnemy>();
            
            if (_EnemyHealth != null)
            {
                _EnemyHealth.TakeDamage(_damageAmount);
                
                if (_EnemyHealth.GetCurrentHealth() <= 0)
                {
                    _EnemyHealth.Die();
                }
            }

            //Debug.DrawRay(_BulletSpawnPosition.position, hit.point - _BulletSpawnPosition.position, Color.red, 1);

        }
   }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit hit)
    {

        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);

            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        Trail.transform.position = hit.point;

        Destroy(Trail.gameObject, Trail.time);

    }


    // Agent Usable or not
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
