using UnityEngine;

[CreateAssetMenu(menuName = "BulletPatterns/StraightShot")]
public class StraightShot : BulletPattern
{
    public float bulletSpeed = 5f;
    public float bulletLifetime = 1f;
    public override void Emit(Transform emitterTransform, GameObject bulletPrefab, Vector3? targetPosition = null)
    {
        if (targetPosition == null)
        {
            Debug.LogWarning("Target position is null!");
            return;
        }

        Vector3 directionToPlayer = targetPosition.Value - emitterTransform.position;

        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        
        Quaternion bulletRotation = Quaternion.Euler(0, 0, angle - 90f);

        GameObject bulletObj = Instantiate(bulletPrefab, emitterTransform.position, bulletRotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.speed = bulletSpeed;
            bullet.velocity = Vector2.up;
            bullet.rotation = bulletRotation.eulerAngles.z;
            bullet.lifetime = bulletLifetime;
        }
    }
}
