using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour {    

    public Slider FXVolumeSlider;
    public Slider BGVolumeSlider;

    // PUBLIC INSTANCE VARIABLES

    [Header("Text")]
    public TextMeshProUGUI VersionLabel;
    public TextMeshProUGUI PreviousRunnerPlaceholder;

    [Header("Buttons")]
    [SerializeField]
    public Button StartButton;
    public Button ExitButton;

    [Header("ScoreBoard")]
    public ScrollRect Scoreboard;
    public GameObject ScorePrefab;
    
    void Start () {
        VersionLabel.text = "Version: " + Application.version;               
        SoundManager.StartBackground(SoundManager.BgSound.Title);
        BGVolumeSlider.value = GameManager.Instance.BGMVolume;
        FXVolumeSlider.value = GameManager.Instance.SFXVolume;


        foreach (var score in GameManager.Instance.ScoreManager.GetTop10Scores())
        {
            GameObject scoreGO = Instantiate(ScorePrefab, Scoreboard.content);
            TextMeshProUGUI[] textComponents = scoreGO.GetComponentsInChildren<TextMeshProUGUI>();
            textComponents[0].text = score.PlayerName;
            textComponents[1].text = score.Score.ToString();
        }

        PreviousRunnerPlaceholder.text = GameManager.Instance.UserName;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    // PUBLIC METHODS
    public void Start_Game()
    {
        SceneManager.LoadScene("Main");
    }
    public void Close_Game()
    {
        GameManager.Instance.Quit();
    }
    public void EnterUserName(TMP_InputField value)
    {
        GameManager.Instance.UserName = value.text;
    }
    public void SelectedDifficulty(TMP_Dropdown target)
    {       
        switch (target.value)
        {
            case 0:
                GameManager.Instance.Difficulty = GameManager.DifficultyLevel.Easy;
                break;
            case 1:
                GameManager.Instance.Difficulty = GameManager.DifficultyLevel.Normal;
                break;
            case 2:
                GameManager.Instance.Difficulty = GameManager.DifficultyLevel.Hard;
                break;
        }
    }
}
