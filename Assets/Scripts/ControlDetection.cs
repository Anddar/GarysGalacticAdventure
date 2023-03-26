using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ControlDetection : MonoBehaviour
{
    // First Buttons Selected in Menu Screens
    [SerializeField] GameObject MainMenuFirstButtonContinue, MainMenuFirstButtonPlay, OptionsMenuFirstButton, SaveMenuFirstButton, OverwriteDialogFirstButton, PauseMenuFirstButton;
    GameObject MainMenuFirstButton;

        // Options Menu Header Buttons
        Button OptionsGeneralButton, OptionsAudioButton, OptionsVideoButton, OptionsExitButton;
        Selectable OptionsGeneralNavDown, OptionsAudioNavDown, OptionsVideoNavDown;

        // Inner Options Menu Tabs
        [SerializeField] GameObject OptionsGeneralTab, OptionsAudioTab, OptionsVideoTab;


    // All Menu Objects
    [SerializeField] GameObject MainMenu, OptionsMenu, SaveMenu, OverwriteDialogBox, PauseMenu;

    PlayerInputActions playerInputActions;

    private string current_device;

    void Start()
    {
        OptionsGeneralButton = OptionsMenuFirstButton.GetComponent<Button>();
        OptionsAudioButton = OptionsGeneralButton.navigation.selectOnRight.gameObject.GetComponent<Button>();
        OptionsVideoButton = OptionsAudioButton.GetComponent<Button>().navigation.selectOnRight.gameObject.GetComponent<Button>();
        OptionsExitButton = OptionsVideoButton.GetComponent<Button>().navigation.selectOnRight.gameObject.GetComponent<Button>();

        OptionsGeneralNavDown = OptionsMenuFirstButton.GetComponent<Button>().navigation.selectOnDown;
        OptionsAudioNavDown = OptionsAudioButton.navigation.selectOnDown;
        OptionsVideoNavDown = OptionsVideoButton.navigation.selectOnDown;

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
                SaveMenuController.setGamePadHintState(current_device);
            } else if (OptionsMenu.activeInHierarchy) {
                setSelectedObject(OptionsMenuFirstButton, OptionsMenu);
                OptionsMenuController.setGamePadHintState(current_device);
            } else if (MainMenu.activeInHierarchy) {
                setSelectedObject(MainMenuFirstButton, MainMenu);
                MainMenuController.setGamePadHintState(current_device);
            } else {
                setSelectedObject(null, null);
            }
        } else if (PauseMenu != null) {
            // This is if we are in-game and have a pause menu
            if (OptionsMenu.activeInHierarchy) {
                setSelectedObject(OptionsMenuFirstButton, OptionsMenu);
                OptionsMenuController.setGamePadHintState(current_device);
            } else if (PauseMenu.activeInHierarchy) {
                setSelectedObject(PauseMenuFirstButton, PauseMenu);
                PauseMenuController.setGamePadHintState(current_device);
            } else {
                setSelectedObject(null, null);
            }
        }

        if (OptionsGeneralTab.activeInHierarchy) {
            setOptionsTabNavDown(OptionsGeneralNavDown);
        } else if (OptionsAudioTab.activeInHierarchy) {
            setOptionsTabNavDown(OptionsAudioNavDown);
        } else if (OptionsVideoTab.activeInHierarchy) {
            setOptionsTabNavDown(OptionsVideoNavDown);
        }
    } 

    void Awake() {
        // Enabling Controller Detection in the game
        playerInputActions = new PlayerInputActions();
        playerInputActions.ControllerDetect.Enable();
        playerInputActions.ControllerDetect.Detect.performed += allInput;
        playerInputActions.ControllerDetect.Back.performed += onControllerBack;
    }

    void OnDestroy() {
        InputSystem.DisableAllEnabledActions();
    }

    private void inputDeviceChanged(InputDevice device) {
        string device_name = device.name;
        if (device_name == "Keyboard" || device_name == "Mouse") {
            current_device = "Keyboard";
        } else if (device_name == "DualSenseGamepadHID" || device_name == "DualShock4GamepadHID") {
            current_device = "Playstation";
        } else if (device_name == "XInputControllerWindows") {
            current_device = "Xbox";
        }  
    }

    // Moves user back out of a particular menu
    private void onControllerBack(InputAction.CallbackContext context) {
        if (MainMenu != null) {
            // If were in the Main Menu we will work in this button selection process
            if (SaveMenu.activeInHierarchy) {
                SaveMenu.SetActive(false);
            } else if (OptionsMenu.activeInHierarchy) {
                OptionsGeneralTab.SetActive(true);
                OptionsAudioTab.SetActive(false);
                OptionsVideoTab.SetActive(false);

                OptionsMenu.SetActive(false);

                // Save options when closing the Options menu
                OptionsDataPersistenceManager.instance.SaveOptions();
            }
        } else if (PauseMenu != null) {
            // This is if we are in-game and have a pause menu
            if (OptionsMenu.activeInHierarchy) {
                OptionsGeneralTab.SetActive(true);
                OptionsAudioTab.SetActive(false);
                OptionsVideoTab.SetActive(false);

                OptionsMenu.SetActive(false);

                // Save options when closing the Options menu
                OptionsDataPersistenceManager.instance.SaveOptions();
            } else if (PauseMenu.activeInHierarchy) {
                if (PauseMenuController.getPauseStatus()) {
                    PauseMenuController.setPauseStatus(false);
                    AudioListener.pause = false; // Unpause all the audio in the game
                    PauseMenu.SetActive(false);
                    Time.timeScale = 1; // Set timescale back to normal so the game starts again
                }
            }
        }
    }

    // For when a user presses a key on any control device it will come through here as well to determine the control type (Keyboard&Mouse or Gamepad)
    private void allInput(InputAction.CallbackContext context) {
        inputDeviceChanged(context.control.device);
    }

    private void setSelectedObject(GameObject obj, GameObject menu) {
        if (obj == null && menu == null) {
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        if (EventSystem.current.currentSelectedGameObject == null) {
            EventSystem.current.SetSelectedGameObject(obj);
            return;
        }

        if (menu.tag == "Saves" && EventSystem.current.currentSelectedGameObject.tag == "SaveMenu") {
            return;
        }
        
        if (EventSystem.current.currentSelectedGameObject != obj && menu.tag != EventSystem.current.currentSelectedGameObject.tag) {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(obj);
        }
    }

    // Changes the options navigation so that any of the four tab buttons can go down into any options tab menu
    private void setOptionsTabNavDown(Selectable selectDown) {
        Navigation newNavigation = new Navigation();
        newNavigation.mode = Navigation.Mode.Explicit;
        newNavigation.selectOnLeft = OptionsGeneralButton.navigation.selectOnLeft;
        newNavigation.selectOnRight = OptionsGeneralButton.navigation.selectOnRight;
        newNavigation.selectOnDown = selectDown;
        OptionsGeneralButton.navigation = newNavigation;

        newNavigation.selectOnLeft = OptionsAudioButton.navigation.selectOnLeft;
        newNavigation.selectOnRight = OptionsAudioButton.navigation.selectOnRight;
        OptionsAudioButton.navigation = newNavigation;

        newNavigation.selectOnLeft = OptionsVideoButton.navigation.selectOnLeft;
        newNavigation.selectOnRight = OptionsVideoButton.navigation.selectOnRight;
        OptionsVideoButton.navigation = newNavigation;

        newNavigation.selectOnLeft = OptionsExitButton.navigation.selectOnLeft;
        newNavigation.selectOnRight = OptionsExitButton.navigation.selectOnRight;
        OptionsExitButton.navigation = newNavigation;
    }

}