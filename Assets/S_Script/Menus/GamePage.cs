using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePage : MonoBehaviour
{
    public GameManager _GameManagerScript;

    private void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }
    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        _GameManagerScript._gameLose = false;
    }
}
