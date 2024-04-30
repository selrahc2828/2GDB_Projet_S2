using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuzzKiller : Enemy
{
    public GameObject[] _pools;
    public float _closestPoolDistance;
    public GameObject _closestPool;

    public void Awake()
    {
        _Niveau1 = FindAnyObjectByType<Niveau1>();
        _GameManager = FindAnyObjectByType<GameManager>();
        _thisAgent = this.GetComponentInParent<NavMeshAgent>();
        _FeedbackScript = FindAnyObjectByType<FeedBack>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _GameManager._numberOfEnemyOnScreen++;

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
        if(_pools.Length > 0)
        {
            foreach(GameObject pool in _pools)
            {
                if(Vector3.Distance(transform.position, pool.transform.position) < _closestPoolDistance)
                {
                    _closestPoolDistance = Vector3.Distance(transform.position, pool.transform.position);
                    _closestPool = pool;
                }
            }
            _thisAgent.SetDestination(_closestPool.transform.position);
        }
    }

    public override void Die()
    {
        _closestPool.GetComponent<PoolScript>().CheckSurrounding();
        base.Die();
    }
}
