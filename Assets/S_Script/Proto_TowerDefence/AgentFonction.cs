using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentFonction : MonoBehaviour
{
    [Header("Reference")]
    public AgentToTrace _AgentDispo;
    public AgentChoise _AgentUsable;
    public GameManager _GameManagerScript;
    public SphereCollider _ColliderTrigger;
    private GameObject currentTargetEnemy;
    public NavMeshAgent _NavMeshAgent;

    [Header("Layer")]
    public LayerMask _BulletLayer;

    [Header("RangeShoot And Slow")]
    public float _SlowRange;
    public float _ShootRange; 


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
    private float time;
    

    [Header("Particle System & Trail Renderer")]
    public ParticleSystem _projectileParticleSystem;
    public TrailRenderer _TrailBullet;



    public void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }


    private void Start()
    {
        time = 0f;

        // Get the script for all the agent in scene
        _AgentDispo = GameObject.FindObjectOfType<AgentToTrace>();
        _AgentUsable = GameObject.FindObjectOfType<AgentChoise>();
        _projectileParticleSystem= GetComponentInChildren<ParticleSystem>();

        // Game Manager Value for Slow
        _SlowRange = _GameManagerScript._SlowRangeGameManager;
        _slowdownSpeed = _GameManagerScript._SlowdownSpeedGameManager;

        // Game Manager Value for Shoot
        _ShootRange = _GameManagerScript._ShootRangeGameManager;
        _damageAmount = _GameManagerScript._DamageAmount;
        _fireRate = _GameManagerScript._FireRate;

        // Game Manager Value For Mouvement 
        _NavMeshAgent.speed = _GameManagerScript._SpeedAgent;
        _NavMeshAgent.angularSpeed = _GameManagerScript._AngularSpeedAgent;
        _NavMeshAgent.acceleration = _GameManagerScript._AccelerationAgent;


    }

    private void Update()
    {
        // change de range of the collider in fonction with booléen 
        if (_ShootEnemy)
        {
            _ColliderTrigger.radius = _ShootRange;
        }
        
        if (_SlowEnemy)
        {
            _ColliderTrigger.radius = _SlowRange;
        }

        time += Time.deltaTime;
       
    }



    private void OnTriggerEnter(Collider other)
    {
        // slow Enemy en trigger enter and fix it to _SlowSpeed
        if (_SlowEnemy && other.CompareTag("AgentMechant"))
        {
            NavMeshAgent enemyAgent = other.GetComponent<NavMeshAgent>();
            if (enemyAgent != null)
            {
                enemyAgent.speed = _slowdownSpeed;
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        // Shoot Enemy if the agent is used with _ShootEnemy active 
        if (_ShootEnemy && other.CompareTag("AgentMechant") && !IsAgentUsable(GetComponent<NavMeshAgent>()) && time >= _fireRate)
        {
            // Change the target Enemy
            currentTargetEnemy = other.gameObject;
            ShootToEnemy();

            time = 0;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // reset Agent Speed when exit the trigger when _SlowEnemy is true
        if (!_SlowEnemy && other.CompareTag("AgentMechant"))
        {
            NavMeshAgent enemyAgent = other.GetComponent<NavMeshAgent>();
            if (enemyAgent != null)
            {
                enemyAgent.speed = _InitialSpeed;
            }
        }

        // Reset the current target if enemy escape the trigger 
        if (other.gameObject == currentTargetEnemy)
        {
            currentTargetEnemy = null;
        }
    }


    // Shoot Fonction  | Called Line 93 OnTriggerStay
    public void ShootToEnemy()
    {
        RaycastHit hit;

        if (currentTargetEnemy != null)
        {
            // Chose Enemy direction
            Vector3 directionToEnemy = (currentTargetEnemy.transform.position - _BulletSpawnPosition.position).normalized;

            // Look AT enemy
            _gun.transform.LookAt(_BulletSpawnPosition.position + directionToEnemy);

            // Raycast to bulletSpawnPoint to enemy
            if (Physics.Raycast(_BulletSpawnPosition.position, directionToEnemy, out hit, _ShootRange, _BulletLayer))
            {
                // Instanciate Trail for feedback 
                TrailRenderer _Trail = Instantiate(_TrailBullet, _BulletSpawnPosition.position, Quaternion.identity);
                // Start Coroutine to lerp the position of the trail 
                StartCoroutine(SpawnTrail(_Trail, hit));


                // ------ All this part to damage the enemy ------
                HeathEnemy _EnemyHealth = hit.collider.GetComponent<HeathEnemy>();

                Debug.DrawRay(_BulletSpawnPosition.position, hit.point - _BulletSpawnPosition.position, Color.red, 1);

                if (_EnemyHealth != null)
                {
                    _EnemyHealth.TakeDamage(_damageAmount);

                    if (_EnemyHealth.GetCurrentHealth() <= 0)
                    {
                        _EnemyHealth.Die();
                        currentTargetEnemy = null; 
                    }
                }
                // ------- ------
            }
        }
    }

    // Lerp the trail position | Called Line 136 in the Shoot fonction 
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
        // This check if the agent are usable or not 
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
