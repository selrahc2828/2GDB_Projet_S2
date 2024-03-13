using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private int levelSelected = 0;
    public GameManager _GameManagerScript;

    private void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }
    public void SelectLevel1()
    {
        levelSelected = 1;
    }
    public void SelectLevel2()
    {
        levelSelected = 2;
    }
    public void SelectLevel3()
    {
        levelSelected = 3;
    }
    public void SelectLevel4()
    {
        levelSelected = 4;
    }
    public void SelectLevel5()
    {
        levelSelected = 5;
    }
    public void SelectLevel6()
    {
        levelSelected = 6;
    }
    public void PlayLevel()
    {
        switch (levelSelected)
        {
            case 1:
                SceneManager.LoadScene("Level1");
                _GameManagerScript._gameLose = false;
                break;
            case 2:
                SceneManager.LoadScene("Level2");
                _GameManagerScript._gameLose = false;
                break;
            case 3:
                SceneManager.LoadScene("Level3");
                _GameManagerScript._gameLose = false;
                break;
            case 4:
                SceneManager.LoadScene("Level4");
                _GameManagerScript._gameLose = false;
                break;
            case 5:
                SceneManager.LoadScene("Level5");
                _GameManagerScript._gameLose = false;
                break;
            case 6:
                SceneManager.LoadScene("Level6");
                _GameManagerScript._gameLose = false;
                break;
            default:
                break;
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
