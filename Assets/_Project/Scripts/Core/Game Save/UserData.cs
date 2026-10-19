using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public string name;
    public float highScore;
    public int screenMode;
    public float masterVol = 1f;
    public float musicVol = 1f;
    public float sfxVol = 1f;
}

[Serializable]
public class SaveData
{
    public List<UserData> users = new List<UserData>();
}
