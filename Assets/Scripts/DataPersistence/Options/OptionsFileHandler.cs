using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class OptionsFileHandler
{
    private string optionsDirectoryPath = "";
    private string optionsFileName = "";

    public OptionsFileHandler(string optionsDirectoryPath, string optionsFileName) {
        this.optionsDirectoryPath = optionsDirectoryPath;
        this.optionsFileName = optionsFileName;
    }

    // Loads our past options from the existing options file
    public OptionsData Load() {
        string complete_path = Path.Combine(optionsDirectoryPath, optionsFileName);
        OptionsData retrievedData = null;

        if (File.Exists(complete_path)) {
            try {
                string dataComingIn = "";

                // Reading the options data in from the options file
                using (FileStream optionsFile = new FileStream(complete_path, FileMode.Open)) {
                    using (StreamReader optionsFileReader = new StreamReader(optionsFile)) {
                        dataComingIn = optionsFileReader.ReadToEnd();
                    }
                }
                
                // Convert our read data back into an options data object to be used in our game
                retrievedData = JsonUtility.FromJson<OptionsData>(dataComingIn);

            } catch (Exception e) {
                Debug.LogError("Error reading the options data file from directory, " + complete_path + "\n" + e);
            }
        }
    
        return retrievedData;
    }

    // Saves our current options/settings into the options file
    public void Save(OptionsData data) {
        string complete_path = Path.Combine(optionsDirectoryPath, optionsFileName);
        try {
            // Creates the directory for the options file to be placed in, if the directory does not already exist
            Directory.CreateDirectory(Path.GetDirectoryName(complete_path));

            // Format and convert our data to JSON to be saved in the options file
            string optionsToSave = JsonUtility.ToJson(data, true);

            // Create options file and write options into file to be saved
            using (FileStream optionsFile = new FileStream(complete_path, FileMode.Create)) {
                using (StreamWriter optionsFileWriter = new StreamWriter(optionsFile)) {
                    optionsFileWriter.Write(optionsToSave);
                }
            }
            
        } catch (Exception e) {
            Debug.LogError("Error saving the options data to file, " + complete_path + "\n" + e);
        }

    }
}