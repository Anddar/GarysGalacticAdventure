using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsSliderValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Text sliderLabel;

    // Updates the sliders text
    public void setSliderValue(float value) {
        valueText.text = value.ToString();
        if (sliderLabel.text.Contains("VOLUME")) {
            AudioManager.updateAudioLevel(sliderLabel.text, value);
        }
    }
}
