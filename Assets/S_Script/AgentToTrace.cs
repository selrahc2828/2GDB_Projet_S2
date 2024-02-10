using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AgentToTrace : MonoBehaviour
{
    public LayerMask _affectedLayer;

    Dictionary<Vector3, bool> _listePositionTrace;
    Dictionary<NavMeshAgent, bool> _listeAgent;

    List<Dictionary<Vector3, bool>> _listeOfListePositionTrace;
    public GameObject _parentAgent;

    private NavMeshAgent _chosenAgent;
    private Vector3 _chosenPosition;

    // Start is called before the first frame update
    void Start()
    {
        _listePositionTrace = new Dictionary<Vector3, bool>();
        _listeAgent = new Dictionary<NavMeshAgent, bool>();
        _listeOfListePositionTrace = new List<Dictionary<Vector3, bool>>();

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
            _listeOfListePositionTrace.Add(_listePositionTrace);
            ComeToPoint();
            _listePositionTrace.Clear();
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
                    if(!CheckIfPointAlreadyExistHere(hit.point, _listePositionTrace))
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

    bool CheckIfPointAlreadyExistHere(Vector3 _newPoint, Dictionary<Vector3, bool> _newDictionary)
    {
        if(_listeOfListePositionTrace.Count > 0)
        {
            foreach (Dictionary<Vector3, bool> _oldDictionary in _listeOfListePositionTrace)
            {
                foreach (KeyValuePair<Vector3, bool> _oldPoint in _oldDictionary)
                {
                    if (Vector3.Distance(_newPoint, _oldPoint.Key) <= 0.5)
                    {
                        return true;
                    }
                }
            }
        }
        foreach(KeyValuePair<Vector3, bool> _newPointInNewDictionary in _newDictionary)
        {
            if (Vector3.Distance(_newPoint, _newPointInNewDictionary.Key) <= 0.5)
            {
                return true;
            }
        }
        return false;
    }

    void ComeToPoint()
    {
        List<Vector3> _keysPosition = new List<Vector3>(_listePositionTrace.Keys);
        List<NavMeshAgent> _keysAgent = new List<NavMeshAgent>(_listeAgent.Keys);

        for (int i = 0; i < _keysPosition.Count; i++)
        {
            Vector3 _position = _keysPosition[i];
            bool _value = _listePositionTrace[_position];
            if (_value)
            {
                float _distanceMin = 0;
                for (int j = 0; j < _keysAgent.Count; j++)
                {
                    NavMeshAgent _agentPosition = _keysAgent[j];
                    bool _agentValue = _listeAgent[_agentPosition];
                    if (_agentValue)
                    {
                        if (_distanceMin != 0)
                        {
                            if (Vector3.Distance(_position, _agentPosition.transform.position) < _distanceMin)
                            {
                                _distanceMin = Vector3.Distance(_position, _agentPosition.transform.position);
                                _chosenAgent = _agentPosition;
                                _chosenPosition = _position;
                            }
                        }
                        else
                        {
                            _distanceMin = Vector3.Distance(_position, _agentPosition.transform.position);
                            _chosenAgent = _agentPosition;
                            _chosenPosition = _position;
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
