using FMOD;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RecognizeItsSelf : MonoBehaviour
{
    public Vector3 _basePosition;
    public AgentToTrace _TraceScript;
    public NavMeshAgent _selfAgent;
    public Dictionary<NavMeshAgent, bool> _dictionnaireAgents;
    public float _resetTime;
    public GameManager _gameManager;
    public Material _initialMaterial;
    public Material _exaustedMaterial;
    public MeshRenderer _meshRenderer;

    public float _exaustionLevel;
    private float _exaustionTrueLevel;
    private float _exaustionMaxLevel;
    public bool _aviability;


    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _TraceScript = GameObject.FindObjectOfType<AgentToTrace>();
    }
    private void Start()
    {
        _exaustionMaxLevel = _gameManager._maxFatigue;
        _exaustionLevel = 0;
        _resetTime = _gameManager._resetTime;
        _aviability = true;
        _basePosition = transform.position;
        _selfAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CalculateExaustion();
        UpdateExaustionMeter();
    }

    IEnumerator ResetPositionInTimer()
    {
        yield return new WaitForSeconds(_resetTime);

        ResetPosition();
    }

    public void CalculateExaustion()
    {

        if (_selfAgent.remainingDistance < 0.5)
        {
            if (_aviability)
            {
                if (_exaustionTrueLevel >= 0)
                {
                    _exaustionTrueLevel = 0;
                }
            }
            else
            {
                if (_exaustionTrueLevel <= _exaustionMaxLevel)
                {
                    _exaustionTrueLevel += Time.deltaTime;
                }
            }
        }
        _exaustionLevel = _exaustionTrueLevel / _exaustionMaxLevel;
    }

    public void UpdateExaustionMeter()
    {
        // Lerp between color1 and color2 based on lerpAmount
        UnityEngine.Color lerpedColor = UnityEngine.Color.Lerp(_initialMaterial.color, _exaustedMaterial.color, _exaustionLevel);

        // Apply the lerped color to the renderer's material
        _meshRenderer.material.color = lerpedColor;
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

    public void ResetPosition()
    {
        _selfAgent.SetDestination(_basePosition);
        ResetAllAgentData();
    }

    public void ResetAllAgentData()
    {
        if(WitchListIsIt() != null)
        {
            List<NavMeshAgent> _listeOfThisAgent = WitchListIsIt();
            _TraceScript._dictionnaireOfListeAgent.Remove(_listeOfThisAgent);
        }

        _dictionnaireAgents = _TraceScript._dictionnaireAgent;
        _dictionnaireAgents[_selfAgent] = true;
        _aviability = true;

    }
}
