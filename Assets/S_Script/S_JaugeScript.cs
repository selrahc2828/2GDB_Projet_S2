using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_JaugeScript : MonoBehaviour
{
    [Header("Jauge")]
    [SerializeField] public int m_JaugeLevel;


    [Header("DebugHelper")]
    [SerializeField] public Text m_JaugeLevelText;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            // Detruire le GameObject 
            Destroy(other.gameObject);

            // Augementer / Diminuer la Jauge a chaque cube rencontré, pour changer s'il il augemente ou baisse il suffi de changer les signes avec le m_JaugeLevel
            m_JaugeLevel++;

            // Observer le changement en text
            m_JaugeLevelText.text = " JaugeLevel : " + m_JaugeLevel;
        }
    }
}
