using UnityEngine;

[CreateAssetMenu(menuName = "Shooter2D/Movement/OrbitPlayer")]
public class Move_OrbitPlayer : MovementPattern2D
{
    public float orbitRadius = 4f;
    public float angularSpeedDeg = 90f;
    public float centerChaseLerp = 2f; // how fast we slide toward orbit center

    public override Vector2 EvaluateVelocity(Transform self, Transform player, float t)
    {
        if (!player) return Vector2.zero;
        Vector2 toPlayer = (Vector2)player.position - (Vector2)self.position;

        // Move toward the ring (radius) then apply tangential velocity
        float dist = toPlayer.magnitude;
        Vector2 towardCenter = toPlayer.normalized * (dist - orbitRadius) * centerChaseLerp;

        // Tangential velocity
        Vector2 tangent = new Vector2(-toPlayer.y, toPlayer.x).normalized * (angularSpeedDeg * Mathf.Deg2Rad);

        return towardCenter + tangent;
    }
}
