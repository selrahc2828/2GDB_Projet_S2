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
    public int _actualNumberOfEnemy;
    public int _currentWave;
    public int _displayedWave;
    public int _timerBetweenWave;
    public float _timerBetweenSegment;
    public bool _startSpawning = false;
    public float _timerForSpawn;
    private int wavesSinceLastChange = 0;

    private Dictionary<GameObject, Vector3> _initialScales;

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
        _initialScales = new Dictionary<GameObject, Vector3>();

        foreach (Transform spotTransform in _poolSpots.transform)
        {
            _spotList.Add(spotTransform);
        }
        foreach (Transform poolTransform in _pools.transform)
        {
            var poolGameObject = poolTransform.gameObject;
            _poolList.Add(poolGameObject);
            _initialScales[poolGameObject] = poolGameObject.transform.localScale;
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
        ChangePoolSpot();
    }

    private void Update()
    {
        CheckForNextWave();
        _TextNumberOfWave.text = "Wave : " + _displayedWave;
        if (_startSpawning)
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
        if (_timerForSpawn > _timerBetweenSegment * 2 && _CSpawned == false)
        {
            _CSpawned = true;
            SpawnSegmentC();
        }
        if (_timerForSpawn > _timerBetweenSegment * 3 && _DSpawned == false)
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
        if (_currentWave >= 5)
        {
            _currentWave = 1;
        }
        _displayedWave++;

        wavesSinceLastChange++;
        if (wavesSinceLastChange >= 1)
        {
            ChangePoolSpot();
            wavesSinceLastChange = 0;
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
        List<Transform> availableSpots = new List<Transform>(_spotList);

        foreach (GameObject poolGameObject in _poolList)
        {
            StartCoroutine(ScaleDownAndChangePosition(poolGameObject, availableSpots)); 
        }
    }

    private IEnumerator ScaleDownAndChangePosition(GameObject pool, List<Transform> availableSpots)
    {
        yield return StartCoroutine(ScaleDownPool(pool));
        int SpotNumber = GetUnusedSpotIndex(availableSpots);

        if (SpotNumber != -1)
        {
            pool.transform.position = availableSpots[SpotNumber].position;
            availableSpots.RemoveAt(SpotNumber); 
        }

        StartCoroutine(ScaleUpPool(pool));
    }

    private int GetUnusedSpotIndex(List<Transform> availableSpots)
    {
        if (availableSpots.Count == 0)
        {
            return -1; 
        }

        int SpotNumber = Random.Range(0, availableSpots.Count);
        return SpotNumber;
    }

    private IEnumerator ScaleDownPool(GameObject pool)
    {
        Vector3 initialScale = pool.transform.localScale;
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            pool.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pool.transform.localScale = Vector3.zero;
    }

    private IEnumerator ScaleUpPool(GameObject pool)
    {
        Vector3 initialScale;
        if (!_initialScales.TryGetValue(pool, out initialScale))
        {
            initialScale = new Vector3(1, 1, 1);
        }
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            pool.transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pool.transform.localScale = initialScale;
    }

    public void CheckForNextWave()
    {
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
