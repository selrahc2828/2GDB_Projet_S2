using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    //Call scripts
    public HeathTowerScript TowerLifeScript;
    public PoolAnim poolEngage;
    public RecognizeItsSelf activeAgent;
    
    
    
    //Internal variables
    public float engagement;
    public int hitPoints;
    public bool isEngaged;
    public int distanceToTower;
    
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
        //Variable setup
        
        //Hit Points
        hitPoints = TowerLifeScript._CurrentHealth / TowerLifeScript._MaxHealth;
        Debug.Log(hitPoints);
        
        // Engagement
        switch (activeAgent._aviability)
        {
            case true:
                isEngaged = false;
                break;
            case false:
                isEngaged = true;
                break;
        }
        
        if (isEngaged)
        {
            switch (poolEngage._isInTrigger)
            {
             case true:
                 engagement = 2f;
                 break;
             case false:
                 engagement = 1f;
                 break;
            }
        }
        else
        {
            engagement = 0f;
        }
        
        //Distance to Tower
        
        
        // Music Manager
        levelMusic.setParameterByName("Engagement", engagement);
        levelMusic.setParameterByName("Hit Points", hitPoints);
        
        //levelMusic.setParameterByName("Enemy_to_tower_distance",)


    }
}
