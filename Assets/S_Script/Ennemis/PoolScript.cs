using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolScript : MonoBehaviour
{
    public bool _infected;
    public void CheckSurrounding()
    {
        _infected = false;
        Collider[] _enemysInZone = Physics.OverlapSphere(transform.position, 10);
        foreach (Collider enemy in _enemysInZone)
        {
            if (enemy.CompareTag("AgentMechant") && enemy.GetComponent<BuzzKiller>() != null)
            {
                _infected = true; 
            }
        }
        if (_infected )
        {
            gameObject.tag = "Infected";
        }
        else
        {
            gameObject.tag = "Untagged";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckSurrounding();
    }
}
