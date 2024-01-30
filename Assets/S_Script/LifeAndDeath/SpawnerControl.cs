using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    public GameObject prefabToSpawn; // Référence au prefab à faire spawn
    public float spawnInterval = 2f; // Intervalle de spawn en secondes

    // Start is called before the first frame update
    void Start()
    {
        // Commencer à faire spawn le prefab à l'intervalle spécifié
        InvokeRepeating("SpawnPrefab", 0f, spawnInterval);
    }

    // Méthode pour faire spawn le prefab
    void SpawnPrefab()
    {
        // Faire spawn le prefab à la position du spawner avec l'identité de rotation
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }
}
