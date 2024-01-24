using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownZone : MonoBehaviour
{
    public float _time;
    public jauge jaugeScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (jaugeScript.actualJauge <= 0)
            {
                other.transform.position += other.transform.forward * 150 * Time.deltaTime;
            }
            else
            {
                jaugeScript.actualJauge -= Time.deltaTime * 100 / _time;
            }
        }
    }
}
