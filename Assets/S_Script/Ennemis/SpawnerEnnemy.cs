using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

public class SpawnerEnnemy : MonoBehaviour
{
    [Header("Reference")]
    public GameManager _GameManagerScript;

    public GameObject[] _baseEnemyPrefabs;
    public GameObject[] _homeWreckerPrefabs;
    public GameObject[] _buzzKillerPrefabs;
    public GameObject[] _PaternPrefab;
    public Vector3 _spawnPoint;
    public float _spawnCooldown = 0.15f;

    private bool _canSpawn = true;

    public GameObject[] _pools;


    public void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }


    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // if canSpawn
            if (_canSpawn)
            {
                // Call Spawn Enemy
                SpawnBaseEnemy();
                // change bool
                _canSpawn = false;
                // Return to have a cooldown
                yield return new WaitForSeconds(_spawnCooldown);
                _canSpawn = true;
            }
            yield return null;
        }
    }
    public void SpawnAWaveSegment(GameObject _segmentX)
    {
        Instantiate(_segmentX, Vector3.zero, Quaternion.identity);
    }

    public void SpawnPattern()
    {
        _spawnPoint = GenerateRandomVector() * 90;
        // instantiate the enemy to the spawn position 
        GameObject PaternPrefab = _PaternPrefab[Random.Range(0, _PaternPrefab.Length)];
        Vector3 directionToOrigin = (Vector3.zero - _spawnPoint).normalized;
        Instantiate(PaternPrefab, _spawnPoint, Quaternion.LookRotation(directionToOrigin));
    }

    public void SpawnBaseEnemy()
    {
        _spawnPoint = GenerateRandomVector() * 90;
        // instantiate the enemy to the spawn position 
        GameObject enemyPrefab = _baseEnemyPrefabs[Random.Range(0, _baseEnemyPrefabs.Length)];
        Instantiate(enemyPrefab, _spawnPoint, Quaternion.identity);
    }

    public void SpawnHomeWrecker()
    {
        _spawnPoint = GenerateRandomVector() * 90;
        // instantiate the enemy to the spawn position 
        GameObject enemyPrefab = _homeWreckerPrefabs[Random.Range(0, _homeWreckerPrefabs.Length)];
        Instantiate(enemyPrefab, _spawnPoint, Quaternion.identity);
    }

    public void SpawnBuzzKiller()
    {
        _spawnPoint = GenerateRandomVector() * 90;
        // instantiate the enemy to the spawn position 
        GameObject enemyPrefab = _buzzKillerPrefabs[Random.Range(0, _buzzKillerPrefabs.Length)];
        Instantiate(enemyPrefab, _spawnPoint, Quaternion.identity);
    }

    public IEnumerator SpawnAWave(int numberOfBaseEnemy, int numberOfHomeBrecker, int numberOfBuzzKiller)
    {
        ////
        ////
        //// A FINIR
        ////
        ////
        for (int i = 0; i < numberOfBaseEnemy; i++)
        {
            yield return new WaitForSeconds(_spawnCooldown);
            SpawnBaseEnemy();
        } 
        
        for (int i = 0; i < numberOfHomeBrecker; i++)
        {
            yield return new WaitForSeconds(_spawnCooldown);
            SpawnHomeWrecker();
        } 
        
        for (int i = 0; i < numberOfBuzzKiller; i++)
        {
            yield return new WaitForSeconds(_spawnCooldown);
            SpawnBuzzKiller();
        }
    }

    public Vector3 GenerateRandomVector()
    {
        // Generate random x and z coordinates
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        // Set y coordinate to 0
        float y = 0f;

        // Create a Vector3 with the generated coordinates
        Vector3 randomVector = new Vector3(randomX, y, randomZ);

        // Normalize the Vector3 to have a magnitude of 1
        randomVector.Normalize();

        return randomVector;
    }
}
