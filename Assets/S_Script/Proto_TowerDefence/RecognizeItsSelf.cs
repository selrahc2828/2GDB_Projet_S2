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

    [Header("Variable de Fatigue")]
    public float _exaustionLevel;
    private float _exaustionTrueLevel;
    private float _exaustionMaxLevel;
    public Material _MaterialEmisive;
    public MeshRenderer _meshRenderer;
    private float _InitialIntensity;

    public Color emissionColor = Color.white;


    [Header("Variable de chainage des agents")]
    public int _towerProximityValue;
    public int _neighbourLowerProximityValue;
    public GameObject _tower;
    private Collider[] _neighbourAgents;


    [Header("Autre")]
    public bool _canShoot;
    public bool _aviability;
    public float _resetTime;
    private Vector3 _basePosition;
    private NavMeshAgent _selfAgent;
    private Dictionary<NavMeshAgent, bool> _dictionnaireAgents;


    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _TraceScript = GameObject.FindObjectOfType<AgentToTrace>();
    }

    private void Start()
    {

        _neighbourLowerProximityValue = -2;
        _towerProximityValue = -1;
        _exaustionMaxLevel = _gameManager._maxFatigue;
        _exaustionLevel = 0;
        _resetTime = _gameManager._resetTime;
        _aviability = true;
        _canShoot = false;
        _basePosition = transform.position;
        _selfAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CalculateExaustion();
        //UpdateExaustionMeter();

        float emissionIntensity = Mathf.PingPong(Time.time, 1.0f);
        SetEmissionIntensity(emissionIntensity);
    }

    void SetEmissionIntensity(float intensity)
    {
        // Assurez-vous que le matériau est émissif
        if (_MaterialEmisive != null && _MaterialEmisive.HasProperty("_EmissionColor"))
        {
            // Calculer la composante verte (G) en fonction de l'épuisement
            float greenValue = Mathf.Lerp(255f, 0f, _exaustionLevel); // Utilisez Mathf.Lerp pour interpoler entre 255 (pleinement épuisé) et 0 (non épuisé)

            // Créer une nouvelle couleur en utilisant la valeur verte calculée, en gardant les autres composants (rouge et bleu) inchangés
            Color newEmissionColor = new Color(0, greenValue / 255f, 0); // Divisez par 255f pour normaliser la valeur entre 0 et 1

            // Appliquer la nouvelle couleur émissive
            _MaterialEmisive.SetColor("_EmissionColor", newEmissionColor);
        }
    }

    IEnumerator ResetPositionInTimer()
    {
        yield return new WaitForSeconds(_resetTime);

        ResetPosition();
    }

    public int getTowerProximity()
    {
        return _towerProximityValue;
    }

    public void CheckTowerProximity()
    {
        if(Vector3.Distance(transform.position, _tower.transform.position) <= 5)
        {
            _towerProximityValue = 0;
        }
        else
        {
            _neighbourAgents = Physics.OverlapSphere(transform.position, 2);
            _neighbourLowerProximityValue = -2;
            foreach(Collider agentCollider in _neighbourAgents)
            {
                if(agentCollider.CompareTag("Agent"))
                {
                    int _neighbourProximityValue = agentCollider.GetComponent<RecognizeItsSelf>()._towerProximityValue;
                    if(_neighbourProximityValue > -1)
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
                _canShoot = true;
                CheckTowerProximity();
                if (_exaustionTrueLevel <= _exaustionMaxLevel)
                {
                    _exaustionTrueLevel += Time.deltaTime;
                }
            }
        }
        else
        {
            _towerProximityValue = -1;
        }
        _exaustionLevel = _exaustionTrueLevel / _exaustionMaxLevel;
    }
    
    //public void UpdateExaustionMeter()
    //{
    //    // Assurez-vous que le matériau et le MeshRenderer sont définis
    //    if (_MaterialEmisive != null && _meshRenderer != null)
    //    {
    //        // Calculez la nouvelle intensité d'émission en multipliant par le niveau d'exhaustion
    //        float newEmissionIntensity = _InitialIntensity * _exaustionLevel;
    //        // Appliquez la nouvelle intensité d'émission au matériau
    //        _MaterialEmisive.SetFloat("_EmissionIntensity", newEmissionIntensity);

    //        // Actualisez le rendu pour voir les changements
    //        _meshRenderer.UpdateGIMaterials();
    //    }
    //}

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
