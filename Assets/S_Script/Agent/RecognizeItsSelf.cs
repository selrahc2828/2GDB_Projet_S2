using FMOD;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

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
    public int _pool1ProximityValue;
    public float _pool1ProximityNormalizedValue;
    public int _pool2ProximityValue;
    public float _pool2ProximityNormalizedValue;
    public int _pool3ProximityValue;
    public float _pool3ProximityNormalizedValue;
    public int _pool4ProximityValue;
    public float _pool4ProximityNormalizedValue;

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
    public ParticleSystem _ParticulResetPosition;
    public bool check;

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
        _ParticulResetPosition.Stop();
    }
    private void Start()
    {
        _towerProximityValue = -1;
        _towerProximityNormalizedValue = 1;

        _pool1ProximityValue = -1;
        _pool1ProximityNormalizedValue = 1;

        _pool2ProximityValue = -1;
        _pool2ProximityNormalizedValue = 1;

        _pool3ProximityValue = -1;
        _pool3ProximityNormalizedValue = 1;

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

    }


    private float _nextExecutionTime;

    private void Update()
    {
        /*
        if (Time.time >= _nextExecutionTime && check == true)
        {
            check = false;
            //CheckProximityFunctions();
        }
        */
        if (!_gameManager._gameLose)
        {
            CalculateExaustion();
            UpdateExaustionMeter();
            /*
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
            {
                check = true;
                CheckProximityFunctions();
                _nextExecutionTime = Time.time + 1f;

            }*/
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

    #region Pools

    private void CheckProximityFunctions()
    {
        if (!_aviability)
        {
            
            _towerProximityValue = -1;
            _towerProximityNormalizedValue = 1;

            _pool1ProximityValue = -1;
            _pool1ProximityNormalizedValue = 1;

            _pool2ProximityValue = -1;
            _pool2ProximityNormalizedValue = 1;

            _pool3ProximityValue = -1;
            _pool3ProximityNormalizedValue = 1;

            _pool4ProximityValue = -1;
            _pool4ProximityNormalizedValue = 1;
            

            CheckPoolsProximity();

        }
    }

    public int getTowerProximity()
    {
        return _towerProximityValue;
    }

    public int getPoolProximityNumber(GameObject pool)
    {
        switch (pool.name)
        {
            case "Tower":
                return _towerProximityValue;
            case "Pool1 - slow":
                return _pool1ProximityValue;
            case "Pool2 - damage":
                return _pool2ProximityValue;
            case "Pool3 - damage":
                return _pool3ProximityValue;
            case "Pool4 - damage":
                return _pool4ProximityValue;
            default:
                return -2;
        }
    }

    public void SetPoolNormalizedValue(GameObject pool, float number)
    {
        switch (pool.name)
        {
            case "Tower":
                _towerProximityNormalizedValue = number / 100f;

                if (_towerProximityNormalizedValue < 0)
                {
                    _towerProximityNormalizedValue = 1;
                }
                break;
            case "Pool1 - slow":
                _pool1ProximityNormalizedValue = number / 100f;

                if (_pool1ProximityNormalizedValue < 0)
                {
                    _pool1ProximityNormalizedValue = 1;
                }
                break;
            case "Pool2 - damage":
                _pool2ProximityNormalizedValue = number / 100f;

                if (_pool2ProximityNormalizedValue < 0)
                {
                    _pool2ProximityNormalizedValue = 1;
                }
                break;
            case "Pool3 - damage":
                _pool3ProximityNormalizedValue = number / 100f;

                if (_pool3ProximityNormalizedValue < 0)
                {
                    _pool3ProximityNormalizedValue = 1;
                }
                break;
            case "Pool4 - damage":
                _pool3ProximityNormalizedValue = number / 100f;

                if (_pool3ProximityNormalizedValue < 0)
                {
                    _pool3ProximityNormalizedValue = 1;
                }
                break;
        }
    }


    public void CheckPoolsProximity()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);

        _towerProximityValue = GetPoolProximity(_tower, hits);
        _pool1ProximityValue = GetPoolProximity(_GOpool1, hits);
        if( _pool1ProximityValue > 300)
        {
            _pool1ProximityValue = -1;
        }
        _pool2ProximityValue = GetPoolProximity(_GOpool2, hits);
        _pool3ProximityValue = GetPoolProximity(_GOpool3, hits);
        _pool4ProximityValue = GetPoolProximity(_GOpool4, hits);
    }

    public int GetPoolProximity(GameObject pool, Collider[] hits)
    {
        int proximityValue = -1;
        if (Vector3.Distance(transform.position, pool.transform.position) <= 10 && pool.tag != "Infected")
        {
            proximityValue = 0;
        }
        else
        {
            int neighbourProximityValue = -2;
            bool lowerNeighbourFound = false;
            foreach (Collider agentCollider in hits)
            {
                if (agentCollider.CompareTag("Agent"))
                {
                    int _thisNeighbourProximityValue = agentCollider.GetComponent<RecognizeItsSelf>().getPoolProximityNumber(pool);
                    if (_thisNeighbourProximityValue > -1)
                    {
                        if (neighbourProximityValue == -2 || _thisNeighbourProximityValue <= neighbourProximityValue)//c'est ptetre là // c'est pteter encore là mais pas pareil
                        {
                            neighbourProximityValue = _thisNeighbourProximityValue;
                            lowerNeighbourFound = true;
                        }
                    }
                }
            }
            if(getPoolProximityNumber(pool) > -1)
            {
                if (lowerNeighbourFound && neighbourProximityValue < getPoolProximityNumber(pool) && neighbourProximityValue >= -1)
                {
                    proximityValue = neighbourProximityValue + 1;
                }
                else
                {
                    proximityValue = -1;
                }
            }
            else if (lowerNeighbourFound && neighbourProximityValue >= -1)//ptetre le reset nique tout
            {
                proximityValue = neighbourProximityValue + 1;
            }
            else
            {
                proximityValue = -1;
            }
            _ChainFeedbackScript.SetPosOnLine(pool, proximityValue);// ligne pour le feedback lumineux
        }
        SetPoolNormalizedValue(pool, proximityValue);

        return proximityValue;
    }

    #endregion Pools

    #region Exaustion

    public void CalculateExaustion()
    {
        if (_selfAgent.remainingDistance < 0.5)
        {
            if (_aviability)
            {
                _dictionnaireAgents = _TraceScript._dictionnaireAgent;
                if (_exaustionTrueLevel >= 0 && _dictionnaireAgents[_selfAgent] == false)
                {
                    _exaustionTrueLevel = 0;
                    _dictionnaireAgents[_selfAgent] = true;
                    _TraceScript.CountNumberAgentAvailable();
                }
            }
            else
            {
                _canShoot = true;
                check = true;
                CheckProximityFunctions();
                _nextExecutionTime = Time.time + 1f;
                if (_exaustionTrueLevel <= _exaustionMaxLevel)
                {
                    _exaustionTrueLevel += Time.deltaTime * _towerProximityNormalizedValue;
                }
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
        _ParticulResetPosition.Play();
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
        _towerProximityValue = -1;
        _towerProximityNormalizedValue = 1;
        _aviability = true;
    }
}
