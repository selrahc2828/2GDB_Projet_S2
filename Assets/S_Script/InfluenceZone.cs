using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfluenceZone : MonoBehaviour
{
    private Vector3 _boxSize;

    void Start()
    {
        //Check if there are already agents in the triggerZone and change their destination if there is
        CheckObjectsInBox();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //If the object that trigger is tagged "Agent"
        if(other.CompareTag("Agent"))
        {
            //Change the agent's destination to the Influence Tower
            other.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        }
    }

    void CheckObjectsInBox()
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
                if(collider.CompareTag("Agent"))
                {
                    // Change their destination to be the Influence Tower
                    collider.GetComponent<NavMeshAgent>().SetDestination(transform.position);
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
