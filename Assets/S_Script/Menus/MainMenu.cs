using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameManager _GameManagerScript;
    private void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Level1");
        _GameManagerScript._gameLose = false;
    }
    public void LevelSelect()
    {
        SceneManager.LoadScene("Level_Select");
    }
    public void Option()
    {
        SceneManager.LoadScene("Option_Scene");
    }
    public void Quit_Game()
    {
        Application.Quit();
    }
}
