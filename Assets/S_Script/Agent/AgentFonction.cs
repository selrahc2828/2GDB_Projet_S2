using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AgentFonction : MonoBehaviour
{
    [Header("Reference")]
    public AgentToTrace _AgentDispo;
    public GameManager _GameManagerScript;
    public SphereCollider _ColliderTrigger;
    public NavMeshAgent _NavMeshAgent;
    public RecognizeItsSelf _AgentSelfScript;

    private GameObject currentTargetEnemy;
    [SerializeField] private List<GameObject> _EnemiesInRange = new List<GameObject>();


    [Header("Layer")]
    public LayerMask _BulletLayer;

    [Header("RangeShoot And Slow")]
    //public float _SlowRange;
    public float _ShootRange;


    [Header("Choise Comportement")]
    public bool _ShootEnemy;
    public bool _SlowEnemy;

    //[Header("Slow Parameters")]
    //public float _slowdownSpeed;
    //public float _InitialSpeed;

    [Header("Weapon Reference")]
    public Transform _BulletSpawnPosition;
    public Transform _TowerPosition;

    [Header("Mine System")]
    public int _mineDamage;
    public bool _mineUsed;
    public float _mineTimer;
    public float _mineRadius;
    public Vector3 _mineLocation;
    public GameObject _minePrefab;

    [Header("Weapon Parameter")]
    public int _damageAmount;
    public int _initialDamageAmount;
    public float _initialFireRate;
    public float _fireRate;
    public float _exaustion;

    private float nextFireTime = 0f;


    [Header("Particle System & Trail Renderer")]
    public ParticleSystem _projectileParticleSystem;
    public TrailRenderer _TrailBullet;



    public void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
        _AgentSelfScript = this.GetComponentInParent<RecognizeItsSelf>();
    }


    private void Start()
    {

        // Get the script for all the agent in scene
        _AgentDispo = GameObject.FindObjectOfType<AgentToTrace>();
        _projectileParticleSystem = GetComponentInChildren<ParticleSystem>();

        //// Game Manager Value for Slow
        //_SlowRange = _GameManagerScript._SlowRangeGameManager;
        //_slowdownSpeed = _GameManagerScript._SlowdownSpeedGameManager;

        // Game Manager Value for Shoot
        _ShootRange = _GameManagerScript._ShootRangeGameManager;
        _initialDamageAmount = _GameManagerScript._DamageAmount;
        _initialFireRate = _GameManagerScript._FireRate;

        // Game Manager Value For Mouvement 
        _NavMeshAgent.speed = _GameManagerScript._SpeedAgent;
        _NavMeshAgent.angularSpeed = _GameManagerScript._AngularSpeedAgent;
        _NavMeshAgent.acceleration = _GameManagerScript._AccelerationAgent;

    }

    private void Update()
    {
        if (!_GameManagerScript._gameLose)
        {
            _exaustion = _AgentSelfScript._exaustionLevel;
            _fireRate = Mathf.Lerp(_initialFireRate * 1.1f, _initialFireRate * 0.3f, _exaustion);
            _damageAmount = (int)Mathf.Lerp(_initialDamageAmount * 1.1f, _initialDamageAmount * 0.3f, _exaustion);

            if (currentTargetEnemy != null && _ShootEnemy == true && !IsAgentUsable(GetComponent<NavMeshAgent>()) && Time.time >= nextFireTime && _AgentSelfScript._canShoot == true)
            {
                ShootToEnemy();
                nextFireTime = Time.time + (1f / _fireRate);
            }

            if (_AgentSelfScript._canShoot == true)
            {
                // Call A physics OverlapSphere to update list of agent 
                Collider[] colliders = Physics.OverlapSphere(transform.position, _ShootRange, LayerMask.GetMask("AgentMechant"));

                // Add agent in the overlaps Sphere
                foreach (Collider collider in colliders)
                {
                    GameObject enemy = collider.gameObject;
                    if (!_EnemiesInRange.Contains(enemy))
                    {

                        _EnemiesInRange.Add(enemy);
                        // Remove all missing component when an other enemy overlap
                        _EnemiesInRange.RemoveAll(item => item == null);

                        // if first enemys list set it to enemy to aim 
                        if (currentTargetEnemy == null)
                        {
                            currentTargetEnemy = enemy;
                        }
                    }
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {

        // Reset the current target if enemy escape the trigger 
        if (other.gameObject == currentTargetEnemy)
        {
            _EnemiesInRange.Remove(other.gameObject);

            // If enemy exit trigger change target 
            if (currentTargetEnemy == other.gameObject)
            {
                currentTargetEnemy = GetNextTarget();
            }
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

            if (Physics.Raycast(_BulletSpawnPosition.position, directionToEnemy, out hit, _ShootRange, _BulletLayer))
            {
                // Instanciate Trail for feedback 
                TrailRenderer _Trail = Instantiate(_TrailBullet, _BulletSpawnPosition.position, Quaternion.identity);
                // Start Coroutine to lerp the position of the trail 
                StartCoroutine(SpawnTrail(_Trail, hit));

                #region Damage And Effect To Enemies
                // ------ All this part to damage the enemy ------
                Enemy _Enemy = hit.collider.GetComponent<Enemy>();

                Debug.DrawRay(_BulletSpawnPosition.position, hit.point - _BulletSpawnPosition.position, Color.red, 1);

                if (_Enemy != null)
                {
                    if (_AgentSelfScript._pool2ProximityValue > -1)
                    {
                        _damageAmount += 2;
                    }
                    // inflic Damage 
                    _Enemy.TakeDamage(_damageAmount);

                    if (_AgentSelfScript._pool1ProximityValue > -1)
                    {
                        _Enemy.GetSlowed();
                    }

                    if (_Enemy.GetCurrentHealth() <= 0 && currentTargetEnemy == hit.collider.gameObject)
                    {
                        // kill the enemy if his current hp is <= to 0
                        _Enemy.Die();
                        _EnemiesInRange.Remove(currentTargetEnemy);
                    }
                }
                #endregion
            }
        }
    }


    // Get the next target
    private GameObject GetNextTarget()
    {
        // Return actual enemy
        if (_EnemiesInRange.Count > 0)
        {
            return _EnemiesInRange[0];
        }
        else
        {
            return null;
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
