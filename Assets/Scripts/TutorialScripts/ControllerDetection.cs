using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerDetection : MonoBehaviour
{
    private InputDevice lastDevice;

    // Tutorial Button Change Objects
    [SerializeField] private GameObject KeyboardButton;
    [SerializeField] private GameObject KeyboardOR1;
    [SerializeField] private GameObject KeyboardOR2;
    [SerializeField] private GameObject PlayStationButton;
    [SerializeField] private GameObject XboxButton;

    private bool tutorialActive;


    // Start is called before the first frame update
    void Start()
    {
        tutorialActive = true;
        InputSystem.onActionChange += inputActionChangeOnScreen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy(){
        InputSystem.DisableAllEnabledActions();
    }

    // Switches between our tutorial button layouts by detecting where the input came from whether that be a gane controller or keyboard or mouse.
    private void inputActionChangeOnScreen(object obj, InputActionChange change) {
        if (change.Equals(InputActionChange.ActionPerformed)) {
            string device_name = ((InputAction) obj).activeControl.device.name;
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

    // This trigger function will decide when the player has passed through the point where we can remove the tutorial text
    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.CompareTag("Player")) {
            tutorialActive = false;
            gameObject.SetActive(false);
        }
    }

}
