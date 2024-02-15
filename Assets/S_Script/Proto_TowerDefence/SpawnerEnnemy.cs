using UnityEngine;
using System.Collections;

public class SpawnerEnnemy : MonoBehaviour
{
    public GameObject[] _enemyPrefabs;
    public Transform[] _spawnPoints;
    public float _spawnCooldown = 2f;

    private bool canSpawn = true;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (canSpawn)
            {
                SpawnEnemy();
                canSpawn = false;
                yield return new WaitForSeconds(_spawnCooldown);
                canSpawn = true;
            }
            yield return null;
        }
    }

    void SpawnEnemy()
    {
        // Choisir un spawner aléatoire
        int randomIndex = Random.Range(0, _spawnPoints.Length);

        // Instantier un ennemi à partir d'un préfab aléatoire
        GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)];
        Instantiate(enemyPrefab, _spawnPoints[randomIndex].position, Quaternion.identity);
    }
}
