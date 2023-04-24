using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionStates : MonoBehaviour
{
    // Returns if the current level given is complete or not
    public static bool isLevelComplete() {
        switch (SceneManager.GetActiveScene().buildIndex) {
            case 1:
                return FinishLevel1.isLevelComplete();
            case 2:
                return Takeoff.isLevelComplete();
        }
        return false;
    }
}
