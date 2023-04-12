using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class PlayerDataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string playerSaveDirectoryName;
    [SerializeField] private bool ecryptPlayerData;

    private string savesDirectory;
    
    // Data Variables
    private PlayerData playerData;
    private PlayerFileHandler playerFileHandler;

    // Persistence Manager
    public static PlayerDataPersistenceManager instance { get; private set; }
    private List<IPlayerDataPersistence> playerDataPersistenceObjects; // Holds all the objects that are using the data persistence
    
    private void Start() {
        savesDirectory = Path.Combine(Application.persistentDataPath, playerSaveDirectoryName);

        this.playerFileHandler = new PlayerFileHandler(savesDirectory, ecryptPlayerData);
        this.playerDataPersistenceObjects = FindAllPlayerDataPersistenceObjects();
        
        // Creates Folder to hold all saves
        this.playerFileHandler.createSaveDirectory();

        NewGame();
    }

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Multiple Data Persistence Managers in scene!");
        }
        instance = this;
    }

    public void NewGame() {
        this.playerData = new PlayerData();
    }

    public void LoadPlayer(string player_save_num) {
        if (playerDataPersistenceObjects == null) { return; }

        this.playerData = playerFileHandler.Load(player_save_num); // Try to load data, this will determine if there is anything we should update before starting game

        // When no data can be found we start a new game data
        if (this.playerData == null) {
            Debug.Log("No data found. Creating default player, starting on level 1.");
            NewGame();
        }

        Debug.Log("Player Data being loaded: " + this.playerData.level + " score: " + this.playerData.totalScore);

        // Loads the data into each script using the player data persistence script
        foreach (IPlayerDataPersistence playerDataPersistenceObject in playerDataPersistenceObjects) {
            playerDataPersistenceObject.LoadData(playerData);
        }
    }

    public bool SavePlayer(string player_save_num) {
        // If the objects in Scene are not ready we tell the script that is trying to save that we are not ready so it will try to save again.
        if (playerDataPersistenceObjects == null) { return false; }        

        // Pass data to other scripts so they update properly
        foreach (IPlayerDataPersistence playerDataPersistenceObject in playerDataPersistenceObjects) {
            playerDataPersistenceObject.SaveData(ref playerData);
        }

        // Saving our Users Player Data to a file
        Debug.Log("Player Data being saved: " + playerData.level + " score: " + playerData.totalScore);
        playerFileHandler.Save(playerData, player_save_num);

        return true;
    }

    // If player closes game all Player will still be saved
    private void OnApplicationQuit() {
        if (PlayerSave.getCurrentSave() != "" && PlayerSave.getCurrentSave() != null) {
            SavePlayer(PlayerSave.getCurrentSave());
        }
    }

    // Returns whether a save file exists or not by using the active PlayerFileHandler
    public bool saveExists(string player_save_num) {
        return this.playerFileHandler.saveExists(player_save_num);
    }

    // Returns the path to the Outer Save Directory
    public string saveDirectory() {
        return savesDirectory;
    }

    // This function will get us all the objects that are using our player data persistence interface
    private List<IPlayerDataPersistence> FindAllPlayerDataPersistenceObjects() {
        IEnumerable<IPlayerDataPersistence> playerDataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerDataPersistence>();
        return new List<IPlayerDataPersistence>(playerDataPersistenceObjects);
    }
}
