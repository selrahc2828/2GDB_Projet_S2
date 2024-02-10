using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public int _id;
    public PatrolRoute _PatrolScript;
    public NavMeshAgent _Agent;
    private int closestWaypointId;

    public bool _Inverse;

    private float _AgentDestination;

    private void Start()
    {
        _Agent = GetComponent<NavMeshAgent>();


        closestWaypointId = GetClosestWaypointId(transform.position);
        if (closestWaypointId != -1 && closestWaypointId < _PatrolScript._WayPoint.Length)
        {
            _Agent.SetDestination(_PatrolScript._WayPoint[closestWaypointId].position);
        }
    }


    private void Update()
    {
        _AgentDestination = Vector3.Distance(_Agent.destination, _Agent.transform.position);

        Debug.DrawLine(transform.position, _Agent.destination, Color.red);

        if (_AgentDestination <= 1)
        {
            int currentWaypointId = GetClosestWaypointId(_Agent.destination);
            int nextWaypointId;

            if (!_Inverse)
            {
                nextWaypointId = (currentWaypointId + 1) % _PatrolScript._WayPoint.Length;
            }
            else
            {
                nextWaypointId = (currentWaypointId - 1 + _PatrolScript._WayPoint.Length) % _PatrolScript._WayPoint.Length;
            }

            _Agent.SetDestination(_PatrolScript._WayPoint[nextWaypointId].position);
        }

    }


    private int GetClosestWaypointId(Vector3 position)
    {
        float closestDistance = Mathf.Infinity;
        int closestWayPointId = -1;
        for (var i = 0; i < _PatrolScript._WayPoint.Length; i++)
        {
            float dist = Vector3.Distance(position, _PatrolScript._WayPoint[i].position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestWayPointId = i;
            }
        }
        return closestWayPointId;
    }

}
