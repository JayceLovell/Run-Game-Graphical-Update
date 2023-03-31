using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameController _controller;
    [SerializeField]
    private TextMeshProUGUI _timer;
    [SerializeField]
    private GameObject _PauseTitle;
    [SerializeField]
    private GameObject _PauseMenu;
    [SerializeField]
    private Slider BackgroundSlider;
    [SerializeField]
    private Slider SoundEffectSlider;

    [SerializeField]
    public float BGVolume
    {
        get
        {
            return GameManager.Instance.BGMVolume;
        }
        set
        {
            GameManager.Instance.BGMVolume = value;
        }
    }
    [SerializeField]
    public float FXVolume
    {
        get
        {
            return GameManager.Instance.SFXVolume;
        }
        set
        {
            GameManager.Instance.SFXVolume = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _controller = GameObject.Find("GameController").GetComponent<GameController>();
        BackgroundSlider.value = BGVolume;
        SoundEffectSlider.value = FXVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGamePaused)
        {
            if (_PauseMenu.activeInHierarchy)
            {
                PauseMenu(false);
            }
            _timer.text = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}",
                TimeSpan.FromSeconds(_controller.GameTime).Hours, 
                TimeSpan.FromSeconds(_controller.GameTime).Minutes, 
                TimeSpan.FromSeconds(_controller.GameTime).Seconds,
                TimeSpan.FromSeconds(_controller.GameTime).Milliseconds/100);
        }
        else
        {
            if (!_PauseMenu.activeInHierarchy)
            {
                PauseMenu(true);
            }
        }
    }
    private void PauseMenu(bool visible)
    {
        _PauseTitle.SetActive(visible);
        _PauseMenu.SetActive(visible);
    }
    public void ResumeButton()
    {
        GameManager.Instance.IsGamePaused = false;
    }
    public void BackToMenuButton()
    {
        GameManager.Instance.IsGamePaused=false;
        SceneManager.LoadScene("Title");
    }
}
