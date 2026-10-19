public class BuildingBrain : BaseEntityBrain
{
    private Pickup pickup;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if(currentHealth <= 0)
            Die();
    }

    public override void Die()
    {
        base.Die();
        pickup = GameplayManager.Instance.gamePrefabs.healPickup;
        GameplayManager.Instance.pickupSpawner.Spawn(pickup, transform.position);
        Destroy(gameObject);
    }
}
