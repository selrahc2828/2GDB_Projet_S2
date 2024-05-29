using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    
    //Call scripts
    public HeathTowerScript towerLifeScript;
    public GameManager gameManager;
    
    
    
    //Internal variables
    public float hitPoints;
    public float homewrecker;
    public bool hwExisted = false; //Verify if the homewrecker existed once.
    
    public float buzzkiller;
    public bool bkExisted = false; //Verify if the buzz killer existed once.
    
    public int songstate = 0;
    
    //Fmod parameters
    private static FMOD.Studio.EventInstance levelMusic;
    public EventReference bkDead;
    public EventReference hwDead;
    
    
    
    //Transition Timer
    private float _transTimer = 0f;
    private float _beat = 2f;
    private bool _isTransitioning = false;
    
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
        if (_isTransitioning)
        {
            _transTimer += Time.deltaTime;
        }

        if (_transTimer >= _beat)
        {
            _isTransitioning = false;
            songstate = 0;
            _transTimer = 0;
        }
        
        
        // set parameters
        levelMusic.setParameterByName("Hit Points", hitPoints);
        levelMusic.setParameterByName("Homewrecker", homewrecker);
        levelMusic.setParameterByName("Buzzkiller", buzzkiller);
        levelMusic.setParameterByName("Songstate", songstate);
        
        //EndGame

        if (hitPoints <= 0)
        {
            StopAllPlayerEvents();
        }
        
    }

    void StopAllPlayerEvents()
    {
        // FMOD.Studio.Bus playerBus = FMODUnity.RuntimeManager.GetBus("bus:/player");
        // playerBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    public void halfLife()
    {
        songstate = 1;
        _isTransitioning = true;

    }

    public void EnterBuzzkill()
    {
        songstate = 1;
        _isTransitioning = true;
        bkExisted = true;
    }

    public void ExitBuzzkill()
    {
        // if (bkExisted)
        // {
        //
        // }
    }

    public void EnterHomewreck()
    {
        songstate = 1;
        _isTransitioning = true;
        hwExisted = true;
    }

    public void ExitHomewreck()
    {

        // if (hwExisted)
        // {
        //     FMODUnity.RuntimeManager.PlayOneShot(hwDead);
        // }
    }
}
