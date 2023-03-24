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

    // Fade Into Playing Components
    private Animator transition;
    [SerializeField] private float transitionIntoPlayTime = 1f;

    private int scene_index_toLoad;

    // Tells us if the save menu has been fully initialized
    private bool initialized;


    // Start is called before the first frame update
    void Start() {
        save1Name = "PlayerSave1";
        save2Name = "PlayerSave2";
        save3Name = "PlayerSave3";
        save4Name = "PlayerSave4";

        transition = GameObject.FindGameObjectWithTag("LevelFade").GetComponent<Animator>();

        scene_index_toLoad = 1;
        initialized = false;
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

            initialized = true;
        }
    }

    // ------------------ BUTTON FUNCTIONS ------------------
    public void save1ButtonAction() {
        save2Button.interactable = false;
        save3Button.interactable = false;
        save4Button.interactable = false;

        // Define which save was selected
        PlayerSave.setCurrentSave(save1Name);

        if (PlayerDataPersistenceManager.instance.saveExists(save1Name)) {
            PlayerDataPersistenceManager.instance.LoadPlayer(save1Name);
        } else {
            PlayerDataPersistenceManager.instance.SavePlayer(save1Name);
        }

        // Start Save 1
        StartCoroutine(LoadLevel(scene_index_toLoad));
    }

    public void save2ButtonAction() {
        save1Button.interactable = false;
        save3Button.interactable = false;
        save4Button.interactable = false;

        // Define which save was selected
        PlayerSave.setCurrentSave(save2Name);

        if (PlayerDataPersistenceManager.instance.saveExists(save2Name)) {
            PlayerDataPersistenceManager.instance.LoadPlayer(save2Name);
        } else {
            PlayerDataPersistenceManager.instance.SavePlayer(save2Name);
        }

        // Start Save 2
        StartCoroutine(LoadLevel(scene_index_toLoad));
    }

    public void save3ButtonAction() {
        save1Button.interactable = false;
        save2Button.interactable = false;
        save4Button.interactable = false;

        // Define which save was selected
        PlayerSave.setCurrentSave(save3Name);

        if (PlayerDataPersistenceManager.instance.saveExists(save3Name)) {
            PlayerDataPersistenceManager.instance.LoadPlayer(save3Name);
        } else {
            PlayerDataPersistenceManager.instance.SavePlayer(save3Name);
        }

        // Start Save 3
        StartCoroutine(LoadLevel(scene_index_toLoad));
    }

    public void save4ButtonAction() {
        save1Button.interactable = false;
        save2Button.interactable = false;
        save3Button.interactable = false;

        // Define which save was selected
        PlayerSave.setCurrentSave(save4Name);
        
        if (PlayerDataPersistenceManager.instance.saveExists(save4Name)) {
            PlayerDataPersistenceManager.instance.LoadPlayer(save4Name);
        } else {
            PlayerDataPersistenceManager.instance.SavePlayer(save4Name);
        }

        // Start Save 4
        StartCoroutine(LoadLevel(scene_index_toLoad));
    }

    public void exitButtonAction() {
        GameObject.FindGameObjectWithTag("Saves").SetActive(false); // Closes the game saves menu
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
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionIntoPlayTime);
        SceneManager.LoadScene(levelIndex);
    }
}
