using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public jauge jaugeScript;
    public float m_JaugeAdd;
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {


            if (jaugeScript.actualJauge + 30 <= 100)
            {
                jaugeScript.actualJauge += m_JaugeAdd;
            }
            else
            {
                jaugeScript.actualJauge = 100;
            }
        }
    }
}
