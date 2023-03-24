using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOptionsDataPersistence
{
    // Allows the script implementing this interface to load data
    void LoadData(OptionsData optionsData);

    // Allows the script implementing this interface to save data
    void SaveData(ref OptionsData optionsData);
}
