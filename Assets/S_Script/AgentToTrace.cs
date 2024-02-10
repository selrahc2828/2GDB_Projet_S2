using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class AgentToTrace : MonoBehaviour
{
    public LayerMask _affectedLayer;

    Dictionary<Vector3, bool> _listePositionTrace;
    Dictionary<NavMeshAgent, bool> _listeAgent;

    List<Dictionary<Vector3, bool>> _listeOfListePositionTrace;
    public int _numberAgentAviable;
    public GameObject _parentAgent;

    private NavMeshAgent _chosenAgent;
    private Vector3 _chosenPosition;
    private float _sizeAgent;

    public Text _AgentDispo;


    // Start is called before the first frame update
    void Start()
    {
        _sizeAgent = 1f;
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
        CountNumberAgentAviable();
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

        _AgentDispo.text = "Agent Disponible : " + _numberAgentAviable;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 _position in _listePositionTrace.Keys)
        {
            Gizmos.DrawWireSphere(_position, 5);
        }
    }

    void CountNumberAgentAviable()
    {
        _numberAgentAviable = 0;
        foreach (KeyValuePair<NavMeshAgent, bool> _agent in _listeAgent)
        {
            if (_agent.Value)
            {
                _numberAgentAviable++;
            }
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
                        InterpolateNewPointInBetween(hit.point, _listePositionTrace.Last());
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
                    if (Vector3.Distance(_newPoint, _oldPoint.Key) <= _sizeAgent)
                    {
                        return true;
                    }
                }
            }
        }
        foreach(KeyValuePair<Vector3, bool> _newPointInNewDictionary in _newDictionary)
        {
            if (Vector3.Distance(_newPoint, _newPointInNewDictionary.Key) <= _sizeAgent)
            {
                return true;
            }
        }
        return false;
    }

    void InterpolateNewPointInBetween(Vector3 _newPoint, KeyValuePair<Vector3, bool> _lastPoint)
    {
        float _distanceBetweenNewAndLast = Vector3.Distance(_newPoint, _lastPoint.Key);
        if (_distanceBetweenNewAndLast > _sizeAgent)
        {
            //On calcul combien d'agent pourrais rentrer entre les 2 points avec Distance/taille
            int _numberOfPossiblePosition = Mathf.FloorToInt(_distanceBetweenNewAndLast / _sizeAgent)/2;
            float _remainingDistance = _distanceBetweenNewAndLast - (_numberOfPossiblePosition * _sizeAgent);
            if (_numberOfPossiblePosition > 0)
            {
                Vector3 _direction = (_newPoint - _lastPoint.Key).normalized;
                float _distanceBetweenPoints = _sizeAgent + (_remainingDistance / _numberOfPossiblePosition+1);
                for (int i = 1; i < _numberOfPossiblePosition; i++)
                {
                    Vector3 _interpolatedPosition = _lastPoint.Key + _direction * (_distanceBetweenPoints * i);
                    _listePositionTrace.Add(_interpolatedPosition, true);
                }
            }
        }
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
        CountNumberAgentAviable();
    }
}
