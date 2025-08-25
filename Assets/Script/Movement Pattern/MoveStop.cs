using UnityEngine;

[CreateAssetMenu(menuName = "MovementPatterns/MoveToPointAndStop")]
public class MoveToPointAndStopPattern : MovementPattern2D
{
    [SerializeField] Vector2 targetPosition = Vector2.zero;
    [SerializeField] float speed = 3f;
    [SerializeField] float stopDistance = 0.1f;

    public override Vector2 EvaluateVelocity(Transform self, Transform player, float t)
    {
        Vector2 current = self.position;
        Vector2 toTarget = targetPosition - current;
        float dist = toTarget.magnitude;

        if (dist <= stopDistance)
        {
            return Vector2.zero; // Already there → stop
        }

        return toTarget.normalized * speed;
    }
}

