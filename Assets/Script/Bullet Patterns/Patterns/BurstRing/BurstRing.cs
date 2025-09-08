using UnityEngine;

[CreateAssetMenu(menuName = "BulletPatterns/SingularityPulseShot")]
public class SingularityPulseShot : BulletPattern
{
    public float seedSpeed = 8f;
    public float seedTravelTime = 0.6f;
    public float coreDuration = 1.2f;

    public int pulses = 3;
    public float pulseInterval = 0.25f;
    public int bulletsPerPulse = 10;
    public float pulseSpeed = 7f;
    public float pulseLifetime = 1.0f;
    public float coreScale = 1.0f;
    public float pulseScale = 0.9f;
    public float spinPerPulse = 18f;

    public override void Emit(Transform emitterTransform, GameObject bulletPrefab, Vector3? targetPosition = null)
    {
        if (!bulletPrefab || targetPosition == null) return;

        Vector3 dir = targetPosition.Value - emitterTransform.position;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, baseAngle - 90f);

        GameObject seed = Object.Instantiate(bulletPrefab, emitterTransform.position, rot);
        var b = seed.GetComponent<Bullet>();
        if (b)
        {
            b.speed = seedSpeed;
            b.velocity = Vector2.up;
            b.rotation = rot.eulerAngles.z;
            b.lifetime = seedTravelTime + coreDuration + pulses * pulseInterval + 0.1f;
        }

        var impl = seed.AddComponent<SingularityPulseImpl>();
        impl.Setup(bulletPrefab, seedTravelTime, coreDuration, pulses, pulseInterval, bulletsPerPulse, pulseSpeed, pulseLifetime, coreScale, pulseScale, spinPerPulse);
    }
}

