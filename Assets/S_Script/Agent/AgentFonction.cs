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
    public float _ShootRange;

    [Header("Choise Comportement")]
    public bool _ShootEnemy;
    public bool _SlowEnemy;

    [Header("Weapon Reference")]
    public Transform _BulletSpawnPosition;
    public Transform _TowerPosition;

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
    public Material _defaultTrailMaterial;
    public Material _poolDamageTrailMaterial;

    public bool _LinkToPoolDamage;

    public void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
        _AgentSelfScript = this.GetComponentInParent<RecognizeItsSelf>();
    }

    private void Start()
    {
        _AgentDispo = GameObject.FindObjectOfType<AgentToTrace>();
        _projectileParticleSystem = GetComponentInChildren<ParticleSystem>();

        _ShootRange = _GameManagerScript._ShootRangeGameManager;
        _initialDamageAmount = _GameManagerScript._DamageAmount;
        _initialFireRate = _GameManagerScript._FireRate;

        _NavMeshAgent.speed = _GameManagerScript._SpeedAgent;
        _NavMeshAgent.angularSpeed = _GameManagerScript._AngularSpeedAgent;
        _NavMeshAgent.acceleration = _GameManagerScript._AccelerationAgent;

        _LinkToPoolDamage = false;
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
        if (other.gameObject == currentTargetEnemy)
        {
            _EnemiesInRange.Remove(other.gameObject);

            if (currentTargetEnemy == other.gameObject)
            {
                currentTargetEnemy = GetNextTarget();
            }
        }
    }

    public void ShootToEnemy()
    {
        RaycastHit hit;

        if (currentTargetEnemy != null)
        {
            Vector3 directionToEnemy = (currentTargetEnemy.transform.position - _BulletSpawnPosition.position).normalized;

            if (Physics.Raycast(_BulletSpawnPosition.position, directionToEnemy, out hit, _ShootRange, _BulletLayer))
            {
                TrailRenderer _Trail = Instantiate(_TrailBullet, _BulletSpawnPosition.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(_Trail, hit));

                #region Damage And Effect To Enemies
                Enemy _Enemy = hit.collider.GetComponent<Enemy>();

                Debug.DrawRay(_BulletSpawnPosition.position, hit.point - _BulletSpawnPosition.position, Color.red, 1);

                if (_Enemy != null)
                {
                    if (_AgentSelfScript._pool4ProximityValue > -1)
                    {
                        _damageAmount += 2;
                        _LinkToPoolDamage = true;
                    }
                    if (_AgentSelfScript._pool2ProximityValue > -1)
                    {
                        _damageAmount += 2;
                        _LinkToPoolDamage = true;
                    }
                    if (_AgentSelfScript._pool3ProximityValue > -1)
                    {
                        _damageAmount += 2;
                        _LinkToPoolDamage = true;
                    }

                    if (_AgentSelfScript._pool4ProximityValue > -1 && _AgentSelfScript._pool2ProximityValue > -1 && _AgentSelfScript._pool3ProximityValue > -1)
                    {
                        _LinkToPoolDamage = false;
                    }

                    if (_LinkToPoolDamage)
                    {
                        _Trail.material = _poolDamageTrailMaterial;
                    }
                    else
                    {
                        _Trail.material = _defaultTrailMaterial;
                    }

                    _Enemy.TakeDamage(_damageAmount);

                    if (_AgentSelfScript._pool1ProximityValue > -1)
                    {
                        _Enemy.GetSlowed();
                    }

                    if (_Enemy.GetCurrentHealth() <= 0 && currentTargetEnemy == hit.collider.gameObject)
                    {
                        _Enemy.Die();
                        _EnemiesInRange.Remove(currentTargetEnemy);
                    }
                }
                #endregion
            }
        }
    }

    private GameObject GetNextTarget()
    {
        if (_EnemiesInRange.Count > 0)
        {
            return _EnemiesInRange[0];
        }
        else
        {
            return null;
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
