using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OptionsDataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string optionsFileName;
    [SerializeField] private bool ecryptOptionsData;


    // Data Variables
    private OptionsData optionsData;
    private OptionsFileHandler optionsFileHandler;


    // Persistence Manager
    public static OptionsDataPersistenceManager instance { get; private set; }
    private List<IOptionsDataPersistence> optionsDataPersistenceObjects; // Holds all the objects that are using the data persistence

    private void Start() {
        this.optionsFileHandler = new OptionsFileHandler(Application.persistentDataPath, optionsFileName, ecryptOptionsData);
        this.optionsDataPersistenceObjects = FindAllOptionsDataPersistenceObjects();
        
        LoadOptions();
    }

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Multiple Data Persistence Managers in scene!");
        }
        instance = this;
    }

    public void NewGame() {
        this.optionsData = new OptionsData();
    }

    public void LoadOptions() {
        this.optionsData = optionsFileHandler.Load(); // Try to load data, this will determine if there is anything we should update before starting 

        // When no data can be found we start a new game data
        if (this.optionsData == null) {
            Debug.Log("No data found. Using default options.");
            NewGame();
        }

        // Loads the data into each script using the options data persistence script
        foreach (IOptionsDataPersistence optionsDataPersistenceObject in optionsDataPersistenceObjects) {
            optionsDataPersistenceObject.LoadData(optionsData);
        }
    }

    public void SaveOptions() {
        // Pass data to other scripts so they update properly
        foreach (IOptionsDataPersistence optionsDataPersistenceObject in optionsDataPersistenceObjects) {
            optionsDataPersistenceObject.SaveData(ref optionsData);
        }

        // Saving our Users Options Data to a file
        optionsFileHandler.Save(optionsData);
    }

    // If player closes game all Options will still be saved
    private void OnApplicationQuit() {
        SaveOptions();
    }

    // This function will get us all the objects that are using our options data persistence interface
    private List<IOptionsDataPersistence> FindAllOptionsDataPersistenceObjects() {
        IEnumerable<IOptionsDataPersistence> optionsDataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IOptionsDataPersistence>();
        return new List<IOptionsDataPersistence>(optionsDataPersistenceObjects);
    }
}
