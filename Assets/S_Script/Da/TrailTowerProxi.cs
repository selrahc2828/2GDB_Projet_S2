using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTowerProxi : MonoBehaviour
{
    public TrailRenderer TrailRenderer; 
    public List<Transform> agentTransforms; 
    public float speed = 5f; 

    private int currentPointIndex = 0; 

    void Start()
    {
        if (agentTransforms.Count > 0)
            transform.position = agentTransforms[0].position;
    }

    void Update()
    {
        MoveTrailAlongAgents();
    }

    void MoveTrailAlongAgents()
    {
        if (agentTransforms.Count == 0 || currentPointIndex >= agentTransforms.Count - 1)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPosition = agentTransforms[currentPointIndex + 1].position;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (transform.position == targetPosition)
            currentPointIndex++;
    }
}
