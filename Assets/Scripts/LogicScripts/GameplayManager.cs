using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private static bool toggleCrouch;

    // Start is called before the first frame update
    void Start()
    {
        toggleCrouch = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Getter/Flip Toggle Crouch Function
    public static void flipToggleCrouch() {
        toggleCrouch = !toggleCrouch;
    }

    public static bool getToggleCrouchState() {
        return toggleCrouch;
    }

}
