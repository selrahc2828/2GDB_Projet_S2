using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResetAgent : MonoBehaviour
{
    public GameManager _GameManagerScript;
    public LayerMask _affectedLayer;

    private void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!_GameManagerScript._gameLose && Input.GetMouseButton(1))
        {
            // Create a ray from the mouse cursor position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about the raycast hit
            RaycastHit hit;
            float radius = 2f;
            // Perform the raycast
            if (Physics.SphereCast(ray, radius, out hit, float.MaxValue, _affectedLayer))
            {
                if (!hit.collider.isTrigger && hit.collider.gameObject.CompareTag("Agent") && hit.collider.gameObject.GetComponent<RecognizeItsSelf>()._aviability)
                {
                    List<UnityEngine.AI.NavMeshAgent> _itsList = hit.collider.gameObject.GetComponent<RecognizeItsSelf>().WitchListIsIt();
                    if (_itsList != null)
                    {
                        foreach (NavMeshAgent _agent in _itsList)
                        {
                            _agent.GetComponent<RecognizeItsSelf>().ResetPosition();
                            _agent.GetComponent<AgentFonction>().LayDownMine();
                        }
                    }
                }
            }
        }
    }
}
