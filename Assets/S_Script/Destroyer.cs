using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float _speed;

    void Start()
    {
        Destroy(gameObject, _speed);
    }
}
