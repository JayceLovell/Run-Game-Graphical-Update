using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {
    private GameManager _gameManager;
    [SerializeField]
    private float _gameTime;
    private float _batteryCharge;
    private GameObject _respawnPoint;
    private float _batteryDischargeRate;
    private List<Transform> _BatteryPositions;
    

    public List<GameObject> Spooks;
    public List<GameObject> Batteries;
    public GameObject Spook;
    public GameObject Battery;

    public GameManager GameManager
    {
        get { return _gameManager; }
        set { _gameManager = value; }
    }
  
    public float GameTime
    {
        get
        {
            return this._gameTime;
        }
        set
        {
            this._gameTime = value;
        }
    }
    public float BatteryCharge
    {
        get
        {
            return this._batteryCharge;
        }
        set
        {
            this._batteryCharge = value;
            if (_batteryCharge <= 0f)
            {
                GameManager.IsGameLost  = true;                
            }
        }
    }

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }
    // Use this for initialization
    void Start () {
        _spawnSpooks();
        _gameTime = 0;
        _batteryCharge = 100f;
        _gameDifficulty(GameManager.Difficulty);
        //lock cursor to screen
        Cursor.lockState = CursorLockMode.Locked;
        _spawnBatteries();

    }
    
	// Update is called once per frame
	void Update () {
        if (!GameManager.IsGamePaused)
        {
            _gameTime += Time.deltaTime;            
            _gameManager.Score = _calculateScore(GameTime, Spooks.Count, Batteries.Count);
        }
    }

    // Private METHODS*******************************
    private void _spawnSpooks()
    {
        //for(int i = 0; i < _spooks.Length;i++)
        //{
        //    _spooks[i] = Instantiate(Spook, _respawnPoint.transform.position,_respawnPoint.transform.rotation);
        //}
    }
    private void _spawnBatteries()
    {
        GameObject[] batteryPositions = GameObject.FindGameObjectsWithTag("BatteryPosition");
        _BatteryPositions = batteryPositions.Select(batteryPosition => batteryPosition.transform).ToList();

        foreach (Transform position in _BatteryPositions)
        {
            Batteries.Add(Instantiate(Battery, position));
        }
    }
    private void _gameDifficulty(string Difficulty)
    {
        switch(Difficulty)
        {
            case "Easy":
                _batteryDischargeRate = 0.04f;
                break;
            case "Normal":
                _batteryDischargeRate = 0.06f;
                break;
            case "Hard":
                _batteryDischargeRate = 0.10f;
                break;
            default:
                _batteryDischargeRate = 0.02f;
                break;
        }
    }
    /// <summary>
    /// Calcualtes Score based on factors
    /// </summary>
    /// <param name="_timeToEnd">Time it took for player to end</param>
    /// <param name="SpooksLeft">Spooks Left In Game</param>
    /// <param name="BatteriesLeft">How many batties they used</param>
    /// <returns></returns>
    private float _calculateScore(float _time,int SpooksLeft, int BatteriesLeft)
    {
        float score = 0;
        float timeFactor = 100 / _time;
        float enemiesFactor = 100 / (SpooksLeft + 1);
        float batteriesFactor = 100 / (BatteriesLeft + 1);

        score = timeFactor + enemiesFactor + batteriesFactor;

        return score;

    }

}
