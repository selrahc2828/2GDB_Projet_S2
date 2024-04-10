using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageToTower : MonoBehaviour
{
    [Header("Reference")]
    public HeathTowerScript _HealtTower;
    public GameObject _Tower;

    public int _damageAmount = 10; 
    public float _cooldown = 1f; 
    private bool _canDamage = true;


    private void Start()
    {
        // Get the component HeathScript to the present tower when the agent is created 
        _Tower = GameObject.Find("Tower");
        _HealtTower = _Tower.GetComponent<HeathTowerScript>();
    }
    

    private void OnTriggerStay(Collider other)
    {
        // do a check to the collider and the booleen
        if (other.CompareTag("Tower") &&_canDamage)
        {
            // call the fonction "TakeDamage" of the HeatlTowerScript 
            _HealtTower.TakeDamage(_damageAmount);
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
