using FMOD;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class HomeWrecker : Enemy
{
    public AgentToTrace _agentToTraceScript;
    public NavMeshAgent _closestAgentInLineGoal;
    public Dictionary<NavMeshAgent, bool> _dictionnaireAgent;
    List<Vector3> _zigzagPointsList = new List<Vector3>();
    public int _currentListDestination;
    public int _tailleSegmentZigZag;
    public int _amplitudeZigZag;
    private bool _mouseButtonUp = false;
    private bool _agentStarted = false;
    public void Awake()
    {
        _Niveau1 = FindAnyObjectByType<Niveau1>();
        _GameManager = FindAnyObjectByType<GameManager>();
        _thisAgent = this.GetComponentInParent<NavMeshAgent>();
        _FeedbackScript = FindAnyObjectByType<FeedBack>();
        _agentToTraceScript = FindAnyObjectByType<AgentToTrace>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _agentStarted = false;
        _GameManager._numberOfEnemyOnScreen++;
        _GameManager._numberOfHomeWreckerOnScreen++;

        _MaxHealth = _GameManager._HeathHomeWreaker;
        _CurrentHealth = _MaxHealth;

        _slowPower = _GameManager._slowPower;
        _slowDuration = _GameManager._slowDuration;

        _thisAgentBaseSpeed = _GameManager._SpeedHomeWreaker;
        _thisAgent.speed = _GameManager._SpeedHomeWreaker;
        _thisAgent.acceleration = _GameManager._AccelerationHomeWreaker;
        _thisAgent.angularSpeed = _GameManager._AngularSpeedHomeWreaker;

        _tailleSegmentZigZag = _GameManager._tailleSegmentZigZagHomeWrecker;
        _amplitudeZigZag = _GameManager._amplitudeZigZagHomeWrecker;

        _mouseButtonUp = true;
        StartCoroutine(DelayedSearchNewDestination(0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColorOnHP();
        ApplySlow();
        if (Input.GetMouseButtonUp(0))
        {
            _mouseButtonUp = true;
            StartCoroutine(DelayedSearchNewDestination(0.5f));
        }
        if (_thisAgent.hasPath && _agentStarted)
        {
            if (_thisAgent.remainingDistance < 2)
            {
                DestinationReached();
            }
        }
    }

    public void SearchNewDestination()
    {
        if (FindClosestLine() != null)
        {
            _closestAgentInLineGoal = FindClosestLine();
            if (_zigzagPointsList.Count > 0 && _zigzagPointsList[_zigzagPointsList.Count - 1] != _closestAgentInLineGoal.destination)
            {
                CalculateZigZag(transform.position, _closestAgentInLineGoal.destination);
                GoToListDestination();
            }
            if (_zigzagPointsList.Count == 0)
            {
                CalculateZigZag(transform.position, _closestAgentInLineGoal.destination);
                GoToListDestination();
            }
        }
        else
        {
            _thisAgent.SetDestination(_TowerToDestroy.position);
        }
        _agentStarted = true;
    }

    public NavMeshAgent FindClosestLine()
    {
        _dictionnaireAgent = _agentToTraceScript._dictionnaireAgent;
        NavMeshAgent _closestAgentInLine = null;
        float _ShortestDistance = 0;

        foreach (KeyValuePair<NavMeshAgent, bool> pair in _dictionnaireAgent)
        {
            if (!pair.Value)
            {
                if (_ShortestDistance == 0)
                {
                    _ShortestDistance = Vector3.Distance(transform.position, pair.Key.transform.position);
                    _closestAgentInLine = pair.Key;
                }
                else
                {
                    if (Vector3.Distance(transform.position, pair.Key.transform.position) < _ShortestDistance)
                    {
                        _ShortestDistance = Vector3.Distance(transform.position, pair.Key.transform.position);
                        _closestAgentInLine = pair.Key;
                    }
                }
            }
        }
        if (_closestAgentInLine != null)
            _closestAgentInLine.GetComponent<RecognizeItsSelf>()._amIFocus.Add(_thisAgent);
        return _closestAgentInLine;
    }

    public void CalculateZigZag(Vector3 _start, Vector3 _end)
    {
        _zigzagPointsList.Clear();

        Vector3 _toTarget = _end - _start;

        Vector3 _perpendicular = Vector3.Cross(_toTarget.normalized, Vector3.up).normalized;
        int _numberOfZigZag = (int)Vector3.Distance(transform.position, _TowerToDestroy.position) / _tailleSegmentZigZag;
        if (_numberOfZigZag > 0)
        {
            for (int i = 1; i <= _numberOfZigZag; i++)
                _zigzagPointsList.Add(_start + _perpendicular * _amplitudeZigZag * Mathf.Pow(-1, i) + i * _toTarget / (_numberOfZigZag + 1));
        }
        _zigzagPointsList.Add(_end);
    }

    public void GoToListDestination()
    {
        _currentListDestination = 0;
        _thisAgent.SetDestination(_zigzagPointsList[_currentListDestination]);
    }

    public void DestinationReached()
    {
        if (_currentListDestination != _zigzagPointsList.Count -1)
        {
            _currentListDestination++;
            _thisAgent.SetDestination(_zigzagPointsList[_currentListDestination]);
        }
        else
        {
            _agentStarted = false;
            List<NavMeshAgent> _itsList = _closestAgentInLineGoal.GetComponent<RecognizeItsSelf>().WitchListIsIt();
            if (_itsList != null)
            {
                foreach (NavMeshAgent _agent in _itsList)
                {
                    _agent.GetComponent<RecognizeItsSelf>().ResetPosition();
                }
            }
            Die();
        }
    }

    private void OnDestroy()
    {
        _GameManager._numberOfHomeWreckerOnScreen--;
        _GameManager._numberOfEnemyOnScreen--;
        _Niveau1.CheckForNextWave();
    }
    IEnumerator DelayedSearchNewDestination(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (_mouseButtonUp)
        {
            SearchNewDestination();
            _mouseButtonUp = false; // Reset the flag
        }
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 _zigzagPoint in _zigzagPointsList)
            Gizmos.DrawWireSphere(_zigzagPoint, 5f);
    }*/
}
