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
    public  Dictionary<NavMeshAgent, bool> _listeAgent;

    List<Dictionary<Vector3, bool>> _listeOfListePositionTrace;
    Dictionary<List<NavMeshAgent>, int> _dictionnaireOfListeAgent;
    public int _numberAgentAvailable;
    public GameObject _parentAgent;

    public NavMeshAgent _chosenAgent;
    private Vector3 _chosenPosition;
    private float _sizeAgent;

    public Text _AgentDispo;


    // Start is called before the first frame update
    void Start()
    {
        // taille de l'agennt (diametre)
        _sizeAgent = 1f;
        //Dictionnaire dans lequel je stoque des position et un bool�en par position
        _listePositionTrace = new Dictionary<Vector3, bool>();
        //Dictionnaire dans lequel je stoque tout les agents et un bool�en par agent
        _listeAgent = new Dictionary<NavMeshAgent, bool>();
        //Liste dans laquelle je stoque les dictionnaires de position
        _listeOfListePositionTrace = new List<Dictionary<Vector3, bool>>();
        _dictionnaireOfListeAgent = new Dictionary<List<NavMeshAgent>, int>();

        // V�rifier si l'objet parent a �t� attribu�
        if (_parentAgent != null)
        {
            // Parcourir tous les enfants de l'objet parent
            foreach (Transform _agentTransform in _parentAgent.transform)
            {
                // V�rifier si l'enfant poss�de un composant NavMeshAgent
                NavMeshAgent _agent = _agentTransform.GetComponent<NavMeshAgent>();
                if (_agent != null)
                { 
                    // Ajouter le NavMeshAgent au dictionnaire avec son nom comme cl�
                    _listeAgent.Add(_agent, true);
                }
            }
        }
        else
        {
            Debug.LogError("Objet parent non d�fini !");
        }
        //Appel de la fonction pour cr�er la variable du nombre d'agent disponible � un instant T
        CountNumberAgentAvailable();
    }

    // Update is called once per frame
    void Update()
    {
        // Check du click gauche de la souris
        if (Input.GetMouseButton(0))
        {
            //Appel de la fonction pour cr�er les points de position sur le trac� de la souris
            MakeTrace();
        }
        //Test si le bouton gauche de la souris est relev�
        if (Input.GetMouseButtonUp(0))
        {
            //Ajout du dictionnaire de position qui viens d'�tre cr�er dans la liste de dictionnaire
            _listeOfListePositionTrace.Add(_listePositionTrace);
            //Appel de la fonction pour changer la destination des agents
            ComeToPoint();
            //On vide la liste de position pour pouvoir s'en resservir au prochain trac�
            _listePositionTrace.Clear();
        }

        _AgentDispo.text = "Agent Disponible : " + _numberAgentAvailable;
    }

    //outil de debug
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 _position in _listePositionTrace.Keys)
        {
            Gizmos.DrawWireSphere(_position, 5);
            Gizmos.DrawWireSphere(_position, 0.5f);
        }
    }*/

    void CountNumberAgentAvailable()
    {
        //initialisation de la variable a 0
        _numberAgentAvailable = 0;
        //Parcour du dictionnaire d'agent
        foreach (KeyValuePair<NavMeshAgent, bool> _agent in _listeAgent)
        {
            //si le bool�en de l'agent est en True
            if (_agent.Value)
            {
                //on incr�mente la variable du nombre d'agent disponible
                _numberAgentAvailable++;
            }
        }
    }

    void MakeTrace()
    {
        //on tire le raycast l� ou pointe la souris
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //on cr�e la variable hit
        RaycastHit hit;
           
        //si le raycast a touch� quelque chose dans le layer _affectedLayer (ici le terrain)
        if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
        {
            Debug.Log(CheckIfPointAlreadyExistHere(hit.point, _listePositionTrace));
            //Si il n'existe pas de point d�ja pr�sent � cet emplacement
            if (!CheckIfPointAlreadyExistHere(hit.point, _listePositionTrace))
            {
                //Si le dictionnaire de position n'est pas vide
                if (_listePositionTrace.Count > 0)
                {
                    //appel de la fonction qui va cr�er artificiellement des points entre 2 points trop �cart�
                    InterpolateNewPointInBetween(hit.point, _listePositionTrace.Last());
                    //On ajoute le point trouv� par le raycast dans le dictionnaire de points avec le bool�en en true
                    _listePositionTrace.Add(hit.point, true);
                }
                //Si le dictionnaire de position est vide
                else
                {
                    //On ajoute le point trouv� par le raycast dans le dictionnaire de point avec le bool�en en true
                    _listePositionTrace.Add(hit.point, true);
                }
            }
        }
    }

    bool CheckIfPointAlreadyExistHere(Vector3 _newPoint, Dictionary<Vector3, bool> _newDictionary)
    {
        //Si la liste de dictionnaire de position n'est pas vide
        if(_listeOfListePositionTrace.Count > 0)
        {
            //parcour de la liste de dictionnaire
            foreach (Dictionary<Vector3, bool> _oldDictionary in _listeOfListePositionTrace)
            {
                //parcour du dictionnaire de position
                foreach (KeyValuePair<Vector3, bool> _oldPoint in _oldDictionary)
                {
                    //si la distance entre la position du dictionnaire et le nouveau point est inf�rieur a la taille d'un agent
                    if (Vector3.Distance(_newPoint, _oldPoint.Key) <= _sizeAgent)
                    {
                        //la fonction renvois True
                        return true;
                    }
                }
            }
        }
        //parcour du dictionnaire de position qui est entrain d'�tre cr�er
        foreach(KeyValuePair<Vector3, bool> _newPointInNewDictionary in _newDictionary)
        {
            //si la distance entre la position du dictionnaire et le nouveau point est inf�rieur a la taille d'un agent
            if (Vector3.Distance(_newPoint, _newPointInNewDictionary.Key) <= _sizeAgent)
            {
                //la fonction renvois True
                return true;
            }
        }
        //la fonction renvois False
        return false;
    }

    void InterpolateNewPointInBetween(Vector3 _newPoint, KeyValuePair<Vector3, bool> _lastPoint)
    {
        //Cr�ation de la variable distance qui contiens la distance entre le dernier point cr�� et
        //le nouveau point qui va �tre cr�er
        float _distanceBetweenNewAndLast = Vector3.Distance(_newPoint, _lastPoint.Key);
        //si la distance qu'on viens de calculer est inf�rieure � la taille d'un agent
        if (_distanceBetweenNewAndLast > _sizeAgent)
        {
            //On calcul combien d'agent pourrais rentrer entre les 2 points avec nombre = Distance/taille,
            //on prend la valeur et on l'arrondie a l'entier inf�rieur
            int _numberOfPossiblePosition = Mathf.FloorToInt(_distanceBetweenNewAndLast / (_sizeAgent));
            //On calcule la distance restante (non utilis� par les agents) avec reste = distance - (nombre * taille)
            float _remainingDistance = _distanceBetweenNewAndLast - (_numberOfPossiblePosition * _sizeAgent);

            //si le nombre de position calcul� est superieur a 0
            if (_numberOfPossiblePosition > 0)
            {
                //cr�ation de la variable direction � partir de la normalisation du vecteur cr�� avec arriv� - d�part
                Vector3 _direction = (_newPoint - _lastPoint.Key).normalized;
                //cr�ation de la variable distance entre les agents, cr�� � partir de la taille de l'agent + le reste de
                //la distance non utilis� divis� par le nombre d'�cart entre les agents
                float _distanceBetweenPoints = (_sizeAgent) + (_remainingDistance / (_numberOfPossiblePosition+1));
                //pour chaque nombre de position possible (on commence a 1 pour �viter que la premi�re position cr��
                //soit au m�me endroit l'ancienne)
                for (int i = 1; i < _numberOfPossiblePosition; i++)
                {
                    //La nouvelle position est cr�� avec :
                    //derni�re position + la direction x la distance entre les points x le nombre de la boucle
                    Vector3 _interpolatedPosition = _lastPoint.Key + _direction * (_distanceBetweenPoints * i);
                    //on ajoute la nouvelle position dans le dictionnaire de position avec le bool�en a True
                    _listePositionTrace.Add(_interpolatedPosition, true);
                }
            }
        }
    }

    void ComeToPoint()
    {
        //On initialise une liste de position bas� sur le dictionnaire de positions
        List<Vector3> _keysPosition = new List<Vector3>(_listePositionTrace.Keys);
        //on initialise une liste d'agent bas� sur le dictionnaire d'agents
        List<NavMeshAgent> _keysAgent = new List<NavMeshAgent>(_listeAgent.Keys);
        List<NavMeshAgent> _chosenAgentsList = new List<NavMeshAgent>();
        //pour chaque �l�ment de la liste de position
        for (int i = 0; i < _keysPosition.Count; i++)
        {
            //Cr�ation et initialisation de la variable position du point bas� sur le point de la liste
            Vector3 _pointPosition = _keysPosition[i];
            //r�cup�ration du bool�en qui va avec le point qu'on viens de r�cup�rer
            bool _value = _listePositionTrace[_pointPosition];
            //si le bool�en  de la position est True
            if (_value)
            {
                //on initialise la variable distance minimum a 0
                float _distanceMin = 0;
                //pour chaque agent de la liste d'agent
                for (int j = 0; j < _keysAgent.Count; j++)
                {
                    //cr�ation et initialisation de la variable position de l'agent bas� sur l'agent de la liste
                    NavMeshAgent _agentPosition = _keysAgent[j];
                    //r�cup�ration du bool�en qui va avec l'agent qu'on viens de r�cup�rer
                    bool _agentValue = _listeAgent[_agentPosition];
                    //si le bool�en de l'agent est true
                    if (_agentValue)
                    {
                        //si la variable distance n'est pas 0
                        if (_distanceMin != 0)
                        {
                            //si la distance entre la position du point et le position de l'agent
                            //est inf�rieur � la variable distance actuelle
                            if (Vector3.Distance(_pointPosition, _agentPosition.transform.position) < _distanceMin)
                            {
                                //On remplace la variable distance par la distance qu'on viens de calculer
                                _distanceMin = Vector3.Distance(_pointPosition, _agentPosition.transform.position);
                                //on remplace la variable de l'agent choisis par l'agent actuel
                                _chosenAgent = _agentPosition;
                                //on remplace la variable de la position par la position actuelle
                                _chosenPosition = _pointPosition;
                            }
                        }
                        //si la variable de distance est 0
                        else
                        {
                            //On remplace la variable distance par la distance qu'on viens de calculer
                            _distanceMin = Vector3.Distance(_pointPosition, _agentPosition.transform.position);
                            //on remplace la variable de l'agent choisis par l'agent actuel
                            _chosenAgent = _agentPosition;
                            //on remplace la variable de la position par la position actuelle
                            _chosenPosition = _pointPosition;
                        }
                    }
                }
                //Une fois tout les test effectu�, la variable distance est la plus petite possible et l'agent choisi est
                //le plus proche de cette position. Du coup on change la destination de l'agent
                _chosenAgent.SetDestination(_chosenPosition);
                //On passe donc le bool�en de l'agent en false, cela signale qu'il n'est plus disponible
                _listeAgent[_chosenAgent] = false;
                _chosenAgentsList.Add(_chosenAgent);
                //on passe aussi le bool�en de la position en false, ce qui signifie qu'il n'est plus disponible
                _listePositionTrace[_chosenPosition] = false;
            }
        }
        //on appelle la fonction de comptage d'agent disponible pour mettre � jour la variable
        CountNumberAgentAvailable();
        _dictionnaireOfListeAgent.Add(_chosenAgentsList, TestShape(_chosenAgentsList));
    }

    int TestShape(List<NavMeshAgent> _liste)
    {
        foreach(NavMeshAgent _agent in _liste)
        { 
            
        }
        return 0;
    }
}
