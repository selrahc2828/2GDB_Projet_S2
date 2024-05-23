using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MainMenu
{
    public UnityEngine.UI.Toggle _muteSoundButton;
    public UnityEngine.UI.Slider _sliderUI;
    private bool _mute;
    public float _soundVolume = 1.0f; 

    [Header("FMOD")]
    private FMOD.Studio.Bus masterBus;

    private void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");

        _muteSoundButton.isOn = false; 
        _sliderUI.value = _soundVolume;
    }

    public void ChangeSoundVolume()
    {
        _soundVolume = _sliderUI.value; 

        if (!_mute)
        {
            masterBus.setVolume(_soundVolume);
        }
    }

    public void BackToMenu()
    {
        _PanelOption.SetActive(false);

        PlayButton.SetActive(true);
        LevelSelectButton.SetActive(true);
        QuitButton.SetActive(true);
        OptionSelectButton.SetActive(true);

        _TraceScript.enabled = true;
        _RendererTrail.SetActive(true);
    }
}
