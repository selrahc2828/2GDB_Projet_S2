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

public class AgentToTrace : MonoBehaviour
{
    [Header("Layer")]
    public LayerMask _affectedLayer;

    [Header("Dictionnaires")]
    Dictionary<Vector3, bool> _dictionnairePositionTrace;
    public  Dictionary<NavMeshAgent, bool> _dictionnaireAgent;
    public Dictionary<List<NavMeshAgent>, int> _dictionnaireOfListeAgent;

    [Header("Listes")]
    public List<List<Vector3>> _listeOfListePositionTrace;
    List<Vector3> _listePositionTrace;

    [Header("Variables utiles")]
    public int _numberAgentAvailable;
    public float espaceEntreAgentPourCercle;
    public float _slowMo;
    public int _numberAgentNeeded;

    public GameObject _parentAgent;
    public GameManager _gameManager;
    private NavMeshAgent _chosenAgent;
    private Vector3 _chosenPosition;
    private float _sizeAgent;
    public float _proportionOfAgentAssigned;
    public int _numberTotalAgent;
    public Text _AgentNumbertext;


    // Start is called before the first frame update
    void Start()
    {
        _numberAgentAvailable = 0;
        _slowMo = _gameManager._slowMo;
        // taille de l'agennt (diametre)
        _sizeAgent = 1f;
        //Dictionnaire dans lequel je stoque des position et un booléen par position
        _dictionnairePositionTrace = new Dictionary<Vector3, bool>();
        //Dictionnaire dans lequel je stoque tout les agents et un booléen par agent
        _dictionnaireAgent = new Dictionary<NavMeshAgent, bool>();
        //Liste dans laquelle je stoque les dictionnaires de position
        _listeOfListePositionTrace = new List<List<Vector3>>();
        _listePositionTrace = new List<Vector3>();
        _dictionnaireOfListeAgent = new Dictionary<List<NavMeshAgent>, int>();

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
                    _dictionnaireAgent.Add(_agent, true);
                }
            }
        }
        else
        {
            Debug.LogError("Objet parent non défini !");
        }
        //Appel de la fonction pour créer la variable du nombre d'agent disponible à un instant T
        CountNumberAgentAvailable();
        _numberTotalAgent = _dictionnaireAgent.Count();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_gameManager._gameLose && !_gameManager._gamePaused)
        {
            ProportionOfEngagedAgent();
            if (Input.GetMouseButtonDown(0))
            {
                _numberAgentNeeded = 0;
                // Reset the time scale to normal
                Time.timeScale = _slowMo;
            }

            // Check du click gauche de la souris
            if (Input.GetMouseButton(0))
            {
                //Appel de la fonction pour créer les points de position sur le tracé de la souris
                MakeTrace();
            }
            //Test si le bouton gauche de la souris est relevé
            if (Input.GetMouseButtonUp(0))
            {
                // Reset the time scale to normal
                Time.timeScale = 1f;
                //Ajout de la liste de position qui viens d'être créer dans la liste de liste
                _listeOfListePositionTrace.Add(new List<Vector3>(_listePositionTrace));
                StartCoroutine(DeleteWithDelay(_listeOfListePositionTrace[_listeOfListePositionTrace.Count - 1]));
                //Appel de la fonction pour changer la destination des agents
                ComeToPoint();
                //On vide la liste et le dictionnaire de position pour pouvoir s'en resservir au prochain tracé
                _dictionnairePositionTrace.Clear();
                _listePositionTrace.Clear();
                CountNumberAgentAvailable();
            }
            if (Input.GetMouseButtonUp(1))
            {
                CountNumberAgentAvailable();
            }
            _AgentNumbertext.text = "Agent Available : " + _numberAgentAvailable;
        }
    }

    IEnumerator DeleteWithDelay(List<Vector3> _liste)
    {
        yield return new WaitForSeconds(10f);

        _listeOfListePositionTrace.Remove(_liste);
    }
    void ProportionOfEngagedAgent()
    {
        _proportionOfAgentAssigned = _numberAgentAvailable / _numberTotalAgent;

    }

    public void CountNumberAgentAvailable()
    {
        int _numberCounted = 0;
        //Parcour du dictionnaire d'agent
        foreach (KeyValuePair<NavMeshAgent, bool> _agent in _dictionnaireAgent)
        {
            if (_agent.Value)
            {
                _numberCounted++;
            }
        }
        _numberAgentAvailable = _numberCounted;
    }

    void MakeTrace()
    {
        //on tire le raycast là ou pointe la souris
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
           
        //si le raycast a touché quelque chose dans le layer _affectedLayer (ici le terrain)
        if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
        {
            //test sur le booleen de la fonction qui check s'il n'existe pas de point déja présent à cet emplacement
            if (!CheckIfPointAlreadyExistHere(hit.point, _dictionnairePositionTrace))
            {
                //Si le dictionnaire de position n'est pas vide
                if (_dictionnairePositionTrace.Count > 0)
                {
                    //appel de la fonction qui va créer artificiellement des points entre 2 points trop écarté
                    InterpolateNewPointInBetween(hit.point, _dictionnairePositionTrace.Last());
                }
                _numberAgentNeeded++;
                _dictionnairePositionTrace.Add(hit.point, true);
                _listePositionTrace.Add(hit.point);
            }
        }
    }

    bool CheckIfPointAlreadyExistHere(Vector3 _newPoint, Dictionary<Vector3, bool> _newDictionary)
    {
        //Si la liste de dictionnaire de position n'est pas vide
        if(_listeOfListePositionTrace.Count > 0)
        {
            //parcour de la liste de liste
            foreach (List<Vector3> _oldList in _listeOfListePositionTrace)
            {
                //parcour du dictionnaire de position
                foreach (Vector3 _oldPoint in _oldList)
                {
                    //si la distance entre la position du dictionnaire et le nouveau point est inférieur a la taille d'un agent
                    if (Vector3.Distance(_newPoint, _oldPoint) <= _sizeAgent)
                    {
                        return true;
                    }
                }
            }
        }
        //parcour du dictionnaire de position qui est entrain d'être créer
        foreach(KeyValuePair<Vector3, bool> _newPointInNewDictionary in _newDictionary)
        {
            //si la distance entre la position du dictionnaire et le nouveau point est inférieur a la taille d'un agent
            if (Vector3.Distance(_newPoint, _newPointInNewDictionary.Key) <= _sizeAgent)
            {
                return true;
            }
        }
        return false;
    }

    void InterpolateNewPointInBetween(Vector3 _newPoint, KeyValuePair<Vector3, bool> _lastPoint)
    {
        //Création de la variable distance qui contiens la distance entre le dernier point créé et
        //le nouveau point qui va être créer
        float _distanceBetweenNewAndLast = Vector3.Distance(_newPoint, _lastPoint.Key);
        //si la distance qu'on viens de calculer est inférieure à la taille d'un agent
        if (_distanceBetweenNewAndLast > _sizeAgent)
        {
            //On calcul combien d'agent pourrais rentrer entre les 2 points avec nombre = Distance/taille,
            //on prend la valeur et on l'arrondie a l'entier inférieur
            int _numberOfPossiblePosition = Mathf.FloorToInt(_distanceBetweenNewAndLast / (_sizeAgent));
            //On calcule la distance restante (non utilisé par les agents) avec reste = distance - (nombre * taille)
            float _remainingDistance = _distanceBetweenNewAndLast - (_numberOfPossiblePosition * _sizeAgent);

            //si le nombre de position calculé est superieur a 0
            if (_numberOfPossiblePosition > 0)
            {
                //création de la variable direction à partir de la normalisation du vecteur créé avec arrivé - départ
                Vector3 _direction = (_newPoint - _lastPoint.Key).normalized;
                //création de la variable distance entre les agents, créé à partir de la taille de l'agent + le reste de
                //la distance non utilisé divisé par le nombre d'écart entre les agents
                float _distanceBetweenPoints = (_sizeAgent) + (_remainingDistance / (_numberOfPossiblePosition+1));
                //pour chaque nombre de position possible (on commence a 1 pour éviter que la première position créé
                //soit au même endroit l'ancienne)
                for (int i = 1; i < _numberOfPossiblePosition; i++)
                {
                    //La nouvelle position est créé avec :
                    //dernière position + la direction x la distance entre les points x le nombre de la boucle
                    Vector3 _interpolatedPosition = _lastPoint.Key + _direction * (_distanceBetweenPoints * i);
                    //on ajoute la nouvelle position dans le dictionnaire de position avec le booléen a True
                    _dictionnairePositionTrace.Add(_interpolatedPosition, true);
                    _numberAgentNeeded++;
                }
            }
        }
    }

    void ComeToPoint()
    {
        //On initialise une liste de position basé sur le dictionnaire de positions
        List<Vector3> _keysPosition = new List<Vector3>(_dictionnairePositionTrace.Keys);
        //on initialise une liste d'agent basé sur le dictionnaire d'agents
        List<NavMeshAgent> _keysAgent = new List<NavMeshAgent>(_dictionnaireAgent.Keys);
        List<NavMeshAgent> _chosenAgentsList = new List<NavMeshAgent>();
        //pour chaque élément de la liste de position
        for (int i = 0; i < _keysPosition.Count; i++)
        {
            //Création et initialisation de la variable position du point basé sur le point de la liste
            Vector3 _pointPosition = _keysPosition[i];
            //récupération du booléen qui va avec le point qu'on viens de récupérer
            bool _value = _dictionnairePositionTrace[_pointPosition];
            //si le booléen  de la position est True
            if (_value)
            {
                //pour chaque agent de la liste d'agent
                for (int j = 0; j < _keysAgent.Count; j++)
                {
                    //création et initialisation de la variable position de l'agent basé sur l'agent de la liste
                    NavMeshAgent _agentPosition = _keysAgent[j];
                    //récupération du booléen qui va avec l'agent qu'on viens de récupérer
                    bool _agentValue = _dictionnaireAgent[_agentPosition];
                    //si le booléen de l'agent est true
                    if (_agentValue)
                    {
                        //on remplace la variable de l'agent choisis par l'agent actuel
                        _chosenAgent = _agentPosition;
                        _chosenPosition = _agentPosition.transform.position;
                    }
                }
                //Une fois tout les test effectué, on change la destination de l'agent
                _chosenAgent.SetDestination(_pointPosition);
                //On passe donc le booléen de l'agent en false, cela signale qu'il n'est plus disponible
                _dictionnaireAgent[_chosenAgent] = false;
                _chosenAgent.GetComponent<RecognizeItsSelf>()._aviability = false;
                _chosenAgentsList.Add(_chosenAgent);
                //on passe aussi le booléen de la position en false, ce qui signifie qu'il n'est plus disponible
                _dictionnairePositionTrace[_chosenPosition] = false;
            }
        }
        //on appelle la fonction de comptage d'agent disponible pour mettre à jour la variable
        CountNumberAgentAvailable();
        if(_listePositionTrace.Count > 0)
        {
            _dictionnaireOfListeAgent.Add(_chosenAgentsList, TestShape(_listePositionTrace));
        }
    }
    
    //détecte la forme que le groupe d'agent a pris, 0 pour un trait et 1 pour un cercle
    int TestShape(List<Vector3> _liste)
    {
        Vector3 _firstAgent = _liste[0];
        Vector3 _lastAgent = _liste[_liste.Count-1];
        if(Vector3.Distance(_firstAgent, _lastAgent) < espaceEntreAgentPourCercle)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
