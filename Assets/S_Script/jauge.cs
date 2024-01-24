using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jauge : MonoBehaviour
{
    private float originalScale;
    private float fullJauge;
    public float actualJauge;
    public float _time;
    // Start is called before the first frame update
    void Start()
    {
        _time = 3;
        originalScale = transform.localScale.y;
        fullJauge = 100;
        actualJauge = fullJauge;
    }

    // Update is called once per frame
    void Update()
    {
        if (actualJauge >= 0)
        {
            actualJauge -= Time.deltaTime * 100/_time;
            transform.localScale = new Vector3(transform.localScale.x, originalScale * (actualJauge / 100), transform.localScale.z);
        }        
    }
}
