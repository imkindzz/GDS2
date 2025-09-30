using UnityEngine;

[CreateAssetMenu(menuName = "BulletPatterns/ThrustLine")]
public class ThrustLine : BulletPattern
{
    [SerializeField] float length = 2.5f;
    [SerializeField] int segments = 6;
    [SerializeField] float lifetime = 0.12f;

    public override void Emit(Transform emitterTransform, GameObject bulletPrefab, Vector3? targetPosition = null)
    {
        if (!bulletPrefab || targetPosition == null) return;

        // aim angle toward player
        Vector3 toTarget = targetPosition.Value - emitterTransform.position;
        float baseAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
        Quaternion aimRot = Quaternion.Euler(0, 0, baseAngle - 90f);
        Vector2 fwd = new Vector2(Mathf.Cos(baseAngle * Mathf.Deg2Rad), Mathf.Sin(baseAngle * Mathf.Deg2Rad));

        Vector3 pos = emitterTransform.position;
        float step = length / Mathf.Max(1, segments);

        for (int i = 1; i <= segments; i++)
        {
            Vector3 p = pos + (Vector3)(fwd * (step * i));
            GameObject g = Object.Instantiate(bulletPrefab, p, aimRot);
            var b = g.GetComponent<Bullet>();
            if (b)
            {
                b.speed = 0f;                          // static hitbox
                b.velocity = Vector2.up;               // still needs a "direction"
                b.rotation = aimRot.eulerAngles.z;
                b.lifetime = lifetime;
            }
        }
    }
}
