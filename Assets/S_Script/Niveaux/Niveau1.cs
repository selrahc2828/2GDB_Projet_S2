using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Niveau1 : MonoBehaviour
{
    public GameManager _gameManager;
    public SpawnerEnnemy _spawnerScript;
    public int _numberOfWave;
    public int _numberOfEnemyToCall;
    public int _actualNumberOfEnemy;
    public int _currentWave;
    public int _numberEnemyWave1;
    public int _numberEnemyWave2;
    public int _numberEnemyWave3;
    public int _numberEnemyWave4;
    public int _numberEnemyWave5;
    public int _timerBetweenWave;

    private void Awake()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _spawnerScript = GameObject.FindObjectOfType<SpawnerEnnemy>();
    }

    private void Start()
    {
        _gameManager._gameLose = false;
        _gameManager._gameWin = false;
        _gameManager._numberOfEnemyOnScreen = 0;
        _gameManager._waveStarted = false;
        _currentWave = 0;
        _numberOfWave = 5;
        NextWave();
    }

    private void Update()
    {

    }
    public void CallWaves(int numberOfEnemy)
    {
        _spawnerScript.StartCoroutine(_spawnerScript.SpawnAWave(numberOfEnemy));
        _gameManager._waveStarted = true;
    }

    public void NextWave()
    {
        _currentWave++;
        switch (_currentWave)
        {
            case 1:
                _numberOfEnemyToCall = _numberEnemyWave1;
                _gameManager._gameStarted = true;
                break;
            case 2:
                _numberOfEnemyToCall = _numberEnemyWave2;
                break;
            case 3:
                _numberOfEnemyToCall = _numberEnemyWave3;
                break;
            case 4:
                _numberOfEnemyToCall = _numberEnemyWave4;
                break;
            case 5:
                _numberOfEnemyToCall = _numberEnemyWave5;
                break;
            default:
                _numberOfEnemyToCall = _numberEnemyWave5;
                break;
        }
        CallWaves(_numberOfEnemyToCall);
    }

    public void CheckForNewtWave()
    {
        if (_gameManager._numberOfEnemyOnScreen <= 0 && !_gameManager._gameLose)
        {
            NextWave();
        }
    }
}
