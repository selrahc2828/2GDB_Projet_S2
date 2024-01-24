using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jauge : MonoBehaviour
{

    [Header("Reference")]
    [SerializeField] public Slider m_JaugeSlider;
    
   
    private float fullJauge;
    public float actualJauge;
    public float _time;
    // Start is called before the first frame update
    void Start()
    {
        
        fullJauge = 100;
        actualJauge = fullJauge;
    }

    // Update is called once per frame
    void Update()
    {
        DecreaseJauge();
        UpdateSliderUI();
            
    }

    public void DecreaseJauge()
    {
        if (actualJauge >= 0)
        {
            actualJauge -= Time.deltaTime * 100 / _time;
        }
    }

    public void UpdateSliderUI()
    {
        m_JaugeSlider.value = 100 - actualJauge;
    }
}
