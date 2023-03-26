using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class TutorialControllerDetection : MonoBehaviour
{
    
    // Tutorial Button Change Objects
    [SerializeField] private GameObject KeyboardButton;
    [SerializeField] private GameObject KeyboardOR1;
    [SerializeField] private GameObject KeyboardOR2;
    [SerializeField] private GameObject PlayStationButton;
    [SerializeField] private GameObject XboxButton;

    PlayerInputActions playerInputActions;

    private bool tutorialActive;
    private string prev_device;


    // Start is called before the first frame update
    void Start()
    {
        tutorialActive = true;
    }

    void Awake() {
        // Enabling Controller Detection in the game
        playerInputActions = new PlayerInputActions();
        playerInputActions.ControllerDetect.Enable();
        playerInputActions.ControllerDetect.Detect.performed += allInput;
    }

    void OnDestroy() {
        InputSystem.DisableAllEnabledActions();
    }

    private void inputDeviceChanged(InputDevice device) {
        string device_name = device.name;
        if (device_name != prev_device) {
            if (device_name == "Keyboard" || device_name == "Mouse") {
                if (tutorialActive) {
                    PlayStationButton.SetActive(false);
                    XboxButton.SetActive(false);

                    KeyboardButton.SetActive(true);
                    KeyboardOR1.SetActive(true);
                    KeyboardOR2.SetActive(true);
                }
            } else if (device_name == "DualSenseGamepadHID" || device_name == "DualShock4GamepadHID") {
                if (tutorialActive) {
                    XboxButton.SetActive(false);
                    KeyboardButton.SetActive(false);
                    KeyboardOR1.SetActive(false);
                    KeyboardOR2.SetActive(false);

                    PlayStationButton.SetActive(true);
                }
            } else if (device_name == "XInputControllerWindows") {
                if (tutorialActive) {
                    PlayStationButton.SetActive(false);
                    KeyboardButton.SetActive(false);
                    KeyboardOR1.SetActive(false);
                    KeyboardOR2.SetActive(false);

                    XboxButton.SetActive(true);
                }
            }
        }
    }

    // For when a user presses a key on any control device it will come through here as well to determine the control type (Keyboard&Mouse or Gamepad)
    private void allInput(InputAction.CallbackContext context) {
        inputDeviceChanged(context.control.device);
    }


    // This trigger function will decide when the player has passed through the point where we can remove the tutorial text
    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player")) {
            tutorialActive = false;
            gameObject.SetActive(false);
        }
    }

}