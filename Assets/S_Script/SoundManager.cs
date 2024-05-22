using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    
    //Call scripts
    public HeathTowerScript towerLifeScript;
    public AgentToTrace activeAgentInformation;
    public GameManager gameManager;
    
    
    
    //Internal variables
    public float engagement;
    public float hitPoints;
    public float pooled;
    public float homewrecker;
    public float buzzkiller;
    public float enemyQuanity;
    
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
        // Agents engaged

        engagement = 1 - activeAgentInformation._proportionOfAgentAssigned;
        
        // Hit Points
        
        hitPoints = towerLifeScript.healthPercentageVariable;
        
        // Pooled Agents
        pooled = 0f;
        
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
        
        //distance to tower

        if (gameManager._numberOfBaseEnemyOnScreen == 0)
        {
            enemyQuanity = 0f;
        }
        else
        {
            enemyQuanity = 1f;
        }
        
        
        levelMusic.setParameterByName("Engagement", engagement);
        levelMusic.setParameterByName("Hit Points", hitPoints);
        levelMusic.setParameterByName("Pooled", pooled);
        levelMusic.setParameterByName("Homewrecker", homewrecker);
        levelMusic.setParameterByName("Buzzkiller", buzzkiller);
        levelMusic.setParameterByName("EnemyQuantity", enemyQuanity);
        


    }
}
