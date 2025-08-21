using UnityEngine;

[CreateAssetMenu(menuName = "Shooter2D/Movement/ZigZag")]
public class Move_ZigZag : MovementPattern2D
{
    public float forwardSpeed = 2.5f;
    public float zigAmplitude = 2f;
    public float zigFrequency = 2f;
    public Vector2 forwardDir = Vector2.down;

    public override Vector2 EvaluateVelocity(Transform self, Transform player, float t)
    {
        Vector2 perp = new Vector2(-forwardDir.y, forwardDir.x);
        float zig = Mathf.Sin(t * zigFrequency) * zigAmplitude;
        return forwardDir.normalized * forwardSpeed + perp * zig;
    }
}

