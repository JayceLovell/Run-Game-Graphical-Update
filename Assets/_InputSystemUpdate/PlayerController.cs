using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions inputAction;
    Vector2 move;
    Vector2 rotate;
    Rigidbody rb;

    private float distanceToGround;
    bool isGrounded = true;
    public float jump = 5f;
    public float walkSpeed = 5f;
    public Camera playerCamera;
    Vector3 cameraRotation;

    private bool isWalking = false;
    void Start()
    {
        inputAction = new PlayerInputActions();

        //using controller from Player InputController
        inputAction = PlayerInputController.controller.inputAction;

        inputAction.Player.Jump.performed += cntxt => Jump();

        inputAction.Player.Move.performed += cntxt => move = cntxt.ReadValue<Vector2>();
        inputAction.Player.Move.canceled += cntxt => move = Vector2.zero;

        inputAction.Player.Look.performed += cntxt => rotate = cntxt.ReadValue<Vector2>();
        inputAction.Player.Look.canceled += cntxt => rotate = Vector2.zero;

        inputAction.Player.FlashlightToggle.performed += cntxt => FlashLightToggle();

        rb = GetComponent<Rigidbody>();

        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        cameraRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);


        //This can be added to a seperate UI manager but added it here for now since it's just the pause
        inputAction.UI.Pause.performed += cntxt => Pause();

    }

    private void FlashLightToggle()
    {
        //To toggle the flashlight
    }

    private void Pause()
    {
        //Implement pause here or switch to UI manager
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
        cameraRotation = new Vector3(cameraRotation.x + rotate.y, cameraRotation.y + rotate.x, cameraRotation.z);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
        transform.eulerAngles = new Vector3(transform.rotation.x, cameraRotation.y, transform.rotation.z);

        transform.Translate(Vector3.right * Time.deltaTime * move.x * walkSpeed, Space.Self);
        transform.Translate(Vector3.forward * Time.deltaTime * move.y * walkSpeed, Space.Self);

        isGrounded = Physics.Raycast(transform.position, -Vector3.up, distanceToGround);
    }
}
