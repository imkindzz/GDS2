using UnityEngine;

[CreateAssetMenu(menuName = "BulletPatterns/BigFastSingleShot")]
public class BigFastSingleShot : BulletPattern
{
    public float bulletSpeed = 18f;
    public float bulletLifetime = 4f;
    public float scaleMultiplier = 2f;
    public bool faceTravelDirection = true;

    public override void Emit(Transform emitterTransform, GameObject bulletPrefab, Vector3? targetPosition = null)
    {
        if (!bulletPrefab || !emitterTransform) return;

        // Direction to target (fallback to local "up" if no target)
        Vector3 dir3 = targetPosition.HasValue
            ? (targetPosition.Value - emitterTransform.position)
            : emitterTransform.up;

        if (dir3.sqrMagnitude < 0.0001f) dir3 = emitterTransform.up; // edge case
        dir3.Normalize();

        // Compute a facing rotation (your SpreadShot used "-90f" to align "up" with travel)
        float angleDeg = Mathf.Atan2(dir3.y, dir3.x) * Mathf.Rad2Deg;
        Quaternion rot = faceTravelDirection ? Quaternion.Euler(0f, 0f, angleDeg - 90f) : emitterTransform.rotation;

        // Spawn
        GameObject bulletObj = Instantiate(bulletPrefab, emitterTransform.position, rot);

        // Make it bigger
        bulletObj.transform.localScale *= scaleMultiplier;

        // Set Bullet component fields if present
        var bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.speed = bulletSpeed;
            bullet.velocity = (Vector2)dir3 * bulletSpeed;   // world-space velocity
            bullet.rotation = rot.eulerAngles.z;
            bullet.lifetime = bulletLifetime;
        }
        else
        {
            // Fallback: drive with Rigidbody2D if your prefab uses physics
            var rb = bulletObj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
                rb.velocity = (Vector2)dir3 * bulletSpeed;
            }
        }

        //Sounds

        // Safety cleanup
        Destroy(bulletObj, bulletLifetime);
    }
}
