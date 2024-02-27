using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToAgent : MonoBehaviour
{
    [Header("Reference")]
    public HeathAgent _HealtAgent;


    public int _damageAmount = 10;
    public float _cooldown = 1f;
    private bool _canDamage = true;


    private void OnCollisionEnter(Collision collision)
    {
        _HealtAgent = collision.gameObject.GetComponent<HeathAgent>();

        // do a check to the collider and the booleen
        if (collision.gameObject.CompareTag("Agent") && _canDamage)
        {
            // call the fonction "TakeDamage" of the HealtAgentScript 
            _HealtAgent.TakeDamage(_damageAmount);
            // Change bool 
            _canDamage = false;
            // start a cooldown
            StartCoroutine(CooldownCoroutine());
        }
    }


    private IEnumerator CooldownCoroutine()
    {
        // return the cooldown when finish so that wait for the time in variable _cooldown
        yield return new WaitForSeconds(_cooldown);
        _canDamage = true;
    }
}
