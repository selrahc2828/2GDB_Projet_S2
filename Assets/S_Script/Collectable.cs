using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public jauge jaugeScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (jaugeScript.actualJauge + 30 <= 100)
            {
                jaugeScript.actualJauge += 30;
            }
            else
            {
                jaugeScript.actualJauge = 100;
            }
        }
    }
}
