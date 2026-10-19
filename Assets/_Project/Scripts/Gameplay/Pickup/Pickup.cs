using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class Pickup : MonoBehaviour
{
    public IObjectPool<Pickup> Pool;

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        PickupItem();
    }

    protected virtual void PickupItem() { }

    protected void ReturnToPool()
    {
       Pool.Release(this);
    }
}