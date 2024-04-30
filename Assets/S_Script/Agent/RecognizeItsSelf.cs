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
    public GameObject _tower;
    private Collider[] _neighbourAgents;


    [Header("Variable de Pools")]
    public GameObject _GOpool1;
    public GameObject _GOpool2;
    public bool _pool1; //slow
    public bool _pool2;




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



    private void Awake()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _TraceScript = GameObject.FindObjectOfType<AgentToTrace>();
        _AgentFonctionScript = this.GetComponentInParent<AgentFonction>();
    }
    private void Start()
    {
        _neighbourLowerProximityValue = -2;
        _towerProximityValue = -1;
        _towerProximityNormalizedValue = 1;

        _pool1 = false;
        _pool2 = false;

        //_animatorsPool = FindObjectsOfType<Animator>();

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
        }

        if (_exaustionLevel >= 0.9f)
        {
            _ParticulFatigue.SetActive(true);
        }
        else
        {
            _ParticulFatigue.SetActive(false);
        }

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
            // Exécuter CheckProximity()
            CheckProximityFunctions();

            // Attendre 1 seconde
            yield return new WaitForSeconds(1f);
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
            foreach (Collider agentCollider in _neighbourAgents)
            {
                if (agentCollider.CompareTag("Agent"))
                {
                    int _neighbourProximityValue = agentCollider.GetComponent<RecognizeItsSelf>()._towerProximityValue;
                    if (_neighbourProximityValue > -1)
                    {
                        if (_neighbourLowerProximityValue == -2 || _neighbourProximityValue <= _neighbourLowerProximityValue)
                        {
                            _neighbourLowerProximityValue = _neighbourProximityValue;
                        }
                    }
                }
            }

            _towerProximityValue = _neighbourLowerProximityValue + 1;

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
    }

    public void CheckPool1Proximity()
    {
        if (Vector3.Distance(transform.position, _GOpool1.transform.position) <= 10)
        {
            _pool1 = true;
        }
        else
        {
            _neighbourAgents = Physics.OverlapSphere(transform.position, 2);
            bool _tempPool1 = false;
            foreach (Collider agentCollider in _neighbourAgents)
            {
                if (agentCollider.CompareTag("Agent"))
                {
                    bool _neighbourPool1Value = agentCollider.GetComponent<RecognizeItsSelf>()._pool1;
                    if (_neighbourPool1Value)
                    {
                        _tempPool1 = true;
                    }
                }
            }
            _pool1 = _tempPool1;
            
        }
    }

    public void CheckPool2Proximity()
    {
        if (Vector3.Distance(transform.position, _GOpool2.transform.position) <= 10)
        {
            _pool2 = true;
            
        }
        else
        {
            _neighbourAgents = Physics.OverlapSphere(transform.position, 2);
            bool _tempPool2 = false;
            foreach (Collider agentCollider in _neighbourAgents)
            {
                if (agentCollider.CompareTag("Agent"))
                {
                    bool _neighbourPool2Value = agentCollider.GetComponent<RecognizeItsSelf>()._pool2;
                    if (_neighbourPool2Value)
                    {
                        _tempPool2 = true;
                    }
                }
            }
            _pool2 = _tempPool2;
            
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
            finalColor = _initialColor;
            intensity = 3f;
        }
        else
        {
            // Si l'agent n'est pas disponible, utiliser l'intensité du seuil
            finalColor = Color.Lerp(_initialColor, _finalColor, _exaustionLevel);
            intensity = _threshold - (_exaustionLevel * 10f);
        }

        finalColor *= intensity;
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
