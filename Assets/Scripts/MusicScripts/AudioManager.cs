using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    private static double master_volume = 1;
    private static double sound_fx_volume = 1;
    private static double music_volume = 1;
    private static double dialogue_volume = 1;

    // This function can be called when player updates audio settings to change volumes.
    public static void updateAudioLevel(string audioSource, double value) {
        switch (audioSource) {
            case "MASTER VOLUME":
                master_volume = value * 0.01;
                sound_fx_volume = (value * 0.01) * master_volume;
                music_volume = (value * 0.01) * master_volume;
                dialogue_volume = (value * 0.01) * master_volume;
                break;

            case "SOUND FX VOLUME":
                sound_fx_volume = (value * 0.01) * master_volume;
                break;

            case "MUSIC VOLUME":
                music_volume = (value * 0.01) * master_volume;
                break;

            case "DIALOGUE VOLUME":
                dialogue_volume = (value * 0.01) * master_volume;
                break;
        }
        //Debug.Log("Master: " + master_volume + ", SFX: " + sound_fx_volume + ", Music: " + music_volume + ", Dialogue: " + dialogue_volume);
    }

    // Volume Getter Methods
    public static double getMasterVolume() {
        return master_volume;
    }

    public static double getSoundFXVolume() {
        return sound_fx_volume;
    }

    public static double getMusicVolume() {
        return music_volume;
    }

    public static double getDialogueVolume() {
        return dialogue_volume;
    }
}
