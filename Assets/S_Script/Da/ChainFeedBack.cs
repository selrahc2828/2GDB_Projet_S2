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
    public float burstTotal;

    public Color burstIntensity;
    [ColorUsage(false, true)]
    public Color burstBlueColor;
    [ColorUsage(false, true)]
    public Color burstpinkColor;
    public Color burstwhiteColor;


    // Update is called once per frame
    void Update()
    {
        burst = 10 * GetBurstIntensity(posOnLine);
        burstwhiteColor = new Color(burst, burst, burst, 1f);
        burst1 = 10 * GetBurstIntensity(posOnLinePool1);

        burst2 = 10 * GetBurstIntensity(posOnLinePool2);
        burst3 = 10 * GetBurstIntensity(posOnLinePool3);
        burst4 = 10 * GetBurstIntensity(posOnLinePool4);

        burstIntensity = burstwhiteColor + burst1 * burstBlueColor + (burst2 + burst3 + burst4) * burstpinkColor;
        //burstTotal = burst + burst1 + burst2 + burst3 + burst4;
        //burstIntensity = new Color(burstTotal, burstTotal, burstTotal, 1f);
        //burstIntensity = new Color(burst + (burst2 + burst3 + burst4) * 0.28f, burst + (burst1 * 0.27f), burst + (burst1 * 0.74f) + (burst2 + burst3 + burst4) * 0.75f, 1f);
    }
    public void SetPosOnLine(GameObject pool, int number)
    {
        switch (pool.name)
        {
            case "Tower":
                posOnLine = number; 
                break;
            case "Pool1 - slow":
                posOnLinePool1 = number; 
                break;
            case "Pool2 - damage":
                posOnLinePool2 = number;
                break;
            case "Pool3 - damage":
                posOnLinePool3 = number;
                break;
            case "Pool4 - damage":
                posOnLinePool4 = number;
                break;
            default:
                break;
        }
    }
    public void UpdateFeedback(float i)
    {
        //GetComponent<Renderer>().material.color = new Color(i, i, i, 1f);
    }

    private float GetBurstIntensity(int position)
    {
        float t = Time.time % burstDuration;
        float offset = t / burstDuration * maxEntityOnLine;
        float r = offset - position;

        return burstCurve.Evaluate(r);
    }
}
