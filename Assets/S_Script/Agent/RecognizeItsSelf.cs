using FMOD;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RecognizeItsSelf : MonoBehaviour
{
    [Header("Variable de Script")]
    public AgentToTrace _TraceScript;
    public GameManager _gameManager;
    public AgentFonction _AgentFonctionScript;
    public GameObject _ParticulFatigue;
    public ChainFeedBack _ChainFeedbackScript;


    [Header("Variable de Fatigue")]
    public float _exaustionLevel;
    private float _exaustionTrueLevel;
    private float _exaustionMaxLevel;
    public float _Intensity;
    public float _threshold;
    public Material _ShaderMaterial;
    public MeshRenderer _meshRenderer;


    [Header("Variable de chainage des agents")]
    public int _towerProximityValue;
    public float _towerProximityNormalizedValue;
    public int _neighbourLowerProximityValue;
    public int _pool1ProximityValue;
    public float _pool1ProximityNormalizedValue;
    public int _pool1NeighbourLowerProximityValue;
    public int _pool2ProximityValue;
    public float _pool2ProximityNormalizedValue;
    public int _pool2NeighbourLowerProximityValue;
    public int _pool3ProximityValue;
    public float _pool3ProximityNormalizedValue;
    public int _pool3NeighbourLowerProximityValue;
    public int _pool4ProximityValue;
    public float _pool4ProximityNormalizedValue;
    public int _pool4NeighbourLowerProximityValue;

    public float _proximityTimer;
    public GameObject _tower;
    private Collider[] _neighbourAgents;
    private Collider[] _pool1NeighbourAgents;
    private Collider[] _pool2NeighbourAgents;
    private Collider[] _pool3NeighbourAgents;
    private Collider[] _pool4NeighbourAgents;


    [Header("Variable de Pools")]
    public GameObject _GOpool1;
    public GameObject _GOpool2;
    public GameObject _GOpool3;
    public GameObject _GOpool4;
    public bool _pool1; //slow
    public bool _pool2; //dmg
    public bool _pool3; //dmg
    public bool _pool4; //dmg


    [Header("Autre")]
    public bool _canShoot;
    public bool _aviability;
    public float _resetTime;
    private Vector3 _basePosition;
    private NavMeshAgent _selfAgent;
    private Dictionary<NavMeshAgent, bool> _dictionnaireAgents;
    public List<NavMeshAgent> _amIFocus;

    [Header("Color")]
    [ColorUsage(false, true)]
    public Color _initialColor;
    [ColorUsage(false, true)]
    public Color _finalColor;
    [ColorUsage(false, true)]
    public Color _AvailabilityColor;



    private void Awake()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _TraceScript = GameObject.FindObjectOfType<AgentToTrace>();
        _AgentFonctionScript = this.GetComponentInParent<AgentFonction>();
        _ChainFeedbackScript = this.GetComponentInParent<ChainFeedBack>();

    }
    private void Start()
    {
        _neighbourLowerProximityValue = -2;
        _towerProximityValue = -1;
        _towerProximityNormalizedValue = 1;

        _pool1NeighbourLowerProximityValue = -2;
        _pool1ProximityValue = -1;
        _pool1ProximityNormalizedValue = 1;

        _pool2NeighbourLowerProximityValue = -2;
        _pool2ProximityValue = -1;
        _pool2ProximityNormalizedValue = 1;

        _pool3NeighbourLowerProximityValue = -2;
        _pool3ProximityValue = -1;
        _pool3ProximityNormalizedValue = 1;

        _pool4NeighbourLowerProximityValue = -2;
        _pool4ProximityValue = -1;
        _pool4ProximityNormalizedValue = 1;

        _proximityTimer = 0;

        _exaustionMaxLevel = _gameManager._maxFatigueSeconde;
        _exaustionLevel = 0;
        _resetTime = _gameManager._resetTime;
        _aviability = true;
        _canShoot = false;
        _basePosition = transform.position;
        _selfAgent = GetComponent<NavMeshAgent>();

        StartCoroutine(CheckProximityFunctionsCoroutine());
    }

    private void Update()
    {
        if (!_gameManager._gameLose)
        {
            CalculateExaustion();
            UpdateExaustionMeter();
            _proximityTimer += Time.deltaTime;
            if (_proximityTimer > 1)
            {
                StartCoroutine(CheckProximityFunctionsCoroutine());
                _proximityTimer = 0;
            }
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(CheckProximityFunctionsCoroutine());
            }
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(CheckProximityFunctionsCoroutine());
            }
        }

        if (_exaustionLevel >= 0.9f)
        {
            _ParticulFatigue.SetActive(true);
        }
        else
        {
            _ParticulFatigue.SetActive(false);
        }

        //if(_aviability)
        //{
        //    _meshRenderer.enabled = false;
        //}
        //else
        //{
        //    _meshRenderer.enabled = true;
        //}

    }
    IEnumerator ResetPositionInTimer()
    {
        yield return new WaitForSeconds(_resetTime);

        ResetPosition();
    }

    private IEnumerator CheckProximityFunctionsCoroutine()
    {
        while (!_aviability)
        {
            _neighbourLowerProximityValue = -2;
            _towerProximityValue = -1;
            _towerProximityNormalizedValue = 1;

            _pool1NeighbourLowerProximityValue = -2;
            _pool1ProximityValue = -1;
            _pool1ProximityNormalizedValue = 1;

            _pool2NeighbourLowerProximityValue = -2;
            _pool2ProximityValue = -1;
            _pool2ProximityNormalizedValue = 1;

            _pool3NeighbourLowerProximityValue = -2;
            _pool3ProximityValue = -1;
            _pool3ProximityNormalizedValue = 1;

            _pool4NeighbourLowerProximityValue = -2;
            _pool4ProximityValue = -1;
            _pool4ProximityNormalizedValue = 1;
            // Attendre 1 seconde
            yield return new WaitForSeconds(1f);
            // Exécuter CheckProximity()
            CheckProximityFunctions();
        }
    }

    private void CheckProximityFunctions()
    {
        CheckTowerProximity();
        CheckPoolsProximity();
    }

    public int getTowerProximity()
    {
        return _towerProximityValue;
    }

    public void CheckTowerProximity()
    {
        if (Vector3.Distance(transform.position, _tower.transform.position) <= 10)
        {
            _towerProximityValue = 0;
        }
        else
        {
            _neighbourAgents = Physics.OverlapSphere(transform.position, 2);
            _neighbourLowerProximityValue = -2;
            bool lowerNeighbour = false;
            foreach (Collider agentCollider in _neighbourAgents)
            {
                if (agentCollider.CompareTag("Agent"))
                {
                    int _neighbourProximityValue = agentCollider.GetComponent<RecognizeItsSelf>()._towerProximityValue;
                    if (_neighbourProximityValue > -1)
                    {
                        if (_neighbourLowerProximityValue == -2 || _neighbourProximityValue < _neighbourLowerProximityValue)
                        {
                            _neighbourLowerProximityValue = _neighbourProximityValue;
                            lowerNeighbour = true;
                        }

                    }
                }
            }
            if(lowerNeighbour)
            {
                _towerProximityValue = _neighbourLowerProximityValue + 1;
            }
            else
            {
                _towerProximityValue = -1;
            }
            _ChainFeedbackScript.posOnLine = _towerProximityValue;

        }

        _towerProximityNormalizedValue = _towerProximityValue / 100f;

        if (_towerProximityNormalizedValue < 0)
        {
            _towerProximityNormalizedValue = 1;
        }
        
    }

    #region Pools

    public void CheckPoolsProximity()
    {
        CheckPool1Proximity();
        CheckPool2Proximity();
        CheckPool3Proximity();
        CheckPool4Proximity();
    }

    public void CheckPool1Proximity()
    {
        if (Vector3.Distance(transform.position, _GOpool1.transform.position) <= 10 && _GOpool1.tag != "Infected")
        {
            _pool1ProximityValue = 0;
        }
        else
        {
            _pool1NeighbourAgents = Physics.OverlapSphere(transform.position, 2);
            _pool1NeighbourLowerProximityValue = -2;
            bool lowerPool1Neighbour = false;
            foreach (Collider agentCollider in _pool1NeighbourAgents)
            {
                if (agentCollider.CompareTag("Agent"))
                {
                    int _pool1NeighbourProximityValue = agentCollider.GetComponent<RecognizeItsSelf>()._pool1ProximityValue;
                    if (_pool1NeighbourProximityValue > -1)
                    {
                        if (_pool1NeighbourLowerProximityValue == -2 || _pool1NeighbourProximityValue <= _pool1NeighbourLowerProximityValue)
                        {
                            _pool1NeighbourLowerProximityValue = _pool1NeighbourProximityValue;
                            lowerPool1Neighbour = true;
                        }
                    }
                }
            }
            if (lowerPool1Neighbour)
            {
                _pool1ProximityValue = _pool1NeighbourLowerProximityValue + 1;
            }
            else
            {
                _pool1ProximityValue = -1;
            }
            //_ChainFeedbackScript.posOnLine = _pool1ProximityValue; // ligne pour le feedback lumineux

        }

        _pool1ProximityNormalizedValue = _pool1ProximityValue / 100f;

        if (_pool1ProximityNormalizedValue < 0)
        {
            _pool1ProximityNormalizedValue = 1;
        }
    }

    public void CheckPool2Proximity()
    {
        if (Vector3.Distance(transform.position, _GOpool2.transform.position) <= 10 && _GOpool2.tag != "Infected")
        {
            _pool2ProximityValue = 0;
        }
        else
        {
            _pool2NeighbourAgents = Physics.OverlapSphere(transform.position, 2);
            _pool2NeighbourLowerProximityValue = -2;
            bool lowerPool2Neighbour = false;
            foreach (Collider agentCollider in _pool2NeighbourAgents)
            {
                if (agentCollider.CompareTag("Agent"))
                {
                    int _pool2NeighbourProximityValue = agentCollider.GetComponent<RecognizeItsSelf>()._pool2ProximityValue;
                    if (_pool2NeighbourProximityValue > -1)
                    {
                        if (_pool2NeighbourLowerProximityValue == -2 || _pool2NeighbourProximityValue <= _pool2NeighbourLowerProximityValue)
                        {
                            _pool2NeighbourLowerProximityValue = _pool2NeighbourProximityValue;
                            lowerPool2Neighbour = true;
                        }
                    }
                }
            }
            if (lowerPool2Neighbour)
            {
                _pool2ProximityValue = _pool2NeighbourLowerProximityValue + 1;
            }
            else
            {
                _pool2ProximityValue = -1;
            }
            //_ChainFeedbackScript.posOnLine = _pool2ProximityValue; // ligne pour le feedback lumineux

        }

        _pool2ProximityNormalizedValue = _pool2ProximityValue / 100f;

        if (_pool2ProximityNormalizedValue < 0)
        {
            _pool2ProximityNormalizedValue = 1;
        }
    }
    
    public void CheckPool3Proximity()
    {
        if (Vector3.Distance(transform.position, _GOpool3.transform.position) <= 10 && _GOpool3.tag != "Infected")
        {
            _pool3ProximityValue = 0;
        }
        else
        {
            _pool3NeighbourAgents = Physics.OverlapSphere(transform.position, 2);
            _pool3NeighbourLowerProximityValue = -2;
            bool lowerPool3Neighbour = false;
            foreach (Collider agentCollider in _pool3NeighbourAgents)
            {
                if (agentCollider.CompareTag("Agent"))
                {
                    int _pool3NeighbourProximityValue = agentCollider.GetComponent<RecognizeItsSelf>()._pool3ProximityValue;
                    if (_pool3NeighbourProximityValue > -1)
                    {
                        if (_pool3NeighbourLowerProximityValue == -2 || _pool3NeighbourProximityValue <= _pool3NeighbourLowerProximityValue)
                        {
                            _pool3NeighbourLowerProximityValue = _pool3NeighbourProximityValue;
                            lowerPool3Neighbour = true;
                        }
                    }
                }
            }
            if (lowerPool3Neighbour)
            {
                _pool3ProximityValue = _pool3NeighbourLowerProximityValue + 1;
            }
            else
            {
                _pool3ProximityValue = -1;
            }
            //_ChainFeedbackScript.posOnLine = _pool3ProximityValue; // ligne pour le feedback lumineux

        }

        _pool3ProximityNormalizedValue = _pool3ProximityValue / 100f;

        if (_pool3ProximityNormalizedValue < 0)
        {
            _pool3ProximityNormalizedValue = 1;
        }
    }

    public void CheckPool4Proximity()
    {
        if (Vector3.Distance(transform.position, _GOpool4.transform.position) <= 10 && _GOpool4.tag != "Infected")
        {
            _pool4ProximityValue = 0;
        }
        else
        {
            _pool4NeighbourAgents = Physics.OverlapSphere(transform.position, 2);
            _pool4NeighbourLowerProximityValue = -2;
            bool lowerPool4Neighbour = false;
            foreach (Collider agentCollider in _pool4NeighbourAgents)
            {
                if (agentCollider.CompareTag("Agent"))
                {
                    int _pool4NeighbourProximityValue = agentCollider.GetComponent<RecognizeItsSelf>()._pool4ProximityValue;
                    if (_pool4NeighbourProximityValue > -1)
                    {
                        if (_pool4NeighbourLowerProximityValue == -2 || _pool4NeighbourProximityValue <= _pool4NeighbourLowerProximityValue)
                        {
                            _pool4NeighbourLowerProximityValue = _pool4NeighbourProximityValue;
                            lowerPool4Neighbour = true;
                        }
                    }
                }
            }
            if (lowerPool4Neighbour)
            {
                _pool4ProximityValue = _pool4NeighbourLowerProximityValue + 1;
            }
            else
            {
                _pool4ProximityValue = -1;
            }
            //_ChainFeedbackScript.posOnLine = _pool2ProximityValue; // ligne pour le feedback lumineux

        }

        _pool4ProximityNormalizedValue = _pool4ProximityValue / 100f;

        if (_pool4ProximityNormalizedValue < 0)
        {
            _pool4ProximityNormalizedValue = 1;
        }
    }

    #endregion Pools

    #region Exaustion

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
                _canShoot = true;
                CheckTowerProximity();
                if (_exaustionTrueLevel <= _exaustionMaxLevel)
                {
                    _exaustionTrueLevel += Time.deltaTime * _towerProximityNormalizedValue;
                }
                CheckPoolsProximity();
            }
        }
        else
        {
            _towerProximityValue = -1;
        }

        _exaustionLevel = _exaustionTrueLevel / _exaustionMaxLevel;
    }

    public void UpdateExaustionMeter()
    {
        Color finalColor;
        float intensity;

        if (_aviability)
        {
            finalColor = _AvailabilityColor;
            intensity = 3f;
        }
        else
        {
            // Si l'agent n'est pas disponible, utiliser l'intensité du seuil
            finalColor = Color.Lerp(_initialColor, _finalColor, _exaustionLevel);
            intensity = _threshold - (_exaustionLevel * 10f);
        }

        finalColor *= intensity;
        finalColor += _ChainFeedbackScript.burstIntensity;
        _meshRenderer.material.SetColor("_FresnelColor", finalColor);
    }

    #endregion Pools


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
        if (WitchListIsIt() != null)
        {
            List<NavMeshAgent> _listeOfThisAgent = WitchListIsIt();
            _TraceScript._dictionnaireOfListeAgent.Remove(_listeOfThisAgent);
        }
        _canShoot = false;
        _AgentFonctionScript._mineUsed = false;
        _pool1 = false;
        _pool2 = false;
        _neighbourLowerProximityValue = -2;
        _towerProximityValue = -1;
        _towerProximityNormalizedValue = 1;
        _dictionnaireAgents = _TraceScript._dictionnaireAgent;
        _dictionnaireAgents[_selfAgent] = true;
        _aviability = true;
        foreach (NavMeshAgent _enemyAgent in _amIFocus)
        {
            //_enemyAgent.GetComponent<HomeWrecker>().SearchNewDestination();
        }
    }
}
