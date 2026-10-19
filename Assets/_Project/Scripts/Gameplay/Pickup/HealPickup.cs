using UnityEngine;

public class HealPickup : Pickup
{
    [SerializeField] private float healValue;
    protected override void PickupItem()
    {
        GameplayManager.Instance.player.currentHero.Heal(healValue);
        ReturnToPool();
    }

}