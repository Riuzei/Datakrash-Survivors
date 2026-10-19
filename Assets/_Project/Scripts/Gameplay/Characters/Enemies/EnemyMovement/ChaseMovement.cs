using UnityEngine;

public class ChaseMovement : EnemyMovement
{
    public override Vector2 GetDirection(Vector2 toTarget) => toTarget.normalized;
}
