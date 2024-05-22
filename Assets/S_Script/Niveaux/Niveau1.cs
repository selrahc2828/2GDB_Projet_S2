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
    public float _timerBetweenSegment;
    public bool _startSpawning = false;
    public float _timerForSpawn;

    [Header("WavePrefab")]
    public GameObject _wave_1_A;
    public GameObject _wave_1_B;
    public GameObject _wave_1_C;
    public GameObject _wave_1_D;
    public GameObject _wave_2_A;
    public GameObject _wave_2_B;
    public GameObject _wave_2_C;
    public GameObject _wave_2_D;
    public GameObject _wave_3_A;
    public GameObject _wave_3_B;
    public GameObject _wave_3_C;
    public GameObject _wave_3_D;
    public GameObject _wave_4_A;
    public GameObject _wave_4_B;
    public GameObject _wave_4_C;
    public GameObject _wave_4_D;
    public GameObject _segmentA;
    public bool _ASpawned = false;
    public GameObject _segmentB;
    public bool _BSpawned = false;
    public GameObject _segmentC;
    public bool _CSpawned = false;
    public GameObject _segmentD;
    public bool _DSpawned = false;

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
        _timerBetweenSegment = _gameManager._timeBetweenSegment; 
        _startSpawning = false;
        _ASpawned = false;
        _BSpawned = false;
        _CSpawned = false;
        _DSpawned = false;
        _currentWave = 0;
        _displayedWave = 0;
        _numberOfWave = 5;
        _timerForSpawn = 0;
        NextWave();
    }

    private void Update()
    {
        CheckForNextWave();
        _TextNumberOfWave.text = "Wave : " + _displayedWave;
        if(_startSpawning)
        {
            _timerForSpawn += Time.deltaTime;
            SpawnTheWave();
        }
    }

    public void SpawnTheWave()
    {
        if (_ASpawned == false) 
        {
            _ASpawned = true;
            SpawnSegmentA();
        }
        if (_timerForSpawn > _timerBetweenSegment && _BSpawned == false) 
        {
            _BSpawned = true;
            SpawnSegmentB();
        }
        if (_timerForSpawn > _timerBetweenSegment*2 && _CSpawned == false) 
        {
            _CSpawned = true;
            SpawnSegmentC();
        }
        if (_timerForSpawn > _timerBetweenSegment*3 && _DSpawned == false)
        {
            _DSpawned = true;
            SpawnSegmentD();
            _startSpawning = false;
            _timerForSpawn = 0;
        }
    }

    public void SpawnSegmentA()
    {
        _spawnerScript.SpawnAWaveSegment(_segmentA);
    }
    public void SpawnSegmentB()
    {
        _spawnerScript.SpawnAWaveSegment(_segmentB);
    }
    public void SpawnSegmentC()
    {
        _spawnerScript.SpawnAWaveSegment(_segmentC);
    }
    public void SpawnSegmentD()
    {
        _spawnerScript.SpawnAWaveSegment(_segmentD);
    }

    public void NextWave()
    {
        _currentWave++;
        if(_currentWave >= 5)
        {
            _currentWave = 1;
        }
        _displayedWave++;
        if (_displayedWave / 5 == (int)(_displayedWave % 5))
        {
            ChangePoolSpot();
        }
        switch (_currentWave)
        {
            case 1:
                _segmentA = _wave_1_A;
                _segmentB = _wave_1_B;
                _segmentC = _wave_1_C;
                _segmentD = _wave_1_D;
                _gameManager._gameStarted = true;
                _gameManager._waveStarted = true;
                break;
            case 2:
                _segmentA = _wave_2_A;
                _segmentB = _wave_2_B;
                _segmentC = _wave_2_C;
                _segmentD = _wave_2_D;
                break;
            case 3:
                _segmentA = _wave_3_A;
                _segmentB = _wave_3_B;
                _segmentC = _wave_3_C;
                _segmentD = _wave_3_D;
                break;
            case 4:
                _segmentA = _wave_4_A;
                _segmentB = _wave_4_B;
                _segmentC = _wave_4_C;
                _segmentD = _wave_4_D;
                break;
        }
        _startSpawning = true;
    }

    public void ChangePoolSpot()
    {
        foreach(GameObject _poolGameObject in _poolList)
        {
            int SpotNumber = Random.Range(0, _spotList.Count);
            _poolGameObject.transform.position = _spotList[SpotNumber].position;
        }
    }

    public void CheckForNextWave()
    {
        /*
        Debug.Log(_gameManager._numberOfEnemyOnScreen);
        Debug.Log("L"+_gameManager._gameLose);
        Debug.Log("D"+_DSpawned);*/
        if (_gameManager._numberOfEnemyOnScreen <= 0 && !_gameManager._gameLose && _DSpawned == true)
        {
            _ASpawned = false;
            _BSpawned = false;
            _CSpawned = false;
            _DSpawned = false;
            NextWave();
        }
    }
}
