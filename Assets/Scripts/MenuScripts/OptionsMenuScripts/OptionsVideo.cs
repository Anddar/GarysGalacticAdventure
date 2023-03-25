using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsVideo : MonoBehaviour
{   
    [SerializeField] private TMP_Dropdown resolution_dropdown;
    [SerializeField] private TMP_Dropdown screen_mode_dropdown;

    void Start() {
        // Setting the dropdown menus to have the proper resolution and screen mode selected from the dropdown since we are reading in the users Options from an Option file. If not
        // from an Options file a default value is used, set by OptionsData Class
        int i = 0;
        foreach (TMP_Dropdown.OptionData option in resolution_dropdown.options) {
            if (option.text.Equals(VideoManager.getCurrentResolution())) {
                resolution_dropdown.value = i;
                break;
            }
            ++i;
        }

        i = 0;
        foreach (TMP_Dropdown.OptionData option in screen_mode_dropdown.options) {
            if (option.text.Equals(VideoManager.getCurrentScreenModeString())) {
                screen_mode_dropdown.value = i;
                break;
            }
            ++i;
        }
    }

    void Update() {
        // Turning off the resolution dropdown so resolution cannot be changed when in Windowed Screen Mode
        if (VideoManager.getCurrentScreenMode() == FullScreenMode.Windowed) {
            resolution_dropdown.interactable = false;
        } else if (!resolution_dropdown.interactable) { 
            resolution_dropdown.interactable = true; 
        }
    }
     
    // Sets the screen resolution when changed in the options menu
    public void setScreenResolution(int index) {
        VideoManager.setGameResolution(resolution_dropdown.options[index].text);
    }

    // Sets the screen mode when changed in the options menu
    public void setScreenMode(int index) {
        VideoManager.setScreenMode(screen_mode_dropdown.options[index].text);
    }

}
