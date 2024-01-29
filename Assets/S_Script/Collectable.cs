using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public jauge _jaugeScript;
    public int _collectibleType = 0;

    private int _smallValue = 5;
    private int _mediumValue = 15;
    private int _largeValue = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (_collectibleType)
            {
                case 1:
                    if (_jaugeScript.actualJauge + _smallValue <= 100)
                    {
                        _jaugeScript.actualJauge += _smallValue;
                    }
                    else
                    {
                        _jaugeScript.actualJauge = 100;
                    }
                    break;
                case 2:
                    if (_jaugeScript.actualJauge + _mediumValue <= 100)
                    {
                        _jaugeScript.actualJauge += _mediumValue;
                    }
                    else
                    {
                        _jaugeScript.actualJauge = 100;
                    }
                    break;
                case 3:
                    if (_jaugeScript.actualJauge + _largeValue <= 100)
                    {
                        _jaugeScript.actualJauge += _largeValue;
                    }
                    else
                    {
                        _jaugeScript.actualJauge = 100;
                    }
                    break;
                default:
                    Debug.Log("Invalid number.");
                    break;
            }
        }
    }
}
