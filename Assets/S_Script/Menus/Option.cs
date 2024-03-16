using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Option : MonoBehaviour
{
    public UnityEngine.UI.Toggle _muteSoundButton;
    public UnityEngine.UI.Slider _sliderUI;
    public bool _mute;
    public float _soundVolume;

    private void Start()
    {
        _sliderUI.onValueChanged.AddListener(delegate { ChangeSondVolume(); });
        _muteSoundButton.onValueChanged.AddListener(delegate { ChangeSoundMute(); });
    }
    public void ChangeSoundMute()
    {
        _mute = _muteSoundButton.isOn;
        if (_mute)
        {
            AudioListener.volume = 0.0f;
        }
        else
        {
            AudioListener.volume = 1.0f;
        }
    }

    public void ChangeSondVolume()
    {

        // Update the sound value whenever the slider value changes
        _soundVolume = _sliderUI.value;
        // You can use this sound value to control the volume of your audio
        Debug.Log("Sound Value: " + _soundVolume);
        if (!_mute)
        {
            AudioListener.volume = _soundVolume;
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }
    public void ApplyChange()
    {

    }
}