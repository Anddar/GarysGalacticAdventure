using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour, IOptionsDataPersistence
{
    private static bool toggleCrouch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Getter/Flip Toggle Crouch Function
    public static void setToggleCrouch(bool state) {
        toggleCrouch = state;
    }

    public static bool getToggleCrouchState() {
        return toggleCrouch;
    }

    // Options Data Persistence 
    public void LoadData(OptionsData optionsData)
    {
        GameplayManager.toggleCrouch = optionsData.ToggleCrouch;
    }

    public void SaveData(ref OptionsData optionsData)
    {
        optionsData.ToggleCrouch = GameplayManager.toggleCrouch;
    }
}
