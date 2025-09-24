using UnityEngine;

[CreateAssetMenu(menuName = "MovementPatterns/ChargePauseChase2D")]
public class ChargePauseChase2D : MovementPattern2D
{
    [SerializeField] float chargeSpeed = 10f;
    [SerializeField] float chargeTime = 0.5f;
    [SerializeField] float pauseTime = 0.35f;

    public override Vector2 EvaluateVelocity(Transform self, Transform player, float t)
    {
        if (!player) return Vector2.zero;

        float cycle = chargeTime + pauseTime;
        float u = t % cycle;

        if (u < chargeTime)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)self.position);
            if (dir.sqrMagnitude < 1e-6f) return Vector2.zero;
            return dir.normalized * chargeSpeed;
        }
        else
        {
            return Vector2.zero;
        }
    }
}
