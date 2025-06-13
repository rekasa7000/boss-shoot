using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Save system for our game
/// </summary>
public class SaveSystem : MonoBehaviour
{
    //We areu using PlayerPrefs so here is the default key that we use
    [SerializeField]
    private string _saveKeyName = "SaveGame";

    public void SaveTheGame(List<string> dataToSave)
    {
        SavedData data = new SavedData
        {
            savedData = dataToSave,
        };
        PlayerPrefs.SetString(_saveKeyName, JsonUtility.ToJson(data));
        Debug.Log("Game Saved");
    }

    public List<string>  LoadGame()
    {
        if (PlayerPrefs.HasKey(_saveKeyName) == false)
            return new();
        SavedData data = JsonUtility.FromJson<SavedData>(PlayerPrefs.GetString(_saveKeyName));
        return data.savedData;
        
    }

    public void ResetData()
        => PlayerPrefs.DeleteKey(_saveKeyName);



    [Serializable]
    private struct SavedData
    {
        public List<string> savedData;
    }
}
