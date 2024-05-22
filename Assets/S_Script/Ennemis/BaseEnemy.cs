using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : Enemy
{
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
        _GameManager._numberOfBaseEnemyOnScreen++;

        _MaxHealth = _GameManager._HeathBaseEnemy;
        _CurrentHealth = _MaxHealth;

        _slowPower = _GameManager._slowPower;
        _slowDuration = _GameManager._slowDuration;

        _thisAgentBaseSpeed = _GameManager._SpeedBaseEnemy;
        _thisAgent.speed = _GameManager._SpeedBaseEnemy;
        _thisAgent.acceleration = _GameManager._AccelerationBaseEnemy;
        _thisAgent.angularSpeed = _GameManager._AngularSpeedBaseEnemy;

        _thisAgent.SetDestination(_TowerToDestroy.position);
    }
    private void OnDestroy()
    {
        _GameManager._numberOfBaseEnemyOnScreen--;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColorOnHP();
        ApplySlow();
    }
}
