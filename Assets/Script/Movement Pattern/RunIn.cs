using UnityEngine;

[CreateAssetMenu(menuName = "MovementPatterns/KamikazeChaseSmooth2D")]
public class KamikazeChaseSmooth2D : MovementPattern2D
{
    [SerializeField] float speed = 6f;
    [SerializeField] float turnRate = 360f;
    [SerializeField] float leadTime = 0.2f;

    Vector2 currentDir = Vector2.down;

    public override Vector2 EvaluateVelocity(Transform self, Transform player, float t)
    {
        if (!player) return currentDir * speed;

        Vector2 targetPos = (Vector2)player.position;
        if (leadTime > 0f)
        {
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb) targetPos += rb.velocity * leadTime;
        }

        Vector2 desiredDir = (targetPos - (Vector2)self.position).normalized;

        float maxRadians = turnRate * Mathf.Deg2Rad * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(
            (Vector3)currentDir,
            (Vector3)desiredDir,
            maxRadians,
            0f
        );

        currentDir = new Vector2(newDir.x, newDir.y).normalized;

        return currentDir * speed;
    }
}


