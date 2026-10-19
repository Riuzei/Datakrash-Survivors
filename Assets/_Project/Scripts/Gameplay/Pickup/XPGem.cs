using UnityEngine;

public class XPGem : Pickup
{
    [SerializeField] EGemType gemType;

    protected override void PickupItem()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.xpPickup);
        ProgressionManager.Instance.GainXP(GetGemValue(gemType));
        ReturnToPool();
    }

    int GetGemValue(EGemType _gemType)
    {
        switch (_gemType)
        {
            case EGemType.Common:
                return Random.Range(1, 2);
            case EGemType.Rare:
                return Random.Range(3, 9);
            case EGemType.Legendary:
                return Random.Range(10, 20);
            default:
                return 0;
        }
    }

}