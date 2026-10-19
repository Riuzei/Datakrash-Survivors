using UnityEngine;

[CreateAssetMenu(fileName = "GamePrefabs", menuName = "Vampire/GameSaveData")]
public class GameSaveData : ScriptableObject
{
    public static SaveData saveData;
    public static UserData currentUser;
}
