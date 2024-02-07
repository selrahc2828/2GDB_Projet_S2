using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfluenceZone : MonoBehaviour
{
    private Vector3 _boxSize;

    [Header("Reference")]
    public RandomMouvement _RandomMouvement;
    

    [Header("BoolToMovement")]
    public bool _Push;
    public bool _Pull;



    void Start()
    {


        //Check if there are already agents in the triggerZone and change their destination if there is
        if (_Pull == true)
        {
            PullObjectToPosition();
        }

        if (_Push == true)
        {
            PushObjectToPosition();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        // ----------------- Pull the Agent ---------------
        //If the object that trigger is tagged "Agent"
        if (other.CompareTag("Agent") && _Pull == true)
        {
           _RandomMouvement = other.GetComponent<RandomMouvement>();
           _RandomMouvement.enabled = false;
            
            if (_RandomMouvement == false)
            {
                Debug.Log("RandomMovement is disable");
            }

            //Change the agent's destination to the Influence Tower
            other.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        }
        // --------------- ---------------

        // ---------------- Push the Agent ----------------
        //If the object that trigger is tagged "Agent"
        if (other.CompareTag("Agent") && _Push == true)
        {
            Vector3 pushDirection = other.transform.position - transform.position;

            // Change the agent's destination to move away from the trigger
            other.GetComponent<NavMeshAgent>().SetDestination(other.transform.position + pushDirection);
        }
        // --------------- ---------------
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            _RandomMouvement = other.GetComponent<RandomMouvement>();
            _RandomMouvement.enabled = true;
        }
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
                if (collider.CompareTag("Agent"))
                {
                    Debug.Log(collider.name + ("RandomMovement"));
                    _RandomMouvement = collider.GetComponent<RandomMouvement>();
                    _RandomMouvement.enabled = false;
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
