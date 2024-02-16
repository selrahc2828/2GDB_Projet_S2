using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentChoise : MonoBehaviour
{
    [Header("Reference")]
    private GameObject[] _agents; 
    private AgentFonction[] _AgentComportements; 

    public Image _Shoot;
    public Image _Slow;

    private void Start()
    {
       
        _agents = GameObject.FindGameObjectsWithTag("Agent");
        _AgentComportements = new AgentFonction[_agents.Length];
        for (int i = 0; i < _agents.Length; i++)
        {
            _AgentComportements[i] = _agents[i].GetComponent<AgentFonction>();
        }
    }


    void Update()
    {
        float _scroll = Input.GetAxis("Mouse ScrollWheel");

        for (int i = 0; i < _AgentComportements.Length; i++)
        {
            if (_scroll > 0f)
            {
                _AgentComportements[i]._ShootEnemy = true;
                _AgentComportements[i]._SlowEnemy = false;
                Debug.Log(_AgentComportements[i]._ShootEnemy);
            }

            if (_scroll < 0f)
            {
                _AgentComportements[i]._SlowEnemy = true;
                _AgentComportements[i]._ShootEnemy = false;
                Debug.Log(_AgentComportements[i]._SlowEnemy);
            }
        }
    }

}
