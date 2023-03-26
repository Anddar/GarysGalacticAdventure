using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveMenuController : MonoBehaviour, IPlayerDataPersistence
{
    // Options Menu Buttons
    [SerializeField] private Button save1Button;
    [SerializeField] private Button save2Button;
    [SerializeField] private Button save3Button;
    [SerializeField] private Button save4Button;
    [SerializeField] private Button exitButton;

    // Save File Names
    private static string save1Name;
    private static string save2Name;
    private static string save3Name;
    private static string save4Name;

    // Overwrite Dialog Box Components
    [SerializeField] private GameObject overwriteDialogBox;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    // Fade Into Playing Components
    private Animator transition;
    [SerializeField] private float transitionIntoPlayTime = 1f;

    private int scene_index_toLoad;

    // Tells us if the save menu has been fully initialized
    private bool initialized;

    // Gamepad Overlays
    private static GameObject xboxOverlay;
    private static GameObject playstationOverlay;


    // Start is called before the first frame update
    void Start() {
        setSaveButtonUseableState(true);
        setOverwriteButtonUseableState(true);

        save1Name = "PlayerSave1";
        save2Name = "PlayerSave2";
        save3Name = "PlayerSave3";
        save4Name = "PlayerSave4";

        transition = GameObject.FindGameObjectWithTag("LevelFade").GetComponent<Animator>();

        scene_index_toLoad = 1;
        initialized = false;

        xboxOverlay = findChildWithTag("Xbox");
        playstationOverlay = findChildWithTag("Playstation");
    }

    // Update is called once per frame
    void Update() {
        if (!initialized) {
            // Checks if any saves exist and places their level name as the save name in the Save Menu
            if (PlayerDataPersistenceManager.instance.saveExists(save1Name)) {
                PlayerSave.setCurrentSave(save1Name);
                PlayerDataPersistenceManager.instance.LoadPlayer(save1Name);
            }

            if (PlayerDataPersistenceManager.instance.saveExists(save2Name)) {
                PlayerSave.setCurrentSave(save2Name);
                PlayerDataPersistenceManager.instance.LoadPlayer(save2Name);
            }

            if (PlayerDataPersistenceManager.instance.saveExists(save3Name)) {
                PlayerSave.setCurrentSave(save3Name);
                PlayerDataPersistenceManager.instance.LoadPlayer(save3Name);
            }

            if (PlayerDataPersistenceManager.instance.saveExists(save4Name)) {
                PlayerSave.setCurrentSave(save4Name);
                PlayerDataPersistenceManager.instance.LoadPlayer(save4Name);
            }

            PlayerSave.setCurrentSave(""); // Make sure to reset the current save after finished initializing
            PlayerDataPersistenceManager.instance.NewGame(); // Reset the PlayerData to be a NewGame
            scene_index_toLoad = 1;

            initialized = true;
        }

        if (xboxOverlay == null || playstationOverlay == null) {
            xboxOverlay = findChildWithTag("Xbox");
            playstationOverlay = findChildWithTag("Playstation");
        }
    }

    // ------------------ BUTTON FUNCTIONS ------------------
    public void save1ButtonAction() {
        setSaveButtonUseableState(false);

        // Define which save was selected
        PlayerSave.setCurrentSave(save1Name);

        // Start Save 1 - If it is a prexisting (loaded) or new save
        if (onSaveSelected(save1Name)) { StartCoroutine(LoadLevel(scene_index_toLoad)); }
    }

    public void save2ButtonAction() {
        setSaveButtonUseableState(false);

        // Define which save was selected
        PlayerSave.setCurrentSave(save2Name);

        // Start Save 2 - If it is a prexisting (loaded) or new save
        if (onSaveSelected(save2Name)) { StartCoroutine(LoadLevel(scene_index_toLoad)); }
    }

    public void save3ButtonAction() {
        setSaveButtonUseableState(false);

        // Define which save was selected
        PlayerSave.setCurrentSave(save3Name);

        // Start Save 3 - If it is a prexisting (loaded) or new save
        if (onSaveSelected(save3Name)) { StartCoroutine(LoadLevel(scene_index_toLoad)); }
    }

    public void save4ButtonAction() {
        setSaveButtonUseableState(false);

        // Define which save was selected
        PlayerSave.setCurrentSave(save4Name);

        // Start Save 4 - If it is a prexisting (loaded) or new save
        if (onSaveSelected(save4Name)) { StartCoroutine(LoadLevel(scene_index_toLoad)); }
    }

    public void overwriteYesButtonAction() {
        // Handling Closing the Overwrite Dialog Box
        setOverwriteButtonUseableState(false);
        overwriteDialogBox.SetActive(false);
        setOverwriteButtonUseableState(true);

        // Overwriting Save File
        PlayerDataPersistenceManager.instance.SavePlayer(PlayerSave.getCurrentSave());

        // Handling Closing the Saves Menu (behind dialog box)
        GameObject.FindGameObjectWithTag("Saves").SetActive(false);
        setSaveButtonUseableState(true);

        // Load level with overwrite
        StartCoroutine(LoadLevel(scene_index_toLoad));
    }

    public void overwriteNoButtonAction() {
        // Handling Closing the Overwrite Dialog Box
        setOverwriteButtonUseableState(false);
        overwriteDialogBox.SetActive(false);
        setOverwriteButtonUseableState(true);

        setSaveButtonUseableState(true); // Turn save menu buttons back on after dialog box closed
    }

    public void exitButtonAction() {
        GameObject.FindGameObjectWithTag("Saves").SetActive(false); // Closes the game saves menu
    }

    public bool onSaveSelected(string saveName) {
        if (MainMenuController.GameStartState == "CONTINUE") {
            if (PlayerDataPersistenceManager.instance.saveExists(saveName)) {
                PlayerDataPersistenceManager.instance.LoadPlayer(saveName);
            } else { 
                setSaveButtonUseableState(true);
                return false; 
            }
        } else if (MainMenuController.GameStartState == "NEW GAME") {
            if (PlayerDataPersistenceManager.instance.saveExists(saveName)) {
                // Ask player if they would like to overwrite save. If true overwrite otherwise do not.
                overwriteDialogBox.SetActive(true);
                return false;
            } else {
                PlayerDataPersistenceManager.instance.SavePlayer(saveName);
            }
        }

        // Handling Closing the Saves Menu
        GameObject.FindGameObjectWithTag("Saves").SetActive(false);
        setSaveButtonUseableState(true);

        // Turn off Main Menu Buttons
        MainMenuController.setMenuButtonState(false);

        return true;
    }

    public void setSaveButtonUseableState(bool state) {
        save1Button.interactable = state;
        save2Button.interactable = state;
        save3Button.interactable = state;
        save4Button.interactable = state;
        exitButton.interactable = state;
    }

    public void setOverwriteButtonUseableState(bool state) {
        yesButton.interactable = state;
        noButton.interactable = state;
    }
    // ------------------------------------------------------

    private string getLevelName(int scene_build_index) {
        switch(scene_build_index) {
            case 1:
                return "Level 1 - Earth";
            case 2:
                return "Level 1.2 - Earth";
        }
        return "Level Name Not Set";
    }

    public static List<string> getSaveFileNames() {
        return new List<string>(){save1Name, save2Name, save3Name, save4Name};
    }

    // Player Data Persistence
    void IPlayerDataPersistence.LoadData(PlayerData playerData)
    {
        switch(PlayerSave.getCurrentSave()) {
            case "PlayerSave1":
                scene_index_toLoad = playerData.level;
                save1Button.GetComponentInChildren<Text>().text = getLevelName(playerData.level);
                break;
            case "PlayerSave2":
                scene_index_toLoad = playerData.level;
                save2Button.GetComponentInChildren<Text>().text = getLevelName(playerData.level);
                break;
            case "PlayerSave3":
                scene_index_toLoad = playerData.level;
                save3Button.GetComponentInChildren<Text>().text = getLevelName(playerData.level);
                break;
            case "PlayerSave4":
                scene_index_toLoad = playerData.level;
                save4Button.GetComponentInChildren<Text>().text = getLevelName(playerData.level);
                break;
        }
    }

    void IPlayerDataPersistence.SaveData(ref PlayerData playerData) {} // We dont need to save any data in the Saves Menu

    // Load player into the proper level in the game
    private IEnumerator LoadLevel(int levelIndex){
        // Turn off Main Menu Buttons
        MainMenuController.setMenuButtonState(false);

        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionIntoPlayTime);
        SceneManager.LoadScene(levelIndex);
    }

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
        if (GameObject.FindGameObjectWithTag("Saves") == null) {
            return null;
        }

        foreach (Transform child in GameObject.FindGameObjectWithTag("Saves").transform) {
            if (child.tag == tag)
                return child.gameObject;
        }
        return null;
    }
}
