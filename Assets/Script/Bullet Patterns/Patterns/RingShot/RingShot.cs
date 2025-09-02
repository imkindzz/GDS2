using UnityEngine;

[CreateAssetMenu(menuName = "BulletPatterns/RingShot")]
public class RingShot : BulletPattern
{
    public float bulletSpeed = 5f;
    public float bulletLifetime = 1f;

    [Min(1)] public int numBullets = 12;   // bullets per ring
    public float startAngle = 0f;          // initial rotation offset (deg)
    public float spinStep = 10f;           // how much the ring rotates after each Emit (deg)

    public override void Emit(Transform emitterTransform, GameObject bulletPrefab, Vector3? targetPosition = null)
    {
        if (numBullets <= 0 || bulletPrefab == null) return;

        float step = 360f / numBullets;

        for (int i = 0; i < numBullets; i++)
        {
            float angle = startAngle + step * i;

            // Unity's "up" is 90° ahead of "right", so rotate sprite by (angle - 90)
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, angle - 90f);
            GameObject bulletObj = Instantiate(bulletPrefab, emitterTransform.position, bulletRotation);

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.speed = bulletSpeed;
                bullet.velocity = Quaternion.Euler(0f, 0f, angle) * Vector2.up; // shoot outwards
                bullet.rotation = bulletRotation.eulerAngles.z;
                bullet.lifetime = bulletLifetime;
            }
        }

        // advance the ring so subsequent emits appear to spin
        startAngle += spinStep;
        if (startAngle >= 360f) startAngle -= 360f;
        else if (startAngle <= -360f) startAngle += 360f;
    }
}
