using UnityEngine;

[CreateAssetMenu(menuName = "MovementPatterns/Capsule")]
public class CapsulePattern : MovementPattern2D
{
    [SerializeField] float width = 3f;   // horizontal extent (capsule half-width)
    [SerializeField] float height = 1.5f; // vertical extent (capsule half-height / radius of rounded ends)
    [SerializeField] float speed = 1f;    // how fast to loop

    public override Vector2 EvaluateVelocity(Transform self, Transform player, float t)
    {
        // Parametric "capsule" path = horizontal line + semicircular arcs on ends
        // We'll move around it using sine/cosine
        float cycleTime = (2 * width + Mathf.PI * height) / speed; // perimeter / speed
        float u = (t % cycleTime) / cycleTime; // normalized time [0,1)

        // Decide which segment we're on
        float perimeter = 2 * width + Mathf.PI * height;
        float dist = u * perimeter;

        // Move right along the top line
        if (dist < width)
        {
            return Vector2.right * speed;
        }
        // Top-right semicircle
        else if (dist < width + Mathf.PI * height / 2f)
        {
            float theta = (dist - width) / height; // 0→π/2
            return new Vector2(Mathf.Cos(theta), -Mathf.Sin(theta)) * speed;
        }
        // Move left along the bottom line
        else if (dist < width + Mathf.PI * height / 2f + width)
        {
            return Vector2.left * speed;
        }
        // Bottom-left semicircle
        else
        {
            float theta = (dist - (2 * width + Mathf.PI * height / 2f)) / height; // 0→π/2
            return new Vector2(-Mathf.Cos(theta), Mathf.Sin(theta)) * speed;
        }
    }
}

