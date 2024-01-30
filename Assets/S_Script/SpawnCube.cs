using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{

    public GameObject _prefabCube1;
    public GameObject _prefabCube2;
    public LayerMask _affectedLayer;


    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0)) 
        {
            GameObject newCube = Instantiate(_prefabCube1, transform.position, Quaternion.identity);
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

        if (Input.GetMouseButtonDown(1))
        {
            GameObject newCube = Instantiate(_prefabCube2, transform.position, Quaternion.identity);
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
}
