using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResetAgent : MonoBehaviour
{
    public LayerMask _affectedLayer;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Create a ray from the mouse cursor position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about the raycast hit
            RaycastHit hit;
            float radius = 1f;
            // Perform the raycast
            if (Physics.SphereCast(ray, radius, out hit, float.MaxValue, _affectedLayer))
            {
                if (!hit.collider.isTrigger)
                {
                    // Check if the collider hit is attached to the object you're interested in
                    if (hit.collider.gameObject.CompareTag("Agent"))
                    {
                        List<UnityEngine.AI.NavMeshAgent> _itsList = hit.collider.gameObject.GetComponent<RecognizeItsSelf>().WitchListIsIt();
                        if( _itsList != null )
                        {
                            foreach (NavMeshAgent _agent in _itsList)
                            {
                                _agent.GetComponent<RecognizeItsSelf>().ResetPosition();
                            }
                        }
                    }
                }
            }
        }
    }
}
