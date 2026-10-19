using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    public abstract Vector2 GetDirection(Vector2 toTarget);
}
