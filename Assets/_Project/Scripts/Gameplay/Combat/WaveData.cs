using UnityEngine;
 
public enum EEnemyType { Blue, Yellow, Red }
[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public EEnemyType enemyType;   
        public float weight = 100f;    
    }
 
    [System.Serializable]
    public class Wave
    {
        public float startTime;       
        public float spawnInterval = 1f;
        public Entry[] entries;
    }
 
    public Wave[] waves;               

    public EEnemyType PickEnemyType(Wave wave)
    {
        float totalWeight = 0f;
        foreach (Entry entry in wave.entries)
            totalWeight += entry.weight;

        float roll = Random.Range(0f, totalWeight);

        foreach (Entry entry in wave.entries)
        {
            if (roll < entry.weight)
                return entry.enemyType;
            roll -= entry.weight;
        }
        return wave.entries[0].enemyType;
    }
}
