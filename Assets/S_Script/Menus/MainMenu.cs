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

    [Header("Activation/Desactivation")]
    public AgentToTrace _TraceScript;
    public GameObject _RendererTrail;


    [Header("Button")]
    public GameObject PlayButton;
    public GameObject LevelSelectButton;
    public GameObject OptionSelectButton;
    public GameObject QuitButton;



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

        if (_PanelLevelSelec == true)
        {
            PlayButton.SetActive(false);
            LevelSelectButton.SetActive(false);
            QuitButton.SetActive(false);
            OptionSelectButton.SetActive(false);

            _TraceScript.enabled = false;
            _RendererTrail.SetActive(false);
        }
    }
    public void Option()
    {
        _PanelOption.SetActive(true);

        if (_PanelOption == true)
        {
            PlayButton.SetActive(false);
            LevelSelectButton.SetActive(false);
            QuitButton.SetActive(false);
            OptionSelectButton.SetActive(false);

            _TraceScript.enabled = false;
            _RendererTrail.SetActive(false);
        }
        
    }
    public void Quit_Game()
    {
        Application.Quit();
    }
}
