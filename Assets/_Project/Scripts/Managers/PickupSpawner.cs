using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PickupSpawner : MonoBehaviour
{
    private readonly Dictionary<Pickup, ObjectPool<Pickup>> poolByPrefab = new();

    public void Spawn(Pickup prefab, Vector2 position)
    {
        if (!poolByPrefab.TryGetValue(prefab, out ObjectPool<Pickup> pool))
        {
            pool = CreatePool(prefab);
            poolByPrefab[prefab] = pool;
        }
        Pickup pickup = poolByPrefab[prefab].Get();     
        pickup.transform.position = position;
        pickup.gameObject.SetActive(true); 
    }

    private ObjectPool<Pickup> CreatePool(Pickup prefab)
    {
        Debug.Log("Creating Pool");
        ObjectPool<Pickup> pool = null;
        pool = new ObjectPool<Pickup>(
            () =>
            {
                Pickup pickup = Instantiate(prefab, transform);
                pickup.Pool = pool;
                pickup.gameObject.SetActive(false); 
                return pickup;
            },
            pickup => { },
            pickup => pickup.gameObject.SetActive(false),
            pickup => Destroy(pickup.gameObject),
            true, 
            50, 
            2000);
        return pool;
    }
}