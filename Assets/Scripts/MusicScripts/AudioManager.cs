using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Game Volumes
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
    }

    // Volume Getter Methods
    public static float getMasterVolume() {
        return (float) master_volume;
    }

    public static float getSoundFXVolume() {
        return (float) sound_fx_volume;
    }

    public static float getMusicVolume() {
        return (float) music_volume;
    }

    public static float getDialogueVolume() {
        return (float) dialogue_volume;
    }
}
