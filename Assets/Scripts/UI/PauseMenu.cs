using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseUI;
    
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public Toggle masterMuteToggle;
    public Toggle bgmMuteToggle;
    public Toggle sfxMuteToggle;

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(val => AudioManager.Instance.SetMasterVolume(val));
        bgmSlider.onValueChanged.AddListener(val => AudioManager.Instance.SetBGMVolume(val));
        sfxSlider.onValueChanged.AddListener(val => AudioManager.Instance.SetSFXVolume(val));

        masterMuteToggle.onValueChanged.AddListener(val => AudioManager.Instance.MuteMaster(val, masterSlider.value));
        bgmMuteToggle.onValueChanged.AddListener(val => AudioManager.Instance.MuteBGM(val, bgmSlider.value));
        sfxMuteToggle.onValueChanged.AddListener(val => AudioManager.Instance.MuteSFX(val, sfxSlider.value));
    }

    public void Pause()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GameManager.Instance.SetElapsedTime(0f);
        SceneTransitionManager.Instance.TransitionToScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneTransitionManager.Instance.TransitionToScene("MainMenu");
    }
}
