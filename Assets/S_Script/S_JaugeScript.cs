using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_JaugeScript : MonoBehaviour
{

    [Header("Jauge")]
    [SerializeField] public float m_JaugeLevel;
    [SerializeField] private float m_MaxJaugeLevel;


    [Header("Paramètres de la jauge")]
    [SerializeField] public float m_JaugeDecreaseRate = 1f;
    [SerializeField] public float m_JaugeIncreaseAmount = 1f;


    [Header("DebugHelper")]
    [SerializeField] public Text m_JaugeLevelText;
    [SerializeField] public Slider m_SliderForJauge;
    


    private void Start()
    {
        m_JaugeLevel = m_MaxJaugeLevel;
        m_JaugeDecreaseRate = 0;
    }

    // Fonction qui permet de choper les cubes 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            // Detruire le GameObject 
            Destroy(other.gameObject);

            // Augementer / Diminuer la Jauge a chaque cube rencontré, pour changer s'il il augemente ou baisse il suffi de changer les signes avec le m_JaugeLevel
            m_JaugeLevel += m_JaugeIncreaseAmount;

        }
    }

    private void Update()
    {
        // appelle de la fonction qui diminue la jauge avec le temps
        DecreaseJaugeOverTime();

        m_JaugeLevelText.text = " Jauge : " + m_JaugeLevel;

        JaugeBar();

    }

    private void DecreaseJaugeOverTime()
    {
        // Diminuer la jauge avec le temps
        m_JaugeLevel -= m_JaugeDecreaseRate * Time.deltaTime;

        // jauge ne descend pas en dessous de zéro
        m_JaugeLevel = Mathf.Clamp(m_JaugeLevel, 0, m_MaxJaugeLevel);

    }


    public void JaugeBar()
    {
        m_SliderForJauge.value = 100 - m_JaugeLevel;
    }

   

}
