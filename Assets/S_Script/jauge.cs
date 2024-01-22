using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jauge : MonoBehaviour
{
    private float originalScale;
    private float fullJauge;
    public float actualJauge;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale.y;
        fullJauge = 100;
        actualJauge = fullJauge;
    }

    // Update is called once per frame
    void Update()
    {
        if (actualJauge >= 0)
        {
            actualJauge -= Time.deltaTime * 30;
            transform.localScale = new Vector3(transform.localScale.x, originalScale * (actualJauge / 100), transform.localScale.z);
        }        
    }
}
