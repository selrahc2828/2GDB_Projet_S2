using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfluenceGregAndMaggie : MonoBehaviour
{
    private Vector3 _boxSize;

    [Header("BoolToMovement")]
    public bool _Greg;
    public bool _Maggie;

    void Start()
    {
        //Check if there are already agents in the triggerZone and change their destination if there is
        PullObjectToPosition();
        PushObjectToPosition();
        
    }


    private void OnTriggerEnter(Collider other)
    {
        // ----------------- Pull the Agent Greg ---------------
        //If the object that trigger is tagged "Agent"
        if (other.CompareTag("Greg") && _Greg == true)
        {
            //Change the agent's destination to the Influence Tower
            other.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        }
        else
        {
           PushObjectToPosition();
        }
        // --------------- ---------------


        // ----------------- Pull the Agent Maggie ---------------
        //If the object that trigger is tagged "Agent"
        if (other.CompareTag("Maggie") && _Maggie == true)
        {
            //Change the agent's destination to the Influence Tower
            other.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        }
        else
        {
           PushObjectToPosition();
        }
        // --------------- ---------------


    }

    void PullObjectToPosition()
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
                if (collider.CompareTag("Greg") && _Greg == true || collider.CompareTag("Maggie") && _Maggie == true)
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
                if (collider.CompareTag("Greg") && _Maggie == true || collider.CompareTag("Maggie") && _Greg == true)
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
