using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PlayerFileHandler
{
    private string playerSaveDirectoryPath;

    // Player File Encryption
    private bool encryptFile = false;
    private readonly string encryptReadPassword = "74fdda1bdc06613492e14a44bc6a7e21";

    public PlayerFileHandler(string playerSaveDirectoryPath, bool encryptFile) {
        this.playerSaveDirectoryPath = playerSaveDirectoryPath;
        this.encryptFile = encryptFile;
    }

    // Loads our past player from the existing player file
    public PlayerData Load(string playerSaveFileName) {
        string complete_path = Path.Combine(playerSaveDirectoryPath, playerSaveFileName);
        PlayerData retrievedData = null;

        if (Directory.Exists(complete_path)) {
            complete_path = Path.Combine(complete_path, playerSaveFileName + ".game"); // The actual player save file
            if (File.Exists(complete_path)) {
                try {
                    string dataComingIn = "";

                    // Reading the player data in from the player file
                    using (FileStream playerFile = new FileStream(complete_path, FileMode.Open)) {
                        using (StreamReader playerFileReader = new StreamReader(playerFile)) {
                            dataComingIn = playerFileReader.ReadToEnd();
                        }
                    }

                    // Decryption Process if EncryptFile is True
                    if (encryptFile) {
                        dataComingIn = EncryptDecryptplayer(dataComingIn);
                    }
                    
                    // Convert our read data back into an player data object to be used in our game
                    retrievedData = JsonUtility.FromJson<PlayerData>(dataComingIn);

                } catch (Exception e) {
                    Debug.LogError("Error reading the player data file from directory, " + complete_path + "\n" + e);
                }
            }
        }
    
        return retrievedData;
    }

    // Saves our current player/settings into the player file
    public void Save(PlayerData data, string playerSaveFileName) {
        string complete_path = Path.Combine(playerSaveDirectoryPath, playerSaveFileName);
        
        // Create a separate folder for each player save
        Directory.CreateDirectory(complete_path);

        try {
            complete_path = Path.Combine(complete_path, playerSaveFileName + ".game"); // The actual player save file

            // Format and convert our data to JSON to be saved in the player file
            string playerToSave = JsonUtility.ToJson(data, true);

            // Encryption Process if EncryptFile is True
            if (encryptFile) {
                playerToSave = EncryptDecryptplayer(playerToSave);
            }

            // Create player file and write player into file to be saved
            using (FileStream playerFile = new FileStream(complete_path, FileMode.Create)) {
                using (StreamWriter playerFileWriter = new StreamWriter(playerFile)) {
                    playerFileWriter.Write(playerToSave);
                }
            }
            
        } catch (Exception e) {
            Debug.LogError("Error saving the player data to file, " + complete_path + "\n" + e);
        }

    }

    // This function will encrypt and decrypt the saved player in the player File
    private string EncryptDecryptplayer(string data) {
        string encrypted_decrypted_data = "";

        // Encrypting/Dycrypting data using XOR Encryption
        for (int i=0; i < data.Length; i++) {
            encrypted_decrypted_data += (char) (data[i] ^ encryptReadPassword[i % encryptReadPassword.Length]);
        }

        return encrypted_decrypted_data;
    }

    // This function will allow for the saves menu to check if saves exist
    public bool saveExists(string playerSaveFileName) {
        string complete_path = Path.Combine(playerSaveDirectoryPath, playerSaveFileName);
        if (Directory.Exists(complete_path)) {
            complete_path = Path.Combine(complete_path, playerSaveFileName + ".game");
            if (File.Exists(complete_path)) {
                return true;
            }  
        }
        return false;
    }

    // This function creates a mandatory save directory where all the saves will be located
    public void createSaveDirectory() {
        // Creates the directory for the player file to be placed in, if the directory does not already exist
        Directory.CreateDirectory(playerSaveDirectoryPath);
    }
}
