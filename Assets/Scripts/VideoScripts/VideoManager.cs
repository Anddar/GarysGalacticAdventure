using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoManager : MonoBehaviour
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
                Debug.Log("changed");
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

    
    // Resolution & Screen Mode Getter Methods
    public static string getCurrentResolution() {
        return video_resolution;
    }

    
    public static FullScreenMode getCurrentScreenMode() {
        return screen_mode;
    }
    
}
