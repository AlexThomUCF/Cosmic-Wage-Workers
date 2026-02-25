using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string path => Application.persistentDataPath + "/save.json";

    public static void SaveGame()
    {
        SaveData data = new SaveData();

        data.completedInteractionIDs = new List<string>(CustomerManager.GetCompletedInteractions());
        data.miniGameCount = FinalMiniGame.miniGameCount;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log("Game Saved to: " + path);
    }

    public static void LoadGame()
    {
        if (!File.Exists(path))
        {
            Debug.Log("No save file found.");
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        CustomerManager.SetCompletedInteractions(data.completedInteractionIDs);
        FinalMiniGame.miniGameCount = data.miniGameCount;

        Debug.Log("Game Loaded.");
    }

    public static bool SaveExists()
    {
        return File.Exists(path);
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save Deleted.");
        }
    }
}