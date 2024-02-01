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
    public float _spawnInterval = 0.5f; 
    public float _randomOffsetRange = 1.0f; 
    public float _AgentSpawnCount = 0;

    private float spawnTimer;
    private List<GameObject> spawnedAgents = new List<GameObject>();

    private void Update()
    {
        // While the input is press do the thing in the IF 
        if (Input.GetMouseButton(0))
        {
            // Timer to limit de number of spawn, here we juste add a sec to DeltaTime
            spawnTimer -= Time.deltaTime;

            // While SpawnTimer < _AgentSpawnCount, that spawn more thant 1 agent on the time that spawnTimer is below the _AgentSpawnCount,, Reduc the value _AgentSpawnCount to have less spawn in sec 
            while (spawnTimer < _AgentSpawnCount)
            {
                // Update the timer
                spawnTimer = _spawnInterval;
                // Call the fonction that spawn agent 
                SpawnAgent();
            }

        }
    }

    // Fonction that spawn the agent 
    void SpawnAgent()
    {
        // Raycast Shoot camera to the layer floor where the mouse is 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // if the raycast hit the layer floor that call the fonction In 
        if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
        {
            // Set the position with a little offset to be more organic
            Vector3 spawnPosition = hit.point + Random.insideUnitSphere * _randomOffsetRange;

            // Create new agent at the mouse position with the offset
            GameObject newAgent = Instantiate(_AgentPrefab, spawnPosition, Quaternion.identity);
            // Add the Spawned agent to the list (can be usefull for furture)
            spawnedAgents.Add(newAgent);

        }
    }
}
