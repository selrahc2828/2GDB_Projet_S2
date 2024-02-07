using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentToTrace : MonoBehaviour
{
    public LayerMask _affectedLayer;

    Dictionary<Vector3, bool> _listePositionTrace;
    Dictionary<NavMeshAgent, bool> _listeAgent;
    public GameObject _parentAgent;

    private NavMeshAgent _chosenAgent;
    private Vector3 _chosenPosition;

    // Start is called before the first frame update
    void Start()
    {
        _listePositionTrace = new Dictionary<Vector3, bool>();
        _listeAgent = new Dictionary<NavMeshAgent, bool>();

        // Vérifier si l'objet parent a été attribué
        if (_parentAgent != null)
        {
            // Parcourir tous les enfants de l'objet parent
            foreach (Transform _agentTransform in _parentAgent.transform)
            {
                // Vérifier si l'enfant possède un composant NavMeshAgent
                NavMeshAgent _agent = _agentTransform.GetComponent<NavMeshAgent>();
                if (_agent != null)
                { 
                    // Ajouter le NavMeshAgent au dictionnaire avec son nom comme clé
                    _listeAgent.Add(_agent, true);
                }
            }
        }
        else
        {
            Debug.LogError("Objet parent non défini !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MakeTrace();

        if (Input.GetMouseButtonUp(0))
        {
            ComeToPoint();  
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 _position in _listePositionTrace.Keys)
        {
            Gizmos.DrawWireSphere(_position, 5);
        }
    }

    void MakeTrace()
    {
        // Check for left mouse button click
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
           
            if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
            {
                if (_listePositionTrace.Count > 0)
                {
                    bool _alreadyOneHere = false;
                    foreach (Vector3 _position in _listePositionTrace.Keys)
                    {
                        if (Vector3.Distance(hit.point, _position) <= 0.5)
                        {
                            _alreadyOneHere = true;      
                        }
                    }
                    if (!_alreadyOneHere)
                    {
                        _listePositionTrace.Add(hit.point, true);
                    }
                }
                else
                {
                    _listePositionTrace.Add(hit.point, true);
                }
            }
        }
    }
    void ComeToPoint()
    {
        Debug.Log("ok");
        foreach (KeyValuePair<Vector3, bool> _position in _listePositionTrace)
        {
            if (_position.Value)
            {
                float _distanceMin = 0;
                foreach (KeyValuePair<NavMeshAgent, bool> _agentPosition in _listeAgent)
                {
                    if (_agentPosition.Value)
                    {
                        if (_distanceMin != 0)
                        {
                            if (Vector3.Distance(_position.Key, _agentPosition.Key.transform.position) < _distanceMin)
                            {
                                _distanceMin = Vector3.Distance(_position.Key, _agentPosition.Key.transform.position);
                                _chosenAgent = _agentPosition.Key;
                                _chosenPosition = _position.Key;
                            }
                        }
                        else
                        {
                            _distanceMin = Vector3.Distance(_position.Key, _agentPosition.Key.transform.position);
                            _chosenAgent = _agentPosition.Key;
                            _chosenPosition = _position.Key;
                        }
                    }
                }
                _chosenAgent.SetDestination(_chosenPosition);
                _listeAgent[_chosenAgent] = false;
                _listePositionTrace[_chosenPosition] = false;
            }
        }
    }
}
