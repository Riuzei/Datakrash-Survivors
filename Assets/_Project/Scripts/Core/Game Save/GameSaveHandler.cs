using System.IO;
using UnityEngine;

public class GameSaveHandler : MonoBehaviour
{
    public static SaveData saveData;
    public static UserData currentUser;

    public static GameSaveHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadSave();
    }
    
#region Create/Login
    public static bool CreateUser(string username)
    {
        UserData _user = saveData.users.Find(user => user.name == username);
        if (_user != null)
        {
            return false;
        }
        else
        {
            UserData newUser = new UserData { name = username, highScore = 0 };
            saveData.users.Add(newUser);
            SaveToDisk(saveData);
            currentUser = newUser;
            return true;
        }
    }

    public static bool LoginUser(string username)
    {
        UserData _user = saveData.users.Find(user => user.name == username);
        if (_user == null)
        {
            return false;
        }
        currentUser = _user;
        return true;
    }
#endregion

#region Load/Save

    public static void Save()
    {
        SaveToDisk(saveData);
    }
    static void SaveToDisk(SaveData _saveData)
    {
        string json = JsonUtility.ToJson(_saveData);
        Debug.Log(json);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "save.txt"), json);
    }

    public static void LoadSave()
    {
        string path = Path.Combine(Application.persistentDataPath, "save.txt");

        if (File.Exists(path))
        {
            try
            {
                string saveString = File.ReadAllText(path);
                saveData = JsonUtility.FromJson<SaveData>(saveString);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Save file corrupted, starting fresh: " + e.Message);
                File.Copy(path, path + ".bak", true); // keep the broken file for inspection
                saveData = new SaveData();
            }
        }
        else
        {
            saveData = new SaveData();
            SaveToDisk(saveData);
        }

        if (saveData == null) saveData = new SaveData();
    }
#endregion

#region Get/Set Value
    public static void SetFloat(string key, float value)
    {
        switch (key)
        {
            case "HighScore":
                currentUser.highScore = value;
                break;
            case "MasterVolume":
                currentUser.masterVol = value;
                break;
            case "MusicVolume":
                currentUser.masterVol = value;
                break;
            case "SFXVolume":
                currentUser.masterVol = value;
                break;  
            default:
                Debug.LogWarning("Unknown key: " + key);
                return;
        }
        SaveToDisk(saveData);
    }
    public static float GetFloat(string key)
    {
        if(currentUser == null)
            Debug.LogError("User not found");
        switch (key)
        {
            case "HighScore":
                return currentUser.highScore;
            case "MasterVolume":
                return currentUser.masterVol;
            case "MusicVolume":
                return currentUser.musicVol;
            case "SFXVolume":
                return currentUser.sfxVol;
            default:
                return 0;
        }
    }
    public static void SetInt(string key, int value)
    {
        switch (key)
        {
            case "ScreenMode":
                currentUser.screenMode = value;
                break;
            default:
                Debug.LogWarning("Unknown key: " + key);
                return;
        }
        SaveToDisk(saveData);
    }
    public static int GetInt(string key, int defaultValue)
    {
        if (currentUser == null) return defaultValue;

        switch (key)
        {
            case "ScreenMode":
                return currentUser.screenMode;
            default:
                Debug.LogWarning("Unknown key: " + key);
                return defaultValue;
        }
    }
#endregion

}