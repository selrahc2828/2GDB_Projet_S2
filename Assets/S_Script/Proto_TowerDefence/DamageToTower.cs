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


    private void Awake()
    {
        _HealtTower = _Tower.GetComponent<HeathTowerScript>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_canDamage)
        {
            Debug.Log("DamageDeal");
            _HealtTower.TakeDamage(_damageAmount);
            _canDamage = false; 
            StartCoroutine(CooldownCoroutine());
        }
        
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(_cooldown);
        _canDamage = true; 
    }
}
