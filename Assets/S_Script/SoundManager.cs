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
    public AgentToTrace activeAgentInformation;
    public GameManager gameManager;
    
    
    
    //Internal variables
    public float hitPoints;
    public float homewrecker;
    public float buzzkiller;
    public int songstate = 0;
    
    //Fmod parameters
    private static FMOD.Studio.EventInstance levelMusic;
    
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

        if (_transTimer >= _beat & _transTimer < 0)
        {
            _isTransitioning = false;
            songstate = 0;
            _transTimer = 0;
            Debug.Log(songstate);
        }
        
        
        // set parameters
        levelMusic.setParameterByName("Hit Points", hitPoints);
        levelMusic.setParameterByName("Homewrecker", homewrecker);
        levelMusic.setParameterByName("Buzzkiller", buzzkiller);
        levelMusic.setParameterByName("Songstate", songstate);
        
        
        
    }

    public void halfLife()
    {
        songstate = 1;
        _isTransitioning = true;
        Debug.Log("half life"+songstate);

    }

    public void EnterBuzzkill()
    {
        songstate = 1;
        _isTransitioning = true;
        Debug.Log("enter buzz"+songstate);
    }

    public void ExitBuzzkill()
    {
        songstate = 1;
        _isTransitioning = true;
        Debug.Log("exit buzz"+songstate);
    }

    public void EnterHomewreck()
    {
        songstate = 1;
        _isTransitioning = true;
        Debug.Log("enter HW"+songstate);
    }

    public void ExitHomewreck()
    {
        songstate = 1;
        _isTransitioning = true;
        Debug.Log("exit HW"+songstate);
    }
}
