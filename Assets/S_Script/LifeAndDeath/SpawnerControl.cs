using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    public JaugeScript _JaugeScript;
    public GameObject prefabToSpawn; 
    public float spawnInterval = 2f; 

   
    void Start()
    {
        InvokeRepeating("SpawnPrefab", 0f, spawnInterval);
    }

    
    void SpawnPrefab()
    {
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }
}
