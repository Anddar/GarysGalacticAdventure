using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Main Menu Buttons & Game Components
    [SerializeField] private GameObject OptionsMenu;

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

    // Start is called before the first frame update
    void Start()
    {
        currentSaves = anyCurrentSaves();

        // Collect all button transforms
        continueTransform = continueButton.transform;
        playTransform = playButton.transform;
        optionsTransform = optionsButton.transform;
        exitTransform = exitButton.transform;

        if (!currentSaves) {
            continueButton.SetActive(false); // Deactivating the continue save button

            // Sliding the buttons up in the Main Menu
            playTransform.position = new Vector3(playTransform.position.x, playTransform.position.y + 100, playTransform.position.z);
            optionsTransform.position = new Vector3(optionsTransform.position.x, optionsTransform.position.y + 100, optionsTransform.position.z);
            exitTransform.position = new Vector3(exitTransform.position.x, exitTransform.position.y + 100, exitTransform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ------------------ BUTTON FUNCTIONS ------------------
    public void continueButtonAction() {

    }

    public void playButtonAction() {
        if (chooseLevelOnPlay != -1) {
            // Allows devs to choose the level they want to start on when pressing the play button
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + chooseLevelOnPlay);
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void optionsButtonAction() {
        OptionsMenu.SetActive(true); // Turning the options menu on
    }

    public void exitButtonAction() {
        Application.Quit(); // Quits the game application
    }
    // ------------------------------------------------------

    // Determine if there are any current saves
    private bool anyCurrentSaves() {
        return false;
    }
}
