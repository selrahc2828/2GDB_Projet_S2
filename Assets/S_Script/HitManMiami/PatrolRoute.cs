using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    public Patrol _PatrolScript;
    public Transform[] _WayPoint;

    void Start()
    {
        int childCount = transform.childCount;
        _WayPoint = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            _WayPoint[i] = transform.GetChild(i);
        }
    }


    private void OnDrawGizmos()
    {
        if (_WayPoint == null || _WayPoint.Length == 0)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < _WayPoint.Length; i++)
        {
            int nextIndex = (i + 1) % _WayPoint.Length; // Indice du prochain waypoint
            Gizmos.DrawLine(_WayPoint[i].position, _WayPoint[nextIndex].position);
        }
    }
}
