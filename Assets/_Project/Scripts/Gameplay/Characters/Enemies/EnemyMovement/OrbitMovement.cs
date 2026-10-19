using UnityEngine;

public class OrbitMovement : EnemyMovement
{
    [SerializeField, Range(0f, 1f)] private float inwardPull = 0.25f;
    private float orbitSign;

    void OnEnable() => orbitSign = Random.value < 0.5f ? 1f : -1f;

    public override Vector2 GetDirection(Vector2 toTarget)
    {
        Vector2 inward = toTarget.normalized;
        Vector2 sideways = new Vector2(-inward.y, inward.x) * orbitSign;
        return (sideways + inward * inwardPull).normalized;
    }
}
