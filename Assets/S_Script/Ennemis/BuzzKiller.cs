using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BuzzKiller : Enemy
{
    public PorteurDePool _porteurDePool;
    public List<GameObject> _poolList;
    public float _closestPoolDistance;
    public GameObject _closestPool;
    public UnityEvent CheckInfectionSurrounding;

    public void Awake()
    {
        _porteurDePool = FindAnyObjectByType<PorteurDePool>();
        _Niveau1 = FindAnyObjectByType<Niveau1>();
        _GameManager = FindAnyObjectByType<GameManager>();
        _thisAgent = this.GetComponentInParent<NavMeshAgent>();
        _FeedbackScript = FindAnyObjectByType<FeedBack>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in _porteurDePool.transform)
        {
            _poolList.Add(t.gameObject);
        }

        _GameManager._numberOfEnemyOnScreen++;
        _GameManager._numberOfBuzzKillerOnScreen++;

        _MaxHealth = _GameManager._HeathBuzzKiller;
        _CurrentHealth = _MaxHealth;

        _slowPower = _GameManager._slowPower;
        _slowDuration = _GameManager._slowDuration;

        _thisAgentBaseSpeed = _GameManager._SpeedBuzzKiller;
        _thisAgent.speed = _GameManager._SpeedBuzzKiller;
        _thisAgent.acceleration = _GameManager._AccelerationBuzzKiller;
        _thisAgent.angularSpeed = _GameManager._AngularSpeedBuzzKiller;

        _closestPoolDistance = float.MaxValue;
        SeekPool();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColorOnHP();
        ApplySlow(); 
    }

    public void SeekPool()
    {
        if(_poolList.Count > 0)
        {
            foreach(GameObject pool in _poolList)
            {
                if(Vector3.Distance(transform.position, pool.transform.position) < _closestPoolDistance)
                {
                    _closestPoolDistance = Vector3.Distance(transform.position, pool.transform.position);
                    _closestPool = pool;
                }
            }
            _thisAgent.SetDestination(_closestPool.GetComponent<CapsuleCollider>().ClosestPoint(transform.position));
        }
    }
    private void OnDestroy()
    {
        _GameManager._numberOfBuzzKillerOnScreen--;
        _GameManager._numberOfEnemyOnScreen--;
        _Niveau1.CheckForNextWave();
    }
    public override void Die()
    {
        _closestPool.GetComponent<PoolScript>().CheckSurrounding();
        CheckInfectionSurrounding.Invoke();
        base.Die();
    }
}
