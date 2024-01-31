using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LifeLimiteDown : MonoBehaviour
{

    private CollisionSpawner _LifeTimeScript;
    public float _MaxTimeRedduction;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
             _LifeTimeScript= other.GetComponent<CollisionSpawner>();
            _LifeTimeScript._MaxTimeBeforeDeath = _MaxTimeRedduction;
        }
    }
}
