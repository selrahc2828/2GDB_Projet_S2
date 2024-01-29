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
                //small projectile
                case 1:
                    //if the jauge + the value is still less that a full jauge
                    if (_jaugeScript.actualJauge + _smallValue <= 100)
                    {
                        //add the value to the jauge
                        _jaugeScript.actualJauge += _smallValue;
                    }
                    else
                    {
                        //make the jauge to max
                        _jaugeScript.actualJauge = 100;
                    }
                    break;
                case 2:
                    //if the jauge + the value is still less that a full jauge
                    if (_jaugeScript.actualJauge + _mediumValue <= 100)
                    {
                        //add the value to the jauge
                        _jaugeScript.actualJauge += _mediumValue;
                    }
                    else
                    {
                        //make the jauge to max
                        _jaugeScript.actualJauge = 100;
                    }
                    break;
                case 3:
                    //if the jauge + the value is still less that a full jauge
                    if (_jaugeScript.actualJauge + _largeValue <= 100)
                    {
                        //add the value to the jauge
                        _jaugeScript.actualJauge += _largeValue;
                    }
                    else
                    {
                        //make the jauge to max
                        _jaugeScript.actualJauge = 100;
                    }
                    break;
                default:
                    //normally you can't go here
                    Debug.Log("Invalid number.");
                    break;
            }
        }
    }
}
