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
    
    // Reference to Options Menu
    [SerializeField] private GameObject OptionsMenu;

    // Pause Menu Buttons
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button saveAndQuitButton;   

    // Pause State
    private static bool pauseStatus; 

    // Start is called before the first frame update
    void Start()
    {
        pauseStatus = false;
    }

    // Update is called once per frame
    void Update()
    {
        // When player closes the options menu we want to open the original pause menu back up
        if (pauseStatus && !OptionsMenu.activeSelf) {
            gameObject.SetActive(true);
        }
    }

    void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.PauseMenu.Enable();
        playerInputActions.Player.PauseMenu.performed += openPauseMenu;
    }

    // This function allows us to open the pause menu from other scripts
    public void openPauseMenu(InputAction.CallbackContext context) {
        AudioListener.pause = true; // Pauses all audio in the game
        Time.timeScale = 0; // Set timescale to 0 to stop time hence stopping the game
        pauseStatus = true;
        gameObject.SetActive(true);
    }

    // This function allows other scripts to check if the game is paused
    public static bool getPauseStatus() {
        return pauseStatus;
    }

    // ------------------ BUTTON FUNCTIONS ------------------
    public void resumeButtonAction() {
        pauseStatus = false;
        AudioListener.pause = false; // Unpause all the audio in the game
        Time.timeScale = 1; // Set timescale back to normal so the game starts again
    }

    public void optionsButtonAction() {
        gameObject.SetActive(false); // Turn the pause menu off so options menu goes in front
        OptionsMenu.SetActive(true); // Turn the options menu on
    }

    public void saveAndQuitButtonAction() {
        // Save Game Here

        Time.timeScale = 1; // Set time back to normal so the game starts againTime.timeScale = 1; // Set time back to normal so the game starts again

        SceneManager.LoadScene(0); // Loads the main menu screen
    }
    // ------------------------------------------------------


}
