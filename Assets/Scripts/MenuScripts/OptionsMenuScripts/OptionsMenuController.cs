using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenuController : MonoBehaviour
{
    // Options Menu Buttons
    [SerializeField] private Button generalButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button videoButton;
    [SerializeField] private Button exitButton;

    // Options Menu Tabs
    [SerializeField] private GameObject generalTab;
    [SerializeField] private GameObject audioTab;
    [SerializeField] private GameObject videoTab;

    // Gamepad Overlays
    private static GameObject xboxOverlay;
    private static GameObject playstationOverlay;

    // Start is called before the first frame update
    void Start() {
        xboxOverlay = findChildWithTag("Xbox");
        playstationOverlay = findChildWithTag("Playstation");
    }

    // Update is called once per frame
    void Update() {
        if (xboxOverlay == null || playstationOverlay == null) {
            xboxOverlay = findChildWithTag("Xbox");
            playstationOverlay = findChildWithTag("Playstation");
        }
    }

    // ------------------ BUTTON FUNCTIONS ------------------
    public void generalButtonAction() {
        audioTab.SetActive(false);
        videoTab.SetActive(false);
        generalTab.SetActive(true);
    }

    public void audioButtonAction() {
        generalTab.SetActive(false);
        videoTab.SetActive(false);
        audioTab.SetActive(true);
    }

    public void videoButtonAction() {
        audioTab.SetActive(false);
        generalTab.SetActive(false);
        videoTab.SetActive(true);
    }

    public void exitButtonAction() {
        // Makes sure that general settings tab is the first to come up.
        generalTab.SetActive(true);
        audioTab.SetActive(false);
        videoTab.SetActive(false);

        gameObject.SetActive(false); // Closes the game options menu

        // Save options when closing the Options menu
        OptionsDataPersistenceManager.instance.SaveOptions();
    }
    // ------------------------------------------------------

    public static void setGamePadHintState(string controller_type) {
        if (xboxOverlay == null || playstationOverlay == null) {
            return;
        }

        switch(controller_type) {
            case "Xbox":
                playstationOverlay.SetActive(false);
                xboxOverlay.SetActive(true);
                break;
            case "Playstation":
                xboxOverlay.SetActive(false);
                playstationOverlay.SetActive(true);
                break;
            case "Keyboard":
                xboxOverlay.SetActive(false);
                playstationOverlay.SetActive(false);
                break;
            default:
                xboxOverlay.SetActive(false);
                playstationOverlay.SetActive(false);
                break;
        }
    }

    private GameObject findChildWithTag(string tag) {
        foreach (Transform child in gameObject.transform) {
            if (child.tag == tag)
                return child.gameObject;
        }
        return null;
    }
}
