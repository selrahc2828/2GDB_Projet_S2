using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundManager : MonoBehaviour
{
    private static FMOD.Studio.EventInstance menuMusic;
    // Start is called before the first frame update
    void Start()
    {
        
        menuMusic = FMODUnity.RuntimeManager.CreateInstance("event:/OST/menu_music");

        menuMusic.start();
        menuMusic.release();
    }
    
}
