using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("Slow Agent Parameter")]
    public float _SlowRangeGameManager;
    public float _SlowdownSpeedGameManager;


    [Header("Shoot Agent Parameter")]
    public float _ShootRangeGameManager;
    public int _DamageAmount;
    public float _FireRate;
    public float _maxFatigue;
    

    [Header("Agent Mouvement Parameter")]
    public float _SpeedAgent;
    public float _AngularSpeedAgent;
    public float _AccelerationAgent;
    public float _resetTime;//pour le reset apres un timer (il n'est pas actif actuellement)


    [Header("Enemy Heath Parameter")]
    public int _HeathEnemy;


    [Header("SpawnEnemy")]
    public float _SpawnRate;

    [Header("Enemy Mouvement Parameter")]
    public float _SpeedEnemy;
    public float _AngularSpeedEnemy;
    public float _AccelerationEnemy;


    [Header("Tower Heath & Parameter")]
    public int _HeathTower;


    [Header("Game Parameter")]
    public float _slowMo;

}
