using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AgentChoise : MonoBehaviour
{
    [Header("Reference")]
    private GameObject[] _agents;
    public AgentFonction[] _AgentComportements;
    public AgentToTrace _TraceScript;

    public Image _Shoot;
    public Image _Slow;

    private Vector3 initialScale;

    private void Start()
    {
        _agents = GameObject.FindGameObjectsWithTag("Agent");
        _AgentComportements = new AgentFonction[_agents.Length];
        for (int i = 0; i < _agents.Length; i++)
        {
            _AgentComportements[i] = _agents[i].GetComponent<AgentFonction>();
        }

        // Sauvegardez l'�chelle initiale de l'image _Shoot
        initialScale = _Shoot.transform.localScale;
    }

    void Update()
    {
        float _scroll = Input.GetAxis("Mouse ScrollWheel");

        for (int i = 0; i < _AgentComportements.Length; i++)
        {
            if (_scroll > 0f)
            {
                if (IsAgentUsable(_AgentComportements[i].GetComponent<NavMeshAgent>()))
                {
                    _AgentComportements[i]._ShootEnemy = true;
                    _AgentComportements[i]._SlowEnemy = false;
                    Debug.Log(_AgentComportements[i]._ShootEnemy);
                }
               
            }

            if (_scroll < 0f)
            {
                if (IsAgentUsable(_AgentComportements[i].GetComponent<NavMeshAgent>()))
                {
                    _AgentComportements[i]._SlowEnemy = true;
                    _AgentComportements[i]._ShootEnemy = false;
                    Debug.Log(_AgentComportements[i]._SlowEnemy);
                }

            }
        }

        //// Update scale Ui
        //UpdateImageScales();
    }

    //void UpdateImageScales()
    //{
       
    //    if (_AgentComportements.Any(agent => IsAgentUsable(agent.GetComponent<NavMeshAgent>()) && !agent._ShootEnemy))
    //    {
    //        _Shoot.transform.localScale = initialScale * 0.5f; 
    //    }
    //    else
    //    {
    //        _Shoot.transform.localScale = initialScale; 
    //    }

      
    //    if (_AgentComportements.Any(agent => IsAgentUsable(agent.GetComponent<NavMeshAgent>()) && !agent._SlowEnemy))
    //    {
    //        _Slow.transform.localScale = initialScale * 0.5f; 
    //    }
    //    else
    //    {
    //        _Slow.transform.localScale = initialScale; 
    //    }
    //}


    public bool IsAgentUsable(NavMeshAgent agent)
    {
        if (_TraceScript._dictionnaireAgent.ContainsKey(agent))
        {
            return _TraceScript._dictionnaireAgent[agent];
        }
        else
        {
            return false;
        }
    }
}
