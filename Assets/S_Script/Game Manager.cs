using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [Header("Slow Agent Parameter")]
    public float _SlowRangeGameManager;
    public float _SlowdownSpeedGameManager;

    [Header("Shoot Agent Parameter")]
    public float _ShootRangeGameManager;
    public int _DamageAmount;
    public float _FireRate;
    public float _maxFatigueSeconde;

    [Header("Agent Mouvement Parameter")]
    public float _SpeedAgent;
    public float _AngularSpeedAgent;
    public float _AccelerationAgent;
    public float _resetTime;//pour le reset apres un timer (il n'est pas actif actuellement)

    [Header("Agent Health")]
    public int _HealthAgent;


    [Header("Enemy Heath Parameter")]
    public int _HeathBaseEnemy;
    public int _HeathHomeWreaker;
    public int _HeathBuzzKiller;


    [Header("Enemy Mouvement Parameter")]
    public float _SpeedBaseEnemy;
    public float _SpeedHomeWreaker;
    public float _SpeedBuzzKiller;

    public float _AngularSpeedBaseEnemy;
    public float _AngularSpeedHomeWreaker;
    public float _AngularSpeedBuzzKiller;

    public float _AccelerationBaseEnemy;
    public float _AccelerationHomeWreaker;
    public float _AccelerationBuzzKiller;

    public int _tailleSegmentZigZagHomeWrecker;
    public int _amplitudeZigZagHomeWrecker;

    [Header("Enemy damage")]
    public int _DamageAmoutToAgent;

    [Header("Slow System")]
    public float _slowDuration;
    public float _slowPower;

    [Header("Mine System")]
    public int _mineDamage;
    public float _mineTimer;
    public float _mineRadius;
    public GameObject _minePrefab;

    [Header("Tower Heath & Parameter")]
    public int _HeathTower;


    [Header("UtilityVariable")]
    public Niveau1 _LevelWaveCheck;
    public int _NumberOfEnemyKilled = 0;
    public TextMeshProUGUI _NumberOfEnemyText;
    public TextMeshProUGUI _TotalWave;
    public TextMeshProUGUI _TotalTime;
    private float _Time;


    [Header("Game Parameter")]
    public float _slowMo;
    public bool _gamePaused;
    public bool _gameLose;
    public bool _gameWin;
    public bool _gameStarted;
    public bool _waveStarted;
    public int _numberOfEnemyOnScreen;
    public GameObject _gameLoseCanevas;
    public GameObject _gameWinCanevas;
    public float _timeBetweenSegment;




    private void Update()
    {
        CheckIfGameIsLoseOrWin();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Time.timeScale = 5f;
        }

        if (!_gameLose)
        {
            _Time += Time.deltaTime;
        }

        _TotalTime.text = "Total time : " + Mathf.Floor(_Time / 60f).ToString("00") + " : " + (_Time % 60).ToString("00");
        _TotalWave.text = "Total Wave : " + _LevelWaveCheck._displayedWave.ToString();

    }

    public void IncreaseEnemyKilledCount()
    {
        _NumberOfEnemyKilled++;
        UpdateEnemyKilledText(); 
    }

    private void UpdateEnemyKilledText()
    {
        if (_NumberOfEnemyText != null)
        {
            _NumberOfEnemyText.text = "Total Enemy Kill :  " + _NumberOfEnemyKilled.ToString();
        }
    }


    public void CheckIfGameIsLoseOrWin()
    {
        if (_gameLose)
        {
            _gameLoseCanevas.SetActive(true);
            DestroyAllEnemies();
        }
        else if (_gameWin) 
        {
            _gameWinCanevas.SetActive(true);
        }
    }


    private void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("AgentMechant");

        foreach (GameObject enemy in enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.Die();
            }
        }
    }


}
