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

    private bool cheatMovingScenes;
    private static Animator transition;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] GameObject pauseMenu;
    private static PauseMenuController pauseController;

    // Start is called before the first frame update
    void Start() {
        xboxOverlay = findChildWithTag("Xbox");
        playstationOverlay = findChildWithTag("Playstation");

        // Transitioner for the cheat menu to move between levels
        if (pauseMenu) {
            pauseController = pauseMenu.GetComponent<PauseMenuController>();
        }
        transition = GameObject.FindGameObjectWithTag("LevelFade").GetComponent<Animator>();
        cheatMovingScenes = false;
    }

    // Update is called once per frame
    void Update() {
        if (xboxOverlay == null || playstationOverlay == null) {
            xboxOverlay = findChildWithTag("Xbox");
            playstationOverlay = findChildWithTag("Playstation");
        }

        if (pauseMenu) {
            pauseController = pauseMenu.GetComponent<PauseMenuController>();
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

    public void moveToLevel1CheatButton() {
        if (SceneManager.GetActiveScene().buildIndex != 1 && !cheatMovingScenes) { cheatMovingScenes = true; StartCoroutine(moveToLevel(1)); }
    }

    public void moveToLevel1_2CheatButton() {
        if (SceneManager.GetActiveScene().buildIndex != 2 && !cheatMovingScenes) { cheatMovingScenes = true; StartCoroutine(moveToLevel(2)); }
    }

    private IEnumerator moveToLevel(int levelIndex) {
        gameObject.SetActive(false);
        generalButtonAction();

        pauseController.setPauseMenuButtonState(false);
        pauseController.forceResume();
        pauseController.setPauseMenuButtonState(true);

        transition.SetTrigger("Start");
        SceneManager.LoadScene(levelIndex);
        yield return true;
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
