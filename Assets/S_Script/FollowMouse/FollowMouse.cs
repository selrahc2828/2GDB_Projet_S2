using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowMouse : MonoBehaviour
{

    public LayerMask _affectedLayer;
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
        {
            
            NavMeshAgent[] agents = FindObjectsOfType<NavMeshAgent>();

            foreach (NavMeshAgent agent in agents)
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
