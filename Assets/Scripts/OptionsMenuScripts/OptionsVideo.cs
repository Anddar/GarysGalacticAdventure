using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsVideo : MonoBehaviour
{   
    [SerializeField] private TMP_Dropdown resolution_dropdown;
    [SerializeField] private TMP_Dropdown screen_mode_dropdown;

    // Sets the screen resolution when changed in the options menu
    public void setScreenResolution(int index) {
        VideoManager.setGameResolution(resolution_dropdown.options[index].text);
    }

    // Sets the screen mode when changed in the options menu
    public void setScreenMode(int index) {
        VideoManager.setScreenMode(screen_mode_dropdown.options[index].text);
    }

}
