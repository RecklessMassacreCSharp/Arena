using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCamTest : MonoBehaviour
{
    private PlayerInput playerInput;

    [Tooltip("static")] [SerializeField] private CinemachineVirtualCamera vcam1;
    [Tooltip("dynamic")] [SerializeField] private CinemachineVirtualCamera vcam2;

    private int priorityBoost = 2;

    private InputAction lookBtnAction;

    private void Awake() {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        lookBtnAction = playerInput.actions["MouseLookToggle"];
    }

    private void OnEnable() {
        lookBtnAction.performed += _ => StartDynamicCam();
        lookBtnAction.canceled += _ => StopDynamicCam();
    }

    private void OnDisable() {
        lookBtnAction.performed -= _ => StartDynamicCam();
        lookBtnAction.canceled -= _ => StopDynamicCam();
    }

    private void StartDynamicCam() {
        vcam2.Priority += priorityBoost;
    }

    private void StopDynamicCam() {
        vcam2.Priority -= priorityBoost;
    }

}
