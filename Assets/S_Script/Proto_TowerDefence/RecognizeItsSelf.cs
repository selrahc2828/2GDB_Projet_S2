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
    public bool _launchTimer;
    public GameManager _gameManager;
    public Material _initialMaterial;
    public Material _tiredMaterial;

    public float _lerpDuration = 15f;
    public MeshRenderer _meshRenderer;
    public float _lerpAmount;


    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        //_initialMaterial = GetComponent<MeshRenderer>().material;
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _TraceScript = GameObject.FindObjectOfType<AgentToTrace>();
    }
    private void Start()
    {
        _lerpDuration = _gameManager._fatigueDuration;
        _lerpAmount = 0f;
        _resetTime = _gameManager._resetTime;
        _launchTimer = false;
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
                    //Debug.Log(IsAvailable());
                    //Debug.Log(IsInShape());
                    //Debug.Log(WitchListIsIt().Count);
                }
            }
        }

        if(_launchTimer)
        {
            if(_selfAgent.remainingDistance < 5)
            {
                //ExaustAgent();
                // Calculate lerpAmount based on elapsed time and lerp duration
                
                _lerpAmount += Time.deltaTime;
                if (_lerpAmount > _lerpDuration)
                {
                    _lerpAmount = _lerpDuration;
                }

                float perc = _lerpAmount / _lerpDuration;
                // Lerp between color1 and color2 based on lerpAmount
                UnityEngine.Color lerpedColor = UnityEngine.Color.Lerp(_initialMaterial.color, _tiredMaterial.color, perc);

                // Apply the lerped color to the renderer's material
                _meshRenderer.material.color = lerpedColor;
                Debug.Log(_lerpAmount);
                // If lerp is complete, reset the elapsed time and stop lerp
                if (_lerpAmount >= 15f)
                {
                    _launchTimer = false;
                }
            }
        }
    }

    IEnumerator ResetPositionInTimer()
    {
        yield return new WaitForSeconds(_resetTime);

        ResetPosition();
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
        _launchTimer = false;
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

    }
}
