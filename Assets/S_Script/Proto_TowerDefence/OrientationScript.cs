using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationScript : MonoBehaviour
{
    public Transform _towerTransform;

    void Update()
    {
        if (_towerTransform != null)
        {
            Vector3 oppositeDirection = transform.position - _towerTransform.position;
            oppositeDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(oppositeDirection);
        }
    }
}
