using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuzzKiller : Enemy
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

        _MaxHealth = _GameManager._HeathBuzzKiller;
        _CurrentHealth = _MaxHealth;

        _slowPower = _GameManager._slowPower;
        _slowDuration = _GameManager._slowDuration;

        _thisAgentBaseSpeed = _GameManager._SpeedBaseEnemy;
        _thisAgent.speed = _GameManager._SpeedBaseEnemy;
        _thisAgent.acceleration = _GameManager._AccelerationBaseEnemy;
        _thisAgent.angularSpeed = _GameManager._AngularSpeedBaseEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColorOnHP();
        ApplySlow();
    }
}
