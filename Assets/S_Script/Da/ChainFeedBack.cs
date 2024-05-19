using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainFeedBack : MonoBehaviour
{
    public int posOnLine;
    public float burstDuration = 2f;
    public int maxEntityOnLine = 50;
    public AnimationCurve burstCurve;

    public float burst;
    public Color burstIntensity;


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
        float t = Time.time % burstDuration;
        float offset = t / burstDuration * maxEntityOnLine;
        float r = offset - posOnLine;
        return burstCurve.Evaluate(r);
    }
}
