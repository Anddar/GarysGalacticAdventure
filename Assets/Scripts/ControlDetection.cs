using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ControlDetection : MonoBehaviour
{
    // First Buttons Selected in Menu Screens
    [SerializeField] GameObject MainMenuFirstButtonContinue, MainMenuFirstButtonPlay, OptionsMenuFirstButton, SaveMenuFirstButton, OverwriteDialogFirstButton, PauseMenuFirstButton;
    GameObject MainMenuFirstButton;

    // All Menu Objects
    [SerializeField] GameObject MainMenu, OptionsMenu, SaveMenu, OverwriteDialogBox, PauseMenu;

    PlayerInputActions playerInputActions;

    private string prev_device;

    void Start()
    {
        if (MainMenuFirstButtonContinue != null && MainMenuFirstButtonPlay != null) {
            if (MainMenuFirstButtonContinue) {
                MainMenuFirstButton = MainMenuFirstButtonContinue;
            } else {
                MainMenuFirstButton = MainMenuFirstButtonPlay;
            }
        } else { MainMenuFirstButton = null; }
    }

    void Update() {
        if (MainMenu != null) {
            // If were in the Main Menu we will work in this button selection process
            if (OverwriteDialogBox.activeInHierarchy) {
                setSelectedObject(OverwriteDialogFirstButton, OverwriteDialogBox);
            } else if (SaveMenu.activeInHierarchy) {
                setSelectedObject(SaveMenuFirstButton, SaveMenu);
            } else if (OptionsMenu.activeInHierarchy) {
                setSelectedObject(OptionsMenuFirstButton, OptionsMenu);
            } else if (MainMenu.activeInHierarchy) {
                setSelectedObject(MainMenuFirstButton, MainMenu);
            } else {
                setSelectedObject(null, null);
            }
        } else {
            // This is if we are in-game in a pause menu
            if (OptionsMenu.activeInHierarchy) {
                setSelectedObject(OptionsMenuFirstButton, OptionsMenu);
            } else if (PauseMenu.activeInHierarchy) {
                setSelectedObject(PauseMenuFirstButton, PauseMenu);
            } else {
                setSelectedObject(null, null);
            }
        }
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

            } else if (device_name == "DualSenseGamepadHID" || device_name == "DualShock4GamepadHID") {

            } else if (device_name == "XInputControllerWindows") {

            }
        }
    }

    // For when a user presses a key on any control device it will come through here as well to determine the control type (Keyboard&Mouse or Gamepad)
    private void allInput(InputAction.CallbackContext context) {
        inputDeviceChanged(context.control.device);
    }

    private void setSelectedObject(GameObject obj, GameObject menu) {
        //EventSystem.current.currentSelectedGameObject != obj && 
        if (obj == null && menu == null) {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        if (EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(obj);
            return;
        }

        if (EventSystem.current.currentSelectedGameObject != obj && menu.tag != EventSystem.current.currentSelectedGameObject.tag) {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(obj);
        }
    }

}

