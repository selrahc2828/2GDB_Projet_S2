using UnityEngine;
using System.Collections;

public class SpawnerEnnemy : MonoBehaviour
{
    [Header("Reference")]
    public GameManager _GameManagerScript;

    public GameObject[] _enemyPrefabs;
    public Transform[] _spawnPoints;
    public float _spawnCooldown = 2f;

    private bool canSpawn = true;


    public void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }


    void Start()
    {
       

        _spawnCooldown = _GameManagerScript._SpawnRate;

        // Call the IEnumerator
        //StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // if canSpawn
            if (canSpawn)
            {
                // Call Spawn Enemy
                SpawnEnemy();
                // change bool
                canSpawn = false;
                // Return to have a cooldown
                yield return new WaitForSeconds(_spawnCooldown);
                canSpawn = true;
            }
            yield return null;
        }
    }

    void SpawnEnemy()
    {
        // Spawn to a random SpawnPoint
        int randomIndex = Random.Range(0, _spawnPoints.Length);

        // instantiate the enemy to the spawn position 
        GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
        Instantiate(enemyPrefab, _spawnPoints[randomIndex].position, Quaternion.identity);
    }

    public void SpawnAWave(int numberOfEnemy)
    {
        for(int i = 0; i < numberOfEnemy; i++)
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
            Instantiate(enemyPrefab, _spawnPoints[randomIndex].position, Quaternion.identity);
        }
    }
}
