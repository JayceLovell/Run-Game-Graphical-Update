using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    //Debugging Stuff
    public bool IsDebuging;

    //Regular Code

    private static GameManager _instance;  
    private ScoreManager _scoreManager;
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard
    }
    private float _bgmVolume;
    private float _sfxVolume;
    [SerializeField]
    private bool _isGamePaused;
    private bool _isGameLost;
    private bool _isGameWon;
    private string _userName;
    private float _score;

    public static GameManager Instance { get; private set; }

    public ScoreManager ScoreManager
    {
        get { return _scoreManager; }
        set { _scoreManager = value; }
    }
    public DifficultyLevel Difficulty;

    public float BGMVolume
    {
        get
        {         
            return (_bgmVolume);
        }
        set
        {
            _bgmVolume = value;
            SoundManager.MasterVolumeChanged(value);
            PlayerData.BGMVolume = value;
        }
    }
    public float SFXVolume
    {
        get
        {            
            return (_sfxVolume);
        }
        set
        {
            _sfxVolume = value;
            PlayerData.SFXVolume=value;
        }
    }
    public bool IsGamePaused
    {
        get { return _isGamePaused; }
        set { 
            _isGamePaused = value;
            if(value)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
                Cursor.visible = false;
        }
    }
    public bool IsGameLost
    {
        get { return _isGameLost; }
        set
        {
            _isGameLost= value;
            if(value)
                SceneManager.LoadScene("GameOver");
        }
    }
    public bool IsGameWon
    {
        get { return _isGameWon; }
        set
        {
            _isGamePaused = value;
            if (value)
            {
                _scoreManager.AddScore(UserName, Score);
                SceneManager.LoadScene("GameWon");
            }
        }
    }
    /// <summary>
    /// Player Username
    /// </summary>
    public string UserName
    {
        get
        {
            if(_userName == null || _userName == "")
                return PlayerData.UserName;
            else
                return _userName;
        }
        set
        {
            _userName = value;
            PlayerData.UserName = value;
        }
    }
    /// <summary>
    /// PLayer Score
    /// </summary>
    public float Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //Awake is always called before any Start functions
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _scoreManager = new ScoreManager();
        LoadPlayerData();
    }
        // Use this for initialization
    void Start () {
        _scoreManager.LoadScores();        
    }
    private void LoadPlayerData()
    {
        _bgmVolume  = PlayerData.BGMVolume;
        _sfxVolume = PlayerData.SFXVolume;
        UserName = PlayerData.UserName;        
    }
    private void SavePlayerData()
    {
        PlayerData.Save();
    }
    public void Quit()
    {
        SavePlayerData();
        _scoreManager.SaveScores();
        Application.Quit();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the player has returned to the title scene
        if (scene.name == "Title")
        {
            Debug.Log("Resetting");
            IsGameLost = false;
            IsGamePaused = false;
            IsGameWon = false;
            Score = 0;
            UserName = null;
        }
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
