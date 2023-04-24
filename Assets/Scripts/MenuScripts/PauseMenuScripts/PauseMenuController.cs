using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    // Getting PlayerInputActions
    private PlayerInputActions playerInputActions;
    
    // Reference to Menus
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OptionsMenu;
    

    // Pause Menu Buttons
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button saveAndQuitButton;   

    // Pause State
    private static bool pauseStatus; 
    
    // Save & Quit State
    private bool savingAndQuiting;

    // Gamepad Overlays
    private static GameObject xboxOverlay;
    private static GameObject playstationOverlay;

    // Start is called before the first frame update
    void Start()
    {
        pauseStatus = false;
        savingAndQuiting = false;

        xboxOverlay = findChildWithTag("Xbox");
        playstationOverlay = findChildWithTag("Playstation");
    }

    // Update is called once per frame
    void Update()
    {
        // When player closes the options menu we want to open the original pause menu back up
        if (pauseStatus && !OptionsMenu.activeSelf) {
            PauseMenu.SetActive(true);
        }

        if (xboxOverlay == null || playstationOverlay == null) {
            xboxOverlay = findChildWithTag("Xbox");
            playstationOverlay = findChildWithTag("Playstation");
        }
    }

    void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.PauseMenu.Enable();
        playerInputActions.Player.PauseMenu.performed += openPauseMenu;
    }

    // This function allows us to open the pause menu from other scripts
    public void openPauseMenu(InputAction.CallbackContext context) {
        if (PauseMenu != null && !savingAndQuiting && !LevelCompletionStates.isLevelComplete()) {
            if (!pauseStatus) {
                pauseStatus = true;
                //AudioListener.pause = true; // Pauses all audio in the game
                Time.timeScale = 0; // Set timescale to 0 to stop time hence stopping the game
                PauseMenu.SetActive(true);
            } else if (pauseStatus && !OptionsMenu.activeSelf) { 
                // If pressing escape again we will close the pause menu
                pauseStatus = false;
                //AudioListener.pause = false; // Unpause all the audio in the game
                PauseMenu.SetActive(false);
                Time.timeScale = 1; // Set timescale back to normal so the game starts again
            }
        }
    }

    // This function allows other scripts to set if the game is paused from another location
    public static void setPauseStatus(bool state) {
        pauseStatus = state;
    }

    // This function allows other scripts to check if the game is paused
    public static bool getPauseStatus() {
        return pauseStatus;
    }

    // ------------------ BUTTON FUNCTIONS ------------------
    public void resumeButtonAction() {
        if (pauseStatus) {
            pauseStatus = false;
            //AudioListener.pause = false; // Unpause all the audio in the game
            PauseMenu.SetActive(false);
            Time.timeScale = 1; // Set timescale back to normal so the game starts again
        }
    }

    public void optionsButtonAction() {
        setPauseMenuButtonState(false);
        PauseMenu.SetActive(false); // Turn the pause menu off so options menu goes in front
        setPauseMenuButtonState(true);
        OptionsMenu.SetActive(true); // Turn the options menu on
    }

    public void saveAndQuitButtonAction() {
        savingAndQuiting = true;

        setPauseMenuButtonState(false);

        // Saving Game
        OptionsDataPersistenceManager.instance.SaveOptions(); // Saving Options
        PlayerDataPersistenceManager.instance.SavePlayer(PlayerSave.getCurrentSave()); // Saving Player

        PlayerBulletCycler.turnOffBulletCycler();

        Time.timeScale = 1; // Set time back to normal so the game starts again

        PlayerSave.setCurrentSave("");

        //AudioListener.pause = false; // Turning Music back on before leaving scene

        setPauseMenuButtonState(true);

        SceneManager.LoadScene(0); // Loads the main menu screen
    }

    public void setPauseMenuButtonState(bool state) {
        resumeButton.interactable = state;
        optionsButton.interactable = state;
        saveAndQuitButton.interactable = state;
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
        if (PauseMenu == null) {
            return null;
        }

        foreach (Transform child in PauseMenu.transform) {
            if (child.tag == tag)
                return child.gameObject;
        }
        return null;
    }

    public void forceResume() {
        resumeButtonAction();
    }

}
