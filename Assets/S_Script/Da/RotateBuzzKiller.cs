using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBuzzKiller : MonoBehaviour
{
    public float rotationSpeed = 100f;

    
    void Update()
    {
       
        float rotationAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAmount, 0);
    }
}
