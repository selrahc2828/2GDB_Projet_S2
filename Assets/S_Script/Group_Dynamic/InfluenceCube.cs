using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfluenceCube : MonoBehaviour
{
    private float _capsuleRadius;
    private float _capsuleHeight;
    private Vector3 _closestPoint;

    void Start()
    {
        //Check if there are already agents in the triggerZone and change their destination if there is
        CheckSphereAtSpawn();

        //The size of detection is the same as the size of the triggerZone
        _capsuleRadius = GetComponent<CapsuleCollider>().radius;
        _capsuleHeight = GetComponent<CapsuleCollider>().height;

    }


    private void OnTriggerEnter(Collider other)
    {
        PullAgentTogether(other.gameObject);
    }

    void CheckSphereAtSpawn()
    {
        // Use Physics.OverlapBox to check for objects within the Box and put them in an array
        Collider[] colliders = Physics.OverlapCapsule(transform.position, transform.position + (Vector3.up * _capsuleHeight), _capsuleRadius);

        //If the array isn't empty
        if (colliders.Length > 0)
        {
            // Check every objects in the array
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<CapsuleCollider>())
                {
                    // Change their destination to be the Influence Tower
                    PullAgentTogether(collider.gameObject);
                }
            }
        }
        else
        {
            // No objects found within the Box
            Debug.Log("No objects within the Box.");
        }
    }

    void PullAgentTogether(GameObject _object)
    {

        _closestPoint = _object.GetComponent<CapsuleCollider>().ClosestPoint(_object.transform.position);
        Debug.Log(_closestPoint.sqrMagnitude);
        Debug.Log(_closestPoint);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_closestPoint, 5);
    }
}