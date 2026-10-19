using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class EnemyBuilder
{
    private readonly Transform parent;
    private readonly List<Type> attributes = new List<Type>();
    private EnemyBrain basePrefab;

    public EnemyBuilder(Transform parent)
    {
        this.parent = parent;
    }

    public EnemyBuilder SetBase(EnemyBrain prefab)
    {
        basePrefab = prefab;
        attributes.Clear();
        return this;
    }

    public EnemyBuilder WithAttribute<T>() where T : Component
    {
        attributes.Add(typeof(T));
        return this;
    }

    public EnemyBrain Build()
    {
        EnemyBrain enemy = Object.Instantiate(basePrefab, parent);
        foreach (Type attribute in attributes)
            enemy.gameObject.AddComponent(attribute);

        enemy.gameObject.SetActive(false);
        attributes.Clear();
        return enemy;
    }
}
