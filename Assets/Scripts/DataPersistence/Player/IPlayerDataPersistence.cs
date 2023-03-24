using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDataPersistence
{
    // Allows the script implementing this interface to load data
    void LoadData(PlayerData playerData);

    // Allows the script implementing this interface to save data
    void SaveData(ref PlayerData playerData);
}
