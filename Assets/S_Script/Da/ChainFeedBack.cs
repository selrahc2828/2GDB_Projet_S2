using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ChainFeedBack : MonoBehaviour
{
    public int posOnLine;
    public int posOnLinePool1;
    public int posOnLinePool2;
    public int posOnLinePool3;
    public int posOnLinePool4;


    public float burstDuration = 2f;
    public int maxEntityOnLine = 50;
    public AnimationCurve burstCurve;

    public float burst;
    public float burst1;
    public float burst2;
    public float burst3;
    public float burst4;

    public Color burstIntensity;
    public Color _PoolSlowColor;
    public Color _PoolDegatsColor;


    // Update is called once per frame
    void Update()
    {
        burst = 10 * GetBurstIntensity();
        burstIntensity = new Color(burst, burst, burst, 1f);

        burst1 = 10* GetBurstIntensityPool1();
        _PoolSlowColor = new Color(burst1, burst1, burst1, 1f);

        burst2 = 10* GetBurstIntensityPool2();
        _PoolDegatsColor = new Color(burst2, burst2, burst2, 1f);

        burst3 = 10* GetBurstIntensityPool3();
        _PoolDegatsColor = new Color(burst3, burst3, burst3, 1f);

        burst4 = 10* GetBurstIntensityPool4();
        _PoolDegatsColor = new Color(burst4, burst4, burst4, 1f);
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

    private float GetBurstIntensityPool1()
    {
        float t = Time.time % burstDuration;
        float offset = t / burstDuration * maxEntityOnLine;
        float r = offset - posOnLinePool1;

        return burstCurve.Evaluate(r);
    }

    private float GetBurstIntensityPool2()
    {
        float t = Time.time % burstDuration;
        float offset = t / burstDuration * maxEntityOnLine;
        float r = offset - posOnLinePool2;

        return burstCurve.Evaluate(r);
    }

    private float GetBurstIntensityPool3()
    {
        float t = Time.time % burstDuration;
        float offset = t / burstDuration * maxEntityOnLine;
        float r = offset - posOnLinePool3;

        return burstCurve.Evaluate(r);
    }

    private float GetBurstIntensityPool4()
    {
        float t = Time.time % burstDuration;
        float offset = t / burstDuration * maxEntityOnLine;
        float r = offset - posOnLinePool4;

        return burstCurve.Evaluate(r);
    }

}
