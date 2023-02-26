using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GridBrushBase;

public class PlayerController : MonoBehaviour
{
    private bool isGamePaused;
    PlayerInputActions inputAction;
    GameController gameController;
    Vector2 move;
    Vector2 rotate;
    Rigidbody rb;

    private float distanceToGround;
    bool isGrounded = true;
    public float jump = 5f;
    public float walkSpeed = 5f;
    public Camera playerCamera;
    Vector3 cameraRotation;

    [Header("Flash Light Stuff")]
    private bool lightFlickerStarted;
    private bool isFlashLightOn;
    public bool IsFlashLightOn
    {
        get; 
        private set;
    }
    public Light SpotLight;
    public Light AboveLight;
    public AudioSource FlickerLight;
    public RawImage MiniMap;

    public Camera MainCamera;
    public Camera MiniMapCamera;
    public Material grey;


    public bool IsGamePause
    {
        get
        {
            return isGamePaused;
        }
        private set
        {
            isGamePaused = value;
            GameManager.Instance.IsGamePaused = value;
        }
    }



    private bool isWalking = false;
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        inputAction = new PlayerInputActions();

        //using controller from Player InputController
        inputAction = PlayerInputController.controller.inputAction;

        inputAction.Player.Jump.performed += cntxt => Jump();

        inputAction.Player.Move.performed += cntxt => move = cntxt.ReadValue<Vector2>();
        inputAction.Player.Move.canceled += cntxt => move = Vector2.zero;

        inputAction.Player.Look.performed += cntxt => rotate = cntxt.ReadValue<Vector2>();
        inputAction.Player.Look.canceled += cntxt => rotate = Vector2.zero;

        inputAction.Player.FlashlightToggle.performed += cntxt => FlashLightToggle();

        inputAction.Player.ReChargeBattery.performed += cntxt => ReChargeBattery();

        inputAction.Player.SwitchLut.performed += cntxt => SwitchLut();

        rb = GetComponent<Rigidbody>();

        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        cameraRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);


        //This can be added to a seperate UI manager but added it here for now since it's just the pause
        inputAction.UI.Pause.performed += cntxt => Pause();

        IsFlashLightOn = true;

    }

    private void FlashLightToggle()
    {
        //To toggle the flashlight
        if (IsFlashLightOn)
        {
            SpotLight.intensity = 0;
            AboveLight.intensity = 0;
            MiniMap.color = new Color(1, 1, 1, 0);
            IsFlashLightOn = false;
        }
        else
        {
            SpotLight.intensity = 4;
            AboveLight.intensity = 1;
            MiniMap.color = new Color(1, 1, 1, 1);
            IsFlashLightOn = true;
        }
    }
    private void ReChargeBattery()
    {
        if (GameManager.Instance.IsDebuging)
            gameController.BatteryCharge = 100;
    }

    private void SwitchLut()
    {
        MainCamera.GetComponent<CameraLutScript>().m_renderMaterial = grey;
        MiniMapCamera.GetComponent<CameraLutScript>().m_renderMaterial = grey;
    }

    private void Pause()
    {
        if (GameManager.Instance.IsGamePaused)
            GameManager.Instance.IsGamePaused = false;
        else
            GameManager.Instance.IsGamePaused = true;
    }

    //handle player jump in terms of if the player is grounded
    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            isGrounded = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.GameManager.IsGamePaused)
        {
            if (isFlashLightOn)
                gameController.BatteryCharge -= gameController.BatteryDischargeRate;

            if (gameController.BatteryCharge < 0.30 && gameController.BatteryCharge > 0.01 && !lightFlickerStarted)
                StartCoroutine(Fliker());

        }
    }
    void FixedUpdate()
    {
        cameraRotation = new Vector3(cameraRotation.x + rotate.y, cameraRotation.y + rotate.x, cameraRotation.z);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
        transform.eulerAngles = new Vector3(transform.rotation.x, cameraRotation.y, transform.rotation.z);

        transform.Translate(Vector3.right * Time.deltaTime * move.x * walkSpeed, Space.Self);
        transform.Translate(Vector3.forward * Time.deltaTime * move.y * walkSpeed, Space.Self);

        isGrounded = Physics.Raycast(transform.position, -Vector3.up, distanceToGround);        
    }
    /// <summary>
    /// TODO add sound for flickering
    /// Makes Flashlight flicker to indicate low battery
    /// </summary>
    /// <returns></returns>
    IEnumerator Fliker()
    {
        lightFlickerStarted = true;
        yield return new WaitForSeconds(0.7f);

        SpotLight.intensity = 0;
        FlickerLight.Play();

        yield return new WaitForSeconds(0.7f);

        SpotLight.intensity = 4;
        lightFlickerStarted = false;
    }
}
