using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IEnemyFactory
{
    EnemyBrain Spawn(EEnemyType type, Vector2 position);
}

public class EnemyFactory : IEnemyFactory
{
    private readonly Dictionary<EEnemyType, ObjectPool<EnemyBrain>> poolByEnemyType = new();
    private readonly EnemyBuilder builder;
    private readonly float despawnDistanceSqr;

    public EnemyFactory(GamePrefabs prefabs, Transform parent, float despawnDistance)
    {
        despawnDistanceSqr = despawnDistance * despawnDistance;
        builder = new EnemyBuilder(parent);

        Register(EEnemyType.Blue, () => builder
            .SetBase(prefabs.enemyBluePrefab)
            .WithAttribute<ChaseMovement>()
            .Build());

        Register(EEnemyType.Yellow, () => builder
            .SetBase(prefabs.enemyGreenPrefab)
            .WithAttribute<OrbitMovement>()
            .Build());

        Register(EEnemyType.Red, () => builder
            .SetBase(prefabs.enemyRedPrefab)
            .WithAttribute<ChaseMovement>()
            .Build());
    }

    public EnemyBrain Spawn(EEnemyType type, Vector2 position)
    {
       if (!poolByEnemyType.TryGetValue(type, out ObjectPool<EnemyBrain> pool))
            return null;
        EnemyBrain enemy = pool.Get();
        enemy.transform.SetPositionAndRotation(position, Quaternion.identity);
        enemy.DespawnDistanceSqr = despawnDistanceSqr;
        enemy.ResetHealth();
        enemy.gameObject.SetActive(true);
        return enemy;
    }

    private void Register(EEnemyType type, Func<EnemyBrain> build)
    {
        ObjectPool<EnemyBrain> pool = null;
        pool = new ObjectPool<EnemyBrain>
        (
            () =>
            {
                EnemyBrain enemy = build();
                enemy.EnemyPool = pool;
                return enemy;
            },
            enemy => { },
            enemy => enemy.gameObject.SetActive(false),
            enemy => UnityEngine.Object.Destroy(enemy.gameObject),
            true, 
            20, 
            500
        );

        poolByEnemyType[type] = pool;
    }

}