using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    public GameObject _prefab;
    public LayerMask _affectedLayer;


    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0)) 
        {
            // Raycast to find the point in world space where the mouse was clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //If the point exist within the affected layer
            if (Physics.Raycast(ray, out hit, float.MaxValue, _affectedLayer))
            {
                //Create the Influence Tower at this point
                Instantiate(_prefab, hit.point, Quaternion.identity);
            }
        }
    }
}
