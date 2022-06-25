using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private PlayerInput playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool hasJumped = false; // To distinguish causes of falling
    private Vector3 jumpForward;

    private InputAction moveAction;
    private InputAction jumpAction; 
    private InputAction characterTurnAction;
    private InputAction mouseAutoMoveAction;

    private Transform cameraTransform;

    private void Start() 
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        // Get actions from player input component
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        characterTurnAction = playerInput.actions["CharacterTurnToggle"]; 
        mouseAutoMoveAction = playerInput.actions["DoubleMouseAutoMove"];
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;

        if (groundedPlayer)
            hasJumped = false;

        int turnToggle = (int) characterTurnAction.ReadValue<float>();
        int autoMoveToggle = (int) mouseAutoMoveAction.ReadValue<float>();

        Vector2 movement = moveAction.ReadValue<Vector2>();

        // For moving forward with both mouse buttons pressed
        if (autoMoveToggle == 1)
            movement = new Vector2(0f, 1f);

        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        
        if (turnToggle == 1) {
            // Dont need y value of camera forward vector cos it slows down character
            Vector3 camV = new Vector3(cameraTransform.forward.x, 0f, cameraTransform.forward.z);
            move = move.x * cameraTransform.right.normalized + move.z * camV.normalized;
            move.y = 0;
        } else {
            move = move.x * transform.right.normalized + move.z * transform.forward.normalized;
            move.y = 0;
        }
        
        // For retaining speed and direction when jumping
        if (!groundedPlayer && hasJumped)
            move = jumpForward;

        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player
        if (jumpAction.triggered && groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumpForward = move;
            hasJumped = true;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate palyer towards camera direction if RMB
        if (turnToggle == 1) {
            float targetAngle = cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = rotation;
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

