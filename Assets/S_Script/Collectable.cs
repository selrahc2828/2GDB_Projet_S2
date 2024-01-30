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
        //If the object that enter the trigger is the player
        if (other.gameObject.CompareTag("Player"))
        {
            //test for each collectibleType (would probably better to make it a better)
            switch (_collectibleType)
            {
                //small collectable
                case 1:
                    AddToJauge(_smallValue);
                    break;
                //medium collectable
                case 2:
                    AddToJauge(_mediumValue);
                    break;
                //large collectable
                case 3:
                    AddToJauge(_largeValue);
                    break;
                default:
                    //normally you can't go here
                    Debug.Log("Invalid number.");
                    break;
            }
        }
    }

    void AddToJauge(int _value)
    {
        //if the jauge + the value is still less that a full jauge
        if (_jaugeScript.actualJauge + _value <= 100)
        {
            //add the value to the jauge
            _jaugeScript.actualJauge += _value;
        }
        else
        {
            //make the jauge to max
            _jaugeScript.actualJauge = 100;
        }
    }
}
