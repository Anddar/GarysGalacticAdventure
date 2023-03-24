using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsData
{
    public bool ToggleCrouch;

    public string GameResolution;
    public int GameWindowMode;

    public int master_volume;
    public int sound_fx_volume;
    public int music_volume;
    public int dialogue_volume;

    public OptionsData() {
        // Default Options when there is currently no user_options.game file storing past options data
        ToggleCrouch = true;

        GameResolution = "1920 x 1080";
        GameWindowMode = 0;

        master_volume = 100;
        sound_fx_volume = 100;
        music_volume = 100;
        dialogue_volume = 100;
    }

}
