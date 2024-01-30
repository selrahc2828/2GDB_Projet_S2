using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfluenceZone2 : MonoBehaviour
{
    private Vector3 _boxSize;

    [Header("BoolToMovement")]
    public bool _PushAndPull;
    public bool _LifeAndDeath;
    public bool _GregAndMaggie;

    void Start()
    {
        //Check if there are already agents in the triggerZone and change their destination if there is
        if (_PushAndPull)
        {
            PushObjectToPosition();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //If the object that trigger is tagged "Agent"
        if (other.CompareTag("Agent") && _PushAndPull == true)
        {
            Vector3 pushDirection = other.transform.position - transform.position;

            // Change the agent's destination to move away from the trigger
            other.GetComponent<NavMeshAgent>().SetDestination(other.transform.position + pushDirection);
        }
    }

    void PushObjectToPosition()
    {
        //The size of detection is the same as the size of the triggerZone
        _boxSize = GetComponent<BoxCollider>().size;

        // Use Physics.OverlapBox to check for objects within the Box and put them in an array
        Collider[] colliders = Physics.OverlapBox(transform.position, _boxSize, Quaternion.identity);

        //If the array isn't empty
        if (colliders.Length > 0)
        {
            // Check every objects in the array
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Agent"))
                {
                    Vector3 pushDirection = collider.transform.position - transform.position;

                    // Change the agent's destination to move away from the trigger
                    collider.GetComponent<NavMeshAgent>().SetDestination(collider.transform.position + pushDirection);
                }
            }
        }
        else
        {
            // No objects found within the Box
            Debug.Log("No objects within the Box.");
        }
    }
}
