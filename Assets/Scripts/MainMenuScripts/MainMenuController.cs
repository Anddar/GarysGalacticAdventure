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

    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject exitButton;

    private Transform continueTransform;
    private Transform playTransform;
    private Transform optionsTransform;
    private Transform exitTransform;

    [SerializeField] private int chooseLevelOnPlay;
    private bool currentSaves;
    private bool buttonsMovedDown;

    // Start is called before the first frame update
    void Start()
    {
        // Collect all button transforms
        continueTransform = continueButton.transform;
        playTransform = playButton.transform;
        optionsTransform = optionsButton.transform;
        exitTransform = exitButton.transform;

        currentSaves = anyCurrentSaves();
        buttonsMovedDown = false;

        if (!currentSaves) {
            continueButton.SetActive(false); // Deactivating the continue save button

            // Sliding the buttons up in the Main Menu
            playTransform.localPosition = new Vector3(playTransform.localPosition.x, playTransform.localPosition.y + 100, playTransform.localPosition.z);
            optionsTransform.localPosition = new Vector3(optionsTransform.localPosition.x, optionsTransform.localPosition.y + 100, optionsTransform.localPosition.z);
            exitTransform.localPosition = new Vector3(exitTransform.localPosition.x, exitTransform.localPosition.y + 100, exitTransform.localPosition.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (anyCurrentSaves() && !buttonsMovedDown) {
            buttonsMovedDown = true;

            continueButton.SetActive(true); // Activating the continue save button
            playButton.GetComponentInChildren<Text>().text = "NEW GAME";
        }
    }

    // ------------------ BUTTON FUNCTIONS ------------------
    public void continueButtonAction() {
        UserSaveMenu.SetActive(true); // Overlaying the Save Menu
    }

    public void playButtonAction() {
        UserSaveMenu.SetActive(true); // Overlaying the Save Menu
    }

    public void optionsButtonAction() {
        OptionsMenu.SetActive(true); // Overlaying the Options Menu
    }

    public void exitButtonAction() {
        Application.Quit(); // Quits the game application
    }
    // ------------------------------------------------------

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

}
