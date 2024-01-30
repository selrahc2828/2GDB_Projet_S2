using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    public GameObject prefabToSpawn; // R�f�rence au prefab � faire spawn
    public float spawnInterval = 2f; // Intervalle de spawn en secondes

    // Start is called before the first frame update
    void Start()
    {
        // Commencer � faire spawn le prefab � l'intervalle sp�cifi�
        InvokeRepeating("SpawnPrefab", 0f, spawnInterval);
    }

    // M�thode pour faire spawn le prefab
    void SpawnPrefab()
    {
        // Faire spawn le prefab � la position du spawner avec l'identit� de rotation
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }
}
