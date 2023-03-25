using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsSliderValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Text sliderLabel;

    void Start() {
        // If the Options Menu sliders are Volume Sliders this switch statement will update their values on the slider to values found from a options file, 
        // if not an Options file then a default value is applied by the OptionsData Class
        switch(sliderLabel.text) {
            case "MASTER VOLUME":
                gameObject.GetComponent<Slider>().value = AudioManager.getRawMasterVolume();
                break;
            case "SOUND FX VOLUME":
                gameObject.GetComponent<Slider>().value = AudioManager.getRawSoundFXVolume();
                break;
            case "MUSIC VOLUME":
                gameObject.GetComponent<Slider>().value = AudioManager.getRawMusicVolume();
                break;
            case "DIALOGUE VOLUME":
                gameObject.GetComponent<Slider>().value = AudioManager.getRawDialogueVolume();
                break;
        }
    }

    // Updates the sliders text
    public void setSliderValue(float value) {
        valueText.text = value.ToString();
        if (sliderLabel.text.Contains("VOLUME")) {
            AudioManager.updateAudioLevel(sliderLabel.text, (int) value);
        }
    }
}
