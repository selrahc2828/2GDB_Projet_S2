using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{

    public GameObject _prefabPull;
    public GameObject _prefabPush;
    public GameObject _prefabAgentSpawn;
    public LayerMask _affectedLayer;

    public bool _Pull;
    public bool _Push;
    public bool _AgentSpawn;

    public void Start()
    {
        _Pull = false;
        _Push = false;
        _AgentSpawn = false;
    }


    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0) && _Pull == true) 
        {
            GameObject newCube = Instantiate(_prefabPull, transform.position, Quaternion.identity);
            InfluenceZone influenceScript = newCube.GetComponent<InfluenceZone>();

            if (influenceScript != null)
            {
                influenceScript._Pull = true;
                influenceScript._Push = false;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
            {
                newCube.transform.position = hit.point;
            }
        }

        if (Input.GetMouseButtonDown(0) && _Push == true)
        {
            GameObject newCube = Instantiate(_prefabPush, transform.position, Quaternion.identity);
            InfluenceZone influenceScript = newCube.GetComponent<InfluenceZone>();

            if (influenceScript != null)
            {
                influenceScript._Pull = false;
                influenceScript._Push = true;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
            {
                newCube.transform.position = hit.point;
            }
        }

        if (Input.GetMouseButtonDown(0) && _AgentSpawn == true)
        {
            GameObject newCube = Instantiate(_prefabAgentSpawn, transform.position, Quaternion.identity);
            InfluenceZone influenceScript = newCube.GetComponent<InfluenceZone>();

            if (influenceScript != null)
            {
                influenceScript._Pull = false;
                influenceScript._Push = true;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
            {
                newCube.transform.position = hit.point;
            }
        }
    }

    public void _PushInput()
    {
        _Push = true;
        _Pull= false;
        _AgentSpawn = false;
    }

    public void _PullInput()
    {
        _Push = false;
        _AgentSpawn = false;
        _Pull = true;
    }

    public void _AgentInput()
    {
        _Push= false;
        _AgentSpawn = true;
        _Pull = false;
    }
}
