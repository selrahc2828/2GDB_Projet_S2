using UnityEngine;
using System.Collections;

public class SpawnerEnnemy : MonoBehaviour
{
    [Header("Reference")]
    public GameManager _GameManagerScript;

    public GameObject[] _enemyPrefabs;
    public Vector3 _spawnPoint;
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
        _spawnPoint = GenerateRandomVector() * 90;
        // instantiate the enemy to the spawn position 
        GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
        Instantiate(enemyPrefab, _spawnPoint, Quaternion.identity);
    }

    public IEnumerator SpawnAWave(int numberOfEnemy)
    {
        for (int i = 0; i < numberOfEnemy; i++)
        {
            yield return new WaitForSeconds(0.15f);
            SpawnEnemy();
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
