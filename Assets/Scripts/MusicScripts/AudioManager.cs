using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IOptionsDataPersistence
{
    // Game Volumes
    private static int master_volume;
    private static int sound_fx_volume;
    private static int music_volume;
    private static int dialogue_volume;
    private static int ui_volume;

    // This function can be called when player updates audio settings to change volumes.
    public static void updateAudioLevel(string audioSource, int value) {
        switch (audioSource) {
            case "MASTER VOLUME":
                master_volume = value;
                break;

            case "SOUND FX VOLUME":
                sound_fx_volume = value;
                break;

            case "MUSIC VOLUME":
                music_volume = value;
                break;

            case "DIALOGUE VOLUME":
                dialogue_volume = value;
                break;

            case "UI VOLUME":
                ui_volume = value;
                break;
        }
    }

    // Volume Getter Methods
    public static float getMasterVolume() {
        return (float) (master_volume * 0.01);
    }

    public static float getSoundFXVolume() {
        return (float) ((sound_fx_volume * 0.01) * (master_volume * 0.01));
    }

    public static float getMusicVolume() {
        return (float) ((music_volume * 0.01) * (master_volume * 0.01));
    }

    public static float getDialogueVolume() {
        return (float) ((dialogue_volume * 0.01) * (master_volume * 0.01));
    }

    public static float getUIVolume() {
        return (float) (ui_volume * 0.01);
    }

    // Returns the raw sound value that is set in the Options Menu
    public static int getRawMasterVolume() {
        return master_volume;
    }

    public static int getRawSoundFXVolume() {
        return sound_fx_volume;
    }

    public static int getRawMusicVolume() {
        return music_volume;
    }

    public static int getRawDialogueVolume() {
        return dialogue_volume;
    }

    public static int getRawUIVolume() {
        return ui_volume;
    }

    // Options Data Persistence
    public void LoadData(OptionsData optionsData)
    {
        AudioManager.master_volume = optionsData.master_volume;
        AudioManager.sound_fx_volume = optionsData.sound_fx_volume;
        AudioManager.music_volume = optionsData.music_volume;
        AudioManager.dialogue_volume = optionsData.dialogue_volume;
        AudioManager.ui_volume = optionsData.ui_volume;
    }

    public void SaveData(ref OptionsData optionsData)
    {
        optionsData.master_volume = AudioManager.master_volume;
        optionsData.sound_fx_volume = AudioManager.sound_fx_volume;
        optionsData.music_volume = AudioManager.music_volume;
        optionsData.dialogue_volume = AudioManager.dialogue_volume;
        optionsData.ui_volume = AudioManager.ui_volume;
    }
}
