using UnityEngine;

[CreateAssetMenu(menuName = "MovementPatterns/HorizontalLeftRight")]
public class HorizontalLeftRightPattern : MovementPattern2D
{
    [SerializeField] float speed = 3f;

    public override Vector2 EvaluateVelocity(Transform self, Transform player, float t)
    {
        // Constant horizontal velocity, positive X = right
        return new Vector2(speed, 0f);
    }
}
