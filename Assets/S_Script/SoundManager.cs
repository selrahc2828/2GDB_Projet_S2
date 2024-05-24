using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    
    //Call scripts
    public HeathTowerScript towerLifeScript;
    public AgentToTrace activeAgentInformation;
    public GameManager gameManager;
    
    
    
    //Internal variables
    public float hitPoints;
    public float homewrecker;
    public float buzzkiller;
    public int songstate = 0;
    
    //Fmod parameters
    private static FMOD.Studio.EventInstance levelMusic;
    

    
    void Start()
    {
        //init music
        levelMusic = FMODUnity.RuntimeManager.CreateInstance("event:/OST/level_music");
        levelMusic.start();
        levelMusic.release();
    }
    void Update()
    { 
        // Hit Points
        hitPoints = towerLifeScript.healthPercentageVariable;
        
        // homewrecker
        if (gameManager._numberOfHomeWreckerOnScreen == 0)
        {
            homewrecker = 0f;
        }
        else
        {
            homewrecker = 1f;
        }
        
        //buzzkiller
        if (gameManager._numberOfBuzzKillerOnScreen == 0)
        {
            buzzkiller = 0f;
        }
        else
        {
            buzzkiller = 1f;
        }
        
        //manage transitions
            
        levelMusic.setParameterByName("Hit Points", hitPoints);
        levelMusic.setParameterByName("Homewrecker", homewrecker);
        levelMusic.setParameterByName("Buzzkiller", buzzkiller);
        levelMusic.setParameterByName("Songstate", songstate);

    }
}
