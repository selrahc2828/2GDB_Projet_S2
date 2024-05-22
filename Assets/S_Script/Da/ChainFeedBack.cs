using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ChainFeedBack : MonoBehaviour
{
    public int posOnLine;
    public float burstDuration = 2f;
    public int maxEntityOnLine = 50;
    public AnimationCurve burstCurve;

    public float burst;
    public Color burstIntensity;
    public Color _PoolSlowColor;
    public Color _PoolD�gatsColor;


    // Update is called once per frame
    void Update()
    {
        burst = 10* GetBurstIntensity();
        burstIntensity = new Color(burst, burst, burst, 1f);
    }

    public void UpdateFeedback(float i)
    {
        //GetComponent<Renderer>().material.color = new Color(i, i, i, 1f);
    }

    private float GetBurstIntensity()
    {
        //t est le temps actuel modulo burstDuration, ce qui boucle le temps dans une p�riode de burstDuration secondes.
        float t = Time.time % burstDuration;

        //offset est une valeur proportionnelle au temps actuel, mise � l'�chelle pour correspondre au nombre maximum d'entit�s.
        float offset = t / burstDuration * maxEntityOnLine;

        //r est la diff�rence entre offset et la position de l'objet (posOnLine).
        float r = offset - posOnLine;

        //burstCurve.Evaluate(r) utilise cette diff�rence pour obtenir une valeur d'intensit� � partir de burstCurve.
        return burstCurve.Evaluate(r);
    }
}
