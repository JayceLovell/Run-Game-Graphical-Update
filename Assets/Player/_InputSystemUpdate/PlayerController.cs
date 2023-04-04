using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GridBrushBase;

public class PlayerController : MonoBehaviour
{
    private bool isGamePaused;
    private bool isSprinting;
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

    public Camera MainCamera;
    public Camera MiniMapCamera;
    public Material grey;

    [Header("Breadcrumbs")]
    public GameObject Breadcrumbs;
    public float nextBreadCrumb = 5.0f;
    public Vector3 lastBreadCrumbPos;

    [Header("Flash Light Stuff")]
    [SerializeField]
    private GameObject FLashLight;   
    public Color BatteryFull;
    public Color BatteryEmpty;
    public Color BatteryInPeekMode;
    private Color CurrentColor;
    [SerializeField]
    private bool lightFlickerStarted;
    [SerializeField]
    private bool isFlashLightOn;
    public Light SpotLight;
    public Light AboveLight;
    public GameObject Window;


    public bool IsFlashLightOn
    {
        get
        {
            return isFlashLightOn;
        }
        set
        {
            isFlashLightOn = value;
        }
    }


    public RawImage MiniMap;
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
        MiniMap = GameObject.FindGameObjectWithTag("MiniMapRender").GetComponent<RawImage>();
        inputAction = new PlayerInputActions();

        //using controller from Player InputController
        inputAction = PlayerInputController.controller.inputAction;

        inputAction.Player.Jump.performed += cntxt => Jump();

        inputAction.Player.Sprint.performed += cntxt => Sprint();

        inputAction.Player.Pause.performed += cntxt => Pause();

        inputAction.Player.Move.performed += cntxt => move = cntxt.ReadValue<Vector2>();
        inputAction.Player.Move.canceled += cntxt => move = Vector2.zero;

        inputAction.Player.Look.performed += cntxt => rotate = cntxt.ReadValue<Vector2>();
        inputAction.Player.Look.canceled += cntxt => rotate = Vector2.zero;

        inputAction.Player.FlashlightToggle.performed += cntxt => FlashLightToggle();

        inputAction.Player.Peek.performed += cntxt => Peek();

        inputAction.Player.ReChargeBattery.performed += cntxt => ReChargeBattery();

        inputAction.Player.SwitchLut.performed += cntxt => SwitchLut();

        inputAction.Player.Interact.performed += cntxt => Interact();

        rb = GetComponent<Rigidbody>();

        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        cameraRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);


        //This can be added to a seperate UI manager but added it here for now since it's just the pause
        //inputAction.UI.Pause.performed += cntxt => Pause();

        IsFlashLightOn = true;
        lightFlickerStarted = false;

        SwitchLut();

        nextBreadCrumb = 5.0f;

        Window.SetActive(false);


    }

    private void Interact()
    {
        GameObject.Find("GetAwayCar").GetComponent<PlayerWon>().GetInCar();
    }

    private void FlashLightToggle()
    {
        if(GameManager.Instance.IsGamePaused)
        { return; }
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
    /// <summary>
    /// Looking through wall ability
    /// </summary>
    private void Peek()
    {
        if (Window.activeInHierarchy)
        {
            Window.SetActive(false);
            gameController.BatteryDischargeRate /= 5;
            SoundManager.PlaySound(SoundManager.SoundFX.ExitSeeThroughMode);
            GameObject.Find("BgSound").GetComponent<SoundBGVolume>().RaiseVolume(0.1f);
            SwitchLut();
            FLashLight.GetComponent<Renderer>().material.SetFloat("_Outline", 0.10f);
        }
        else
        {
            // Activate window and modify game state
            Window.SetActive(true);
            gameController.BatteryDischargeRate *= 5;
            SoundManager.PlaySound(SoundManager.SoundFX.SeeThroughMode);
            GameObject.Find("BgSound").GetComponent<SoundBGVolume>().LowerVolume(1f, 0.1f);
            SwitchLut();
            FLashLight.GetComponent<Renderer>().material.SetFloat("_Outline", 0.50f);
        }                  
    }

    /// <summary>
    /// little cheat
    /// </summary>
    private void ReChargeBattery()
    {
        if (GameManager.Instance.IsDebuging)
            gameController.BatteryCharge = 100;
    }

    private void SwitchLut()
    {
        MainCamera.GetComponent<CameraLutScript>().enabled = !MainCamera.GetComponent<CameraLutScript>().enabled;

        MiniMapCamera.GetComponent<CameraLutScript>().enabled = !MiniMapCamera.GetComponent<CameraLutScript>().enabled;

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
    private void Sprint()
    {
        if (isSprinting)
        {
            walkSpeed = walkSpeed / 2;
            isSprinting = false;
        }
        else
        {
            walkSpeed = walkSpeed * 2;
            isSprinting = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGamePaused && !gameController.IsGameOver)
        {
            if (isFlashLightOn)
            {
                gameController.BatteryCharge -= gameController.BatteryDischargeRate;
                // Calculate the battery level as a percentage
                float batteryLevel = gameController.BatteryCharge / 100.0f;

                // Interpolate between the BatteryFull and BatteryEmpty colors based on the battery level
                CurrentColor = Color.Lerp(BatteryEmpty, BatteryFull, batteryLevel);

                FLashLight.GetComponent<Renderer>().material.SetColor("_OutlineColor", CurrentColor);

            }

            if (gameController.BatteryCharge < 40.0 && gameController.BatteryCharge > 0.01 && !lightFlickerStarted)
                StartCoroutine(Flickr());

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                if (Physics.Raycast(transform.position, -Vector3.up, distanceToGround))
                {
                    if (Vector3.Distance(lastBreadCrumbPos, hit.point) > nextBreadCrumb)
                    {
                        GameObject breadcrumb = Instantiate(Breadcrumbs, hit.point + new Vector3(0, 0.1f, 0), Quaternion.identity);
                        breadcrumb.transform.Rotate(90f, 0, 0);
                        lastBreadCrumbPos = hit.point;
                    }
                }
            }

        }
    }
    void FixedUpdate()
    {
        if (!GameManager.Instance.IsGamePaused && !gameController.IsGameOver)
        {
            cameraRotation = new Vector3(cameraRotation.x + rotate.y, cameraRotation.y + rotate.x, cameraRotation.z);

            playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
            transform.eulerAngles = new Vector3(transform.rotation.x, cameraRotation.y, transform.rotation.z);

            transform.Translate(Vector3.right * Time.deltaTime * move.x * walkSpeed, Space.Self);
            transform.Translate(Vector3.forward * Time.deltaTime * move.y * walkSpeed, Space.Self);

            isGrounded = Physics.Raycast(transform.position, -Vector3.up, distanceToGround);
        }
    }
    /// <summary>
    /// TODO add sound for flickering
    /// Makes Flashlight flicker to indicate low battery
    /// </summary>
    /// <returns></returns>
    private IEnumerator Flickr()
    {        
            lightFlickerStarted = true;

        if (isFlashLightOn)
        {
            SpotLight.intensity = 0;
            SoundManager.PlaySound(SoundManager.SoundFX.FlickrLight);

            yield return new WaitForSeconds(1.5f);

            SpotLight.intensity = 4;
        }
            lightFlickerStarted = false;
    }
    /// <summary>
    /// Blur Camera
    /// </summary>
    /// <returns></returns>
    [ContextMenu("Respawn")]
    public IEnumerator SentBack()
    {
        playerCamera.GetComponent<Blur>().integerRange = 64;
        yield return new WaitForSeconds(1f);
        this.transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        yield return new WaitForSeconds(1f);
        playerCamera.GetComponent<Blur>().integerRange = 1;
    }
}
