using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Niveau1 : MonoBehaviour
{
    public GameManager _gameManager;
    public SpawnerEnnemy _spawnerScript;
    public GameObject _poolSpots;
    public GameObject _pools;
    public List<Transform> _spotList;
    public List<GameObject> _poolList;
    public int _numberOfWave;
    public int _numberOfBasicEnnemy;
    public int _numberOfHomeWrecker;
    public int _numberOfBuzzKiller;
    public int _actualNumberOfEnemy;
    public int _currentWave;
    public int _displayedWave;
    public int _numberEnemyWave1;
    public int _numberEnemyWave2;
    public int _numberEnemyWave3;
    public int _numberEnemyWave4;
    public int _numberEnemyWave5;
    public int _timerBetweenWave;

    [Header("TextInfo")]
    public Text _TextNumberOfWave;

    private void Awake()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _spawnerScript = GameObject.FindObjectOfType<SpawnerEnnemy>();
    }

    private void Start()
    {
        foreach (Transform spotTransform in _poolSpots.transform)
        {
            _spotList.Add(spotTransform);
        }
        foreach (Transform poolTransform in _pools.transform)
        {
            _poolList.Add(poolTransform.gameObject);
        }
        _gameManager._gameLose = false;
        _gameManager._gameWin = false;
        _gameManager._numberOfEnemyOnScreen = 0;
        _gameManager._waveStarted = false;
        _currentWave = 0;
        _displayedWave = 0;
        _numberOfWave = 5;
        NextWave();
    }

    private void Update()
    {
        _TextNumberOfWave.text = "Wave : " + _displayedWave;
    }


    public void CallWaves(int numberOfEnemy, int numberofHomeWrecker, int numberofBuzzKiller)
    {
        if (_spawnerScript != null)
        {
            _spawnerScript.StartCoroutine(_spawnerScript.SpawnAWave(numberOfEnemy, numberofHomeWrecker, numberofBuzzKiller));
        }

        _gameManager._waveStarted = true;
    }

    public void NextWave()
    {
        _currentWave++;
        _displayedWave++;
        if (_currentWave / 5 == (int)(_currentWave % 5))
        {
            ChangePoolSpot();
        }
        switch (_currentWave)
        {
            case 1:
                _numberOfBasicEnnemy = _numberEnemyWave1;
                _numberOfHomeWrecker = 1;
                _numberOfBuzzKiller = 1;
                _gameManager._gameStarted = true;
                break;
            case 2:
                _numberOfBasicEnnemy = _numberEnemyWave2;
                _numberOfHomeWrecker = 1;
                _numberOfBuzzKiller = 0;
                break;
            case 3:
                _numberOfBasicEnnemy = _numberEnemyWave3;
                _numberOfHomeWrecker = 0;
                _numberOfBuzzKiller = 1;
                break;
            case 4:
                _numberOfBasicEnnemy = _numberEnemyWave4;
                _numberOfHomeWrecker = 1;
                _numberOfBuzzKiller = 1;
                break;
            case 5:
                _numberOfBasicEnnemy = _numberEnemyWave5;
                _numberOfHomeWrecker = 2;
                _numberOfBuzzKiller = 1;
                break;
            default:
                _numberOfBasicEnnemy = _numberEnemyWave5;
                _numberOfHomeWrecker = 2;
                _numberOfBuzzKiller = 2;
                break;
        }
        CallWaves(_numberOfBasicEnnemy, _numberOfHomeWrecker, _numberOfBuzzKiller);
    }

    public void ChangePoolSpot()
    {
        foreach(GameObject _poolGameObject in _poolList)
        {
            int SpotNumber = Random.Range(0, _spotList.Count);
            _poolGameObject.transform.position = _spotList[SpotNumber].position;
        }
    }

    public void CheckForNewtWave()
    {
        if (_gameManager._numberOfEnemyOnScreen <= 0 && !_gameManager._gameLose)
        {
            NextWave();
        }
    }
}
