using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RecognizeItsSelf : MonoBehaviour
{
    public Vector3 _basePosition;
    public AgentToTrace _TraceScript;
    public NavMeshAgent _selfAgent;
    public Dictionary<NavMeshAgent, bool> _dictionnaireAgents;


    private void Awake()
    {
        _TraceScript = GameObject.FindObjectOfType<AgentToTrace>();
    }
    private void Start()
    {
        _basePosition = transform.position;
        _selfAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Create a ray from the mouse cursor position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about the raycast hit
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the collider hit is attached to the object you're interested in
                if (hit.collider.gameObject == gameObject)
                {
                    // The object is clicked
                    Debug.Log(IsAvailable());
                    Debug.Log(IsInShape());
                }
            }
        }
    }

    public List<NavMeshAgent> WitchListIsIt()
    {
        if (!IsAvailable())
        {
            foreach (KeyValuePair<List<NavMeshAgent>, int> _listAgent in _TraceScript._dictionnaireOfListeAgent)
            {
                if (_listAgent.Key.Contains(_selfAgent))
                {
                    return _listAgent.Key;
                }
            }
        }
        return null;
        
    }

    public bool IsAvailable()
    {
        _dictionnaireAgents = _TraceScript._dictionnaireAgent;
        return _dictionnaireAgents[_selfAgent];
    }

    public int IsInShape()
    {
        if (!IsAvailable())
        {
            foreach (KeyValuePair<List<NavMeshAgent>, int> _listAgent in _TraceScript._dictionnaireOfListeAgent)
            {
                if (_listAgent.Key.Contains(_selfAgent))
                {
                    return _listAgent.Value;
                }
            }
            return -1;
        }
        else
        {
            return -1;
        }
    }
}
