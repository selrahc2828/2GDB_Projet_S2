using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NewGame()
    {
        //SceneManager.LoadScene("Level1");
    }
    public void LevelSelect()
    {
        //ceneManager.LoadScene("Level_Select");
    }
    public void Option()
    {
        //ceneManager.LoadScene("Option_Scene");
    }
    public void Quit_Game()
    {
        Application.Quit();
    }
}
