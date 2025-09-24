using UnityEngine;

[CreateAssetMenu(menuName = "BulletPatterns/SwordSwingArc")]
public class SwordSwingArc : BulletPattern
{
    [Header("Swing Shape")]
    public float swingAngle = 120f;       // total degrees swept
    public float swingDuration = 0.25f;   // time to sweep
    public bool clockwise = true;
    public float arcRadius = 1.2f;        // distance from emitter (melee reach)

    [Header("Spawn")]
    public int bulletsPerSwing = 24;      // trail density
    public bool centerOnTarget = true;    // face target at start, else use emitter's facing
    public float startAngleOffset = 0f;   // shift the swing (e.g., -30 to bias forward)

    [Header("Hitbox Bullet")]
    public float bulletLifetime = 0.12f;  // quick, like a hitbox
    public float bulletSpeed = 0f;        // 0 = stationary hitbox at arc radius

    public override void Emit(Transform emitterTransform, GameObject bulletPrefab, Vector3? targetPosition = null)
    {
        if (!bulletPrefab || !emitterTransform) return;

        float baseAngleDeg;
        if (centerOnTarget && targetPosition != null)
        {
            Vector3 dir = targetPosition.Value - emitterTransform.position;
            baseAngleDeg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        else
        {
            // emitter's current facing where +Y is forward in your bullet setup
            baseAngleDeg = emitterTransform.eulerAngles.z + 90f;
        }
        baseAngleDeg += startAngleOffset;

        float half = swingAngle * 0.5f;
        float start = baseAngleDeg + (clockwise ? +half : -half);
        float end = baseAngleDeg + (clockwise ? -half : +half);

        var host = new GameObject("SwordSwingHost");
        host.transform.position = emitterTransform.position;
        host.transform.rotation = emitterTransform.rotation;

        var impl = host.AddComponent<SwordSwingArcImpl>();
        impl.Setup(
            bulletPrefab,
            host.transform,
            start,
            end,
            swingDuration,
            bulletsPerSwing,
            arcRadius,
            bulletSpeed,
            bulletLifetime
        );
    }
}
