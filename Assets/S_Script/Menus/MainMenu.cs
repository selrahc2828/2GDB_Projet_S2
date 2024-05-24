using FMODUnity;
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

    [Header("FMOD")]
    private FMOD.Studio.Bus masterBus;

    private void Awake()
    {
        _GameManagerScript = FindAnyObjectByType<GameManager>();
    }

    private void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Level1");
        _GameManagerScript._gameLose = false;
        masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
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
