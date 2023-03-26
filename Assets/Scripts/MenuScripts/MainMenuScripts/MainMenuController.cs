using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    // Main Menu Buttons & Game Components
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject UserSaveMenu;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    private Transform continueTransform;
    private Transform playTransform;
    private Transform optionsTransform;
    private Transform exitTransform;

    public static string GameStartState;
    public static bool menuButtonState;
    private bool currentSaves;
    private bool buttonsMovedDown;

    // Gamepad Overlays
    private static GameObject xboxOverlay;
    private static GameObject playstationOverlay;

    // Start is called before the first frame update
    void Start()
    {
        // Collect all button transforms
        continueTransform = continueButton.transform;
        playTransform = playButton.transform;
        optionsTransform = optionsButton.transform;
        exitTransform = exitButton.transform;

        GameStartState = "";
        menuButtonState = true; // True Meaning Active
        currentSaves = anyCurrentSaves();
        buttonsMovedDown = false;

        if (!currentSaves) {
            continueButton.gameObject.SetActive(false); // Deactivating the continue save button

            // Sliding the buttons up in the Main Menu
            playTransform.localPosition = new Vector3(playTransform.localPosition.x, playTransform.localPosition.y + 100, playTransform.localPosition.z);
            optionsTransform.localPosition = new Vector3(optionsTransform.localPosition.x, optionsTransform.localPosition.y + 100, optionsTransform.localPosition.z);
            exitTransform.localPosition = new Vector3(exitTransform.localPosition.x, exitTransform.localPosition.y + 100, exitTransform.localPosition.z);
        }

        xboxOverlay = findChildWithTag("Xbox");
        playstationOverlay = findChildWithTag("Playstation");
    }

    // Update is called once per frame
    void Update()
    {
        if (anyCurrentSaves() && !buttonsMovedDown) {
            buttonsMovedDown = true;

            continueButton.gameObject.SetActive(true); // Activating the continue save button
            playButton.GetComponentInChildren<Text>().text = "NEW GAME";
        }

        if (!menuButtonState) {
            continueButton.interactable = false;
            playButton.interactable = false;
            optionsButton.interactable = false;
            exitButton.interactable = false;
        }
    }

    // ------------------ BUTTON FUNCTIONS ------------------
    public void continueButtonAction() {
        GameStartState = "CONTINUE";

        UserSaveMenu.SetActive(true); // Overlaying the Save Menu
    }

    public void playButtonAction() {
        GameStartState = "NEW GAME";

        UserSaveMenu.SetActive(true); // Overlaying the Save Menu
    }

    public void optionsButtonAction() {
        OptionsMenu.SetActive(true); // Overlaying the Options Menu
    }

    public void exitButtonAction() {
        Application.Quit(); // Quits the game application
    }
    // ------------------------------------------------------

    // Setter Function to set the menuButtonState variable
    public static void setMenuButtonState(bool state) {
        menuButtonState = state;
    }

    // Determine if there are any current saves
    private bool anyCurrentSaves() {
        string complete_path;
        foreach (string saveName in SaveMenuController.getSaveFileNames()) {
            complete_path = Path.Combine(Application.persistentDataPath, "player_saves");
            complete_path = Path.Combine(complete_path, saveName);
            if (Directory.Exists(complete_path)) {
                complete_path = Path.Combine(complete_path, saveName + ".game");
                if (File.Exists(complete_path)) {
                    return true;
                }
            }
        }
        return false;
    }

    public static void setGamePadHintState(string controller_type) {
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
