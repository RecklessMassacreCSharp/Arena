using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // TODO
    // Think how to eliminate camera shaking when spamming RMB/LMB
    [SerializeField] private float runningSpeed = 3.0f;
    [SerializeField] private float walkingSpeed = 1.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    
    private bool isRunning = true;
    private float speed;
    private PlayerInput playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool hasJumped = false; // To distinguish causes of falling
    private Vector3 jumpForwardSpeed;
    private Vector3 move;

    private InputAction moveAction;
    private InputAction jumpAction; 
    private InputAction characterTurnAction;
    private InputAction mouseAutoMoveAction;
    private InputAction walkAction;

    private Transform cameraTransform;
    
    private void Awake() 
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        // Get actions from player input component
        jumpAction = playerInput.actions["Jump"];
        jumpAction.performed += _ => OnJump();

        walkAction = playerInput.actions["WalkToggle"];
        walkAction.performed += _ => OnWalkToggle();

        moveAction = playerInput.actions["Movement"];
        characterTurnAction = playerInput.actions["CharacterTurnToggle"];
        mouseAutoMoveAction = playerInput.actions["DoubleMouseAutoMove"];
    }

    private void Start() 
    {
        speed = runningSpeed;
    }

    private void Update()
    {
        int turnToggle = (int) characterTurnAction.ReadValue<float>();
        int autoMoveToggle = (int) mouseAutoMoveAction.ReadValue<float>(); 
        MovePlayer(autoMoveToggle ,turnToggle);

        // Rotate player towards camera direction if RMB
        if (turnToggle == 1)
            RotateQuick();
    }

    private void MovePlayer(int autoMoveToggle, int turnToggle) {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;

        if (groundedPlayer)
            hasJumped = false;

        Vector2 movement = moveAction.ReadValue<Vector2>();

        if (autoMoveToggle == 1)
            movement = new Vector2(0f, 1f); // Override movement input value if mouse movement is toggled

        move = new Vector3(movement.x, 0f, movement.y);

        if (turnToggle == 1) {
            // Dont need y value of camera forward vector cos it slows down character
            Vector3 camV = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z);
            move = move.x * cameraTransform.right.normalized + move.z * camV.normalized;
            move.y = 0;
        } else {
            move = move.x * transform.right.normalized + move.z * transform.forward.normalized;
            move.y = 0;
        }

        Vector3 moveSpeed = move * speed; // Velocity vector for x-z plane
        if (!groundedPlayer && hasJumped)
            moveSpeed = jumpForwardSpeed; // Retain direction and speed if player jumped

        controller.Move(moveSpeed * Time.deltaTime); // Move on x-z plane

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime); // Move along y axis
    }

    private void RotateQuick() {
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = rotation;
    }

    private void OnWalkToggle() {
        switch (isRunning)
        {
            case true:
                speed = walkingSpeed;
                isRunning = false;
                break;
            case false:
                speed = runningSpeed;
                isRunning = true;
                break;
        }
    }

    private void OnJump() {
        if (groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumpForwardSpeed = move * speed; // For retaining speed and direction when jumping
            hasJumped = true;
        }
    }   
    
    /*
    Vector3 camV = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z);
    Debug.Log($"cameraTransform = {cameraTransform.forward}");
    Debug.DrawRay(cameraTransform.position, camV.normalized * 5, Color.blue, 10f);

    Debug.Log($"transform = {transform.forward}");
    Debug.DrawRay(transform.position, transform.forward * 5, Color.red, 10f);

    Debug.Log($"move = {move}");
    Debug.DrawRay(transform.position, move.normalized * 5, Color.green, 10f);
    */
}

