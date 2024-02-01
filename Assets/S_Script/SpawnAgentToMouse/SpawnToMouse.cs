using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnToMouse : MonoBehaviour
{
    [Header("Référence")]
    public GameObject _AgentPrefab; 

    public LayerMask _affectedLayer;


    [Header("Parametter")]
    public float spawnInterval = 0.5f; 
    public float randomOffsetRange = 1.0f; 
    public float _AgentSpawnCount = 0;

    private float spawnTimer;
    private List<GameObject> spawnedAgents = new List<GameObject>();

    private void Update()
    {

        if (Input.GetMouseButton(0))
        {
            spawnTimer -= Time.deltaTime;

            while (spawnTimer < _AgentSpawnCount)
            {
                spawnTimer = spawnInterval;
                SpawnAgent();
            }

        }
    }

    void SpawnAgent()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
        {
            Vector3 spawnPosition = hit.point + Random.insideUnitSphere * randomOffsetRange;

            GameObject newAgent = Instantiate(_AgentPrefab, spawnPosition, Quaternion.identity);
            spawnedAgents.Add(newAgent);

        }
    }
}
