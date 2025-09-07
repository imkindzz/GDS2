using UnityEngine;

[CreateAssetMenu(menuName = "BulletPatterns/SeedBurstShot")]
public class SeedBurstShot : BulletPattern
{
    public float seedSpeed = 6f;
    public float seedLifetime = 1.6f;
    public float miniSpeed = 7f;
    public float miniLifetime = 0.9f;
    public int miniCount = 4;
    public float miniScale = 0.7f;

    public override void Emit(Transform emitterTransform, GameObject bulletPrefab, Vector3? targetPosition = null)
    {
        if (bulletPrefab == null || targetPosition == null) return;

        Vector3 dirToTarget = targetPosition.Value - emitterTransform.position;
        float baseAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, baseAngle - 90f);

        GameObject seed = Object.Instantiate(bulletPrefab, emitterTransform.position, rot);

        Bullet b = seed.GetComponent<Bullet>();
        if (b != null)
        {
            b.speed = seedSpeed;
            b.velocity = Vector2.up;
            b.rotation = rot.eulerAngles.z;
            b.lifetime = seedLifetime;
        }

        var impl = seed.AddComponent<SeedBurstImpl>();
        impl.Setup(bulletPrefab, baseAngle, seedLifetime * 0.5f, miniCount, miniSpeed, miniLifetime, miniScale);
    }
}
