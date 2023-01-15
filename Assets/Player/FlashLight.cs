using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashLight : MonoBehaviour
{
    public bool IsFlashLightOn;
    public Light SpotLight;
    public AudioSource FlickerLight;

    private StarterAssetsInputs _input;
    private float _lightpausevalue;
    private bool _flickingstarted;

    private GameController _gameController;

    void Initialize()
    {
        IsFlashLightOn = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        //_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!_gameController.IsGamePause)
        {

        }
        if (_input.toggleFlashLight)
        {
            _toggleFlashLight(IsFlashLightOn);
        }
        //if (_gameController.IsGamePause)
        //{
        //    _lightpausevalue = _gameController.BatteryPower;
        //}
        //if (_gameController.BatteryPower < 0.30 && _gameController.BatteryPower > 0.01)
        //    _startFlicker();
    }
    private void _toggleFlashLight(bool toggle)
    {
        if (toggle)
        {
            SpotLight.intensity = 0;
            IsFlashLightOn = false;
           _input.toggleFlashLight = false;
        }
        else
        {
            SpotLight.intensity = 4;
            IsFlashLightOn = true;
            _input.toggleFlashLight = false;
        }
    }
    private void _startFlicker()
    {
        if (!_flickingstarted)
        {
            StartCoroutine(Fliker());
        }
        _flickingstarted = true;
    }
    IEnumerator Fliker()
    {
        yield return new WaitForSeconds(0.7f);

        SpotLight.intensity = 0;
        FlickerLight.Play();

        yield return new WaitForSeconds(0.7f);

        SpotLight.intensity = 4;
        _flickingstarted = false;
    }
}
