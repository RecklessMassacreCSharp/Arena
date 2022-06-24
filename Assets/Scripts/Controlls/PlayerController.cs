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

    private InputAction moveAction;
    private InputAction jumpAction; 
    private InputAction characterTurnAction;

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
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y = 0f;

        int turnToggle = (int) characterTurnAction.ReadValue<float>();

        // Moving with new input system
        Vector2 movement = moveAction.ReadValue<Vector2>(); // Get value from action
        Vector3 move = new Vector3(movement.x, 0f, movement.y);

        if (turnToggle == 1) {
            move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
            move.y = 0;
        } else {
            move = move.x * transform.right.normalized + move.z * transform.forward.normalized;
            move.y = 0;
        }
        
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player
        if (jumpAction.triggered && groundedPlayer)
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate palyer towards camera direction if RMB
        if (turnToggle == 1) {
            float targetAngle = cameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = rotation; // May be better to just set rotation instead of lerping 
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

