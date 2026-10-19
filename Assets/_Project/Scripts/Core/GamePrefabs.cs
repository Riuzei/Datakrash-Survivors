using UnityEngine;

[CreateAssetMenu(fileName = "GamePrefabs", menuName = "Vampire/GamePrefabs")]
public class GamePrefabs : ScriptableObject
{
    public GameObject playerPrefab;
    public EnemyBrain enemyRedPrefab;
    public EnemyBrain enemyGreenPrefab;
    public EnemyBrain enemyBluePrefab;
    public Weapon orbitWeaponPrefab;

    public XPGem commonGem;
    public XPGem rareGem;
    public XPGem legendaryGem;
    
    public HealPickup healPickup;
    public ParticleSystem healVFX;
    public XPGem GetGem(EGemType type)
    {
        switch (type)
        {
            case EGemType.Common:    
                return commonGem;
            case EGemType.Rare:      
                return rareGem;
            case EGemType.Legendary: 
                return legendaryGem;
            default:                
                return null;
        }
    }
}
