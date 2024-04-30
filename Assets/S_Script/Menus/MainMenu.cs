using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public GameManager _GameManagerScript;
    public GameObject _PanelLevelSelec;
    public GameObject _PanelOption;


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
        _PanelLevelSelec.SetActive(true);
    }
    public void Option()
    {
        _PanelOption.SetActive(true);
    }
    public void Quit_Game()
    {
        Application.Quit();
    }
}
