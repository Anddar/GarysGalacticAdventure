using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCyclerTutorial : MonoBehaviour
{
    // Bullet Cycler Tutorial Object
    [SerializeField] private GameObject BulletCyclerTutorialObject;

    // Each Device Tutorial Buttons
    [SerializeField] private GameObject KeyboardMouse;
    [SerializeField] private GameObject PlaystationController;
    [SerializeField] private GameObject XboxController;

    // Prev Device
    private string prev_device;

    // Start is called before the first frame update
    void Start()
    {
        prev_device = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerBulletCycler.bulletCyclerActive && !BulletCyclerTutorialObject.activeInHierarchy) {
            BulletCyclerTutorialObject.SetActive(true);
        }

        updateTutorial();
    }

    private void updateTutorial() {
        string curr_device = ControlDetection.getCurrentDevice();
        if (prev_device != curr_device) {
            prev_device = curr_device;
            switch (curr_device) {
                case "Keyboard":
                    PlaystationController.SetActive(false);
                    XboxController.SetActive(false);
                    KeyboardMouse.SetActive(true);
                    break;
                case "Mouse":
                    PlaystationController.SetActive(false);
                    XboxController.SetActive(false);
                    KeyboardMouse.SetActive(true);
                    break;
                case "Playstation":
                    KeyboardMouse.SetActive(false);
                    XboxController.SetActive(false);
                    PlaystationController.SetActive(true);
                    break;
                case "Xbox":
                    KeyboardMouse.SetActive(false);
                    PlaystationController.SetActive(false);
                    XboxController.SetActive(true);
                    break;
            }
        }
    }
}
