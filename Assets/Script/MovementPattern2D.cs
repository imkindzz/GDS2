// MovementPattern2D.cs
using UnityEngine;

public abstract class MovementPattern2D : ScriptableObject
{
    public abstract Vector2 EvaluateVelocity(Transform self, Transform player, float t);
}
