using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSave : MonoBehaviour, IPlayerDataPersistence
{
    private static string playerSave;
    private static int currentLevel;

    private bool needsToSave;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;
        DontDestroyOnLoad(gameObject); // So we can keep and understand which player save we are working with
    }

    void Update() {
        if (needsToSave && PlayerDataPersistenceManager.instance != null) {
            PlayerDataPersistenceManager.instance.SavePlayer(playerSave);
            needsToSave = false;
        }
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().buildIndex != 0) {
            currentLevel = SceneManager.GetActiveScene().buildIndex;
            if (PlayerDataPersistenceManager.instance != null) {
                PlayerDataPersistenceManager.instance.SavePlayer(playerSave);
                needsToSave = false;
            } else { needsToSave = true; } 
        }
    }

    public static string getCurrentSave() {
        return playerSave;
    }

    public static void setCurrentSave(string newSave) {
        playerSave = newSave;
    }

    public void LoadData(PlayerData playerData) {}

    public void SaveData(ref PlayerData playerData) {
        if (currentLevel != 0) {
            playerData.level = currentLevel;
        }
    }
}
