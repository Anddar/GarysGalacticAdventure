using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int totalScore;
    public int totalShootSpeedUpgrades;
    public int totalMovementSpeedUpgrades;

    public PlayerData() {
        // Default Options when there is currently no Player Save file storing past playing data
        level = 1;
        totalScore = 0;
        totalShootSpeedUpgrades = 0;
        totalMovementSpeedUpgrades = 0;
    }
}
