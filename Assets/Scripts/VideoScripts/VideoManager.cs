using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoManager : MonoBehaviour, IOptionsDataPersistence
{
    private static string video_resolution;
    private static FullScreenMode screen_mode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Allows the options menu handlers to set the game resolution in through the video manager
    public static void setGameResolution(string resolution) {
        switch(resolution) {
            case "1920 x 1080":
                video_resolution = "1920 x 1080";
                Screen.SetResolution(1920, 1080, screen_mode);
                break;

            case "1366 x 768":
                video_resolution = "1366 x 768";
                Screen.SetResolution(1366, 768, screen_mode);
                break;

            case "1280 x 720":
                video_resolution = "1280 x 720";
                Screen.SetResolution(1280, 720, screen_mode);
                break;
        }
    }

    // Allows the options menu handlers to set the screen mode in through the video manager
    public static void setScreenMode(string mode) {
        switch(mode) {
            case "FULLSCREEN":
                screen_mode = FullScreenMode.ExclusiveFullScreen;
                Screen.fullScreenMode = screen_mode;
                break;

            case "FULLSCREEN (BORDERLESS)":
                screen_mode = FullScreenMode.FullScreenWindow;
                Screen.fullScreenMode = screen_mode;
                break;

            case "WINDOWED":
                screen_mode = FullScreenMode.Windowed;
                Screen.fullScreenMode = screen_mode;
                break;
        }
    }

    private static string convertIntScreenModeToString(int constant) {
        switch(constant) {
            case 0:
                return "FULLSCREEN";
            case 1:
                return "FULLSCREEN (BORDERLESS)";
            case 2:
                return "MAXIMIZED WINDOW";
            case 3:
                return "WINDOWED";
        }
        return "";
    }

    private static FullScreenMode convertIntToScreenMode(int constant) {
        switch(constant) {
            case 0:
                return FullScreenMode.ExclusiveFullScreen;
            case 1:
                return FullScreenMode.FullScreenWindow;
            case 2:
                return FullScreenMode.MaximizedWindow;
            case 3:
                return FullScreenMode.Windowed;
        }
        return FullScreenMode.ExclusiveFullScreen;
    }
    
    // Resolution & Screen Mode Getter Methods
    public static string getCurrentResolution() {
        return video_resolution;
    }

    public static FullScreenMode getCurrentScreenMode() {
        return screen_mode;
    }

    public static string getCurrentScreenModeString() {
        return convertIntScreenModeToString((int) screen_mode);
    }

    // Options Data Persistence
    public void LoadData(OptionsData optionsData)
    {
        video_resolution = optionsData.GameResolution;
        screen_mode = convertIntToScreenMode(optionsData.GameWindowMode);

        // Set Game Resolution and Screen Mode accordingly, if not default, to the Read Options File, if not default options are applied by the OptionsData Class
        setGameResolution(video_resolution);
        
        if (screen_mode != Screen.fullScreenMode) {
            setScreenMode(convertIntScreenModeToString(optionsData.GameWindowMode));
        }
    }

    public void SaveData(ref OptionsData optionsData)
    {
        optionsData.GameResolution = video_resolution;
        optionsData.GameWindowMode = (int) screen_mode;
    }
}
