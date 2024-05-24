using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
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
    public int _numberOfBaseEnemyOnScreen;
    public int _numberOfBuzzKillerOnScreen;
    public int _numberOfHomeWreckerOnScreen;
    public GameObject _gameLoseCanevas;
    public GameObject _gameWinCanevas;
    public float _timeBetweenSegment;


    [Header("FMOD")]
    private FMOD.Studio.Bus masterBus;
    public UnityEvent BuzzKillerPresent;
    public bool _signalBuzzPresentSent;
    public UnityEvent BuzzKillerGone;
    public bool _signalBuzzGoneSent;
    public UnityEvent HomeWreckerPresent;
    public bool _signalHomePresentSent;
    public UnityEvent HomeWreckerGone;
    public bool _signalHomeGoneSent;


    private void Start()
    {
        Time.timeScale = 1.0f;
        masterBus = RuntimeManager.GetBus("bus:/");
    }


    private void Update()
    {
        CheckIfGameIsLoseOrWin();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Time.timeScale = 5f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DestroyAllEnemies();//cheat
        }
        if (!_gameLose)
        {
            _Time += Time.deltaTime;
        }

        _TotalTime.text = "Total time : " + Mathf.Floor(_Time / 60f).ToString("00") + " : " + (_Time % 60).ToString("00");
        _TotalWave.text = "Total Wave : " + _LevelWaveCheck._displayedWave.ToString();

        if(_numberOfBuzzKillerOnScreen > 0 && _signalBuzzPresentSent == false)
        {
            _signalBuzzPresentSent = true;
            _signalBuzzGoneSent = false;
            BuzzKillerPresent.Invoke();
        }
        else if ( _numberOfEnemyOnScreen <= 0 && _signalBuzzGoneSent == false)
        {
            _signalBuzzPresentSent = false;
            _signalBuzzGoneSent = true;
            BuzzKillerGone.Invoke();
        }
        
        if(_numberOfHomeWreckerOnScreen > 0 && _signalHomePresentSent == false)
        {
            _signalHomePresentSent = true;
            _signalHomeGoneSent = false;
            HomeWreckerPresent.Invoke();
        }
        else if (_numberOfHomeWreckerOnScreen <= 0 && _signalHomeGoneSent == false)
        {
            _signalHomePresentSent = false;
            _signalHomeGoneSent = true;
            HomeWreckerGone.Invoke();
        }
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
            masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
