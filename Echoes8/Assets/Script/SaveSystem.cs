using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "save.json");

    public static void SaveData(GameData data)
    {
        try
        {
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, jsonData);
            Debug.Log($"Game data saved to: {SavePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving game data: {e.Message}");
        }
    }

    public static GameData LoadData()
    {
        try
        {
            if (File.Exists(SavePath))
            {
                string jsonData = File.ReadAllText(SavePath);
                Debug.Log($"Game data loaded from: {SavePath}");
                return JsonUtility.FromJson<GameData>(jsonData);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading game data: {e.Message}");
        }

        Debug.Log("No save file found. Creating new GameData.");
        return new GameData();
    }
}