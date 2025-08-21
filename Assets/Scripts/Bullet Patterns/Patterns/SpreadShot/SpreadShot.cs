using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletPatterns/SpreadShot")]
public class SpreadShot : BulletPattern
{
    public float bulletSpeed = 5f;
    public float bulletLifetime = 1f;
    public int numBullets = 2;
    public float spread = 20f; //degrees
    public override void Emit(Transform emitterTransform, GameObject bulletPrefab, Vector3? targetPosition = null)
    {
        if (targetPosition == null)
        {
            Debug.LogWarning("Target position is null!");
            return;
        }

        Vector3 directionToPlayer = targetPosition.Value - emitterTransform.position;

        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        float halfSpread = spread / 2f;

        for (int i = 0; i < numBullets; i++)
        {
            
            float angleOffset = 0f;

            if (numBullets > 1)
            {
                float angleStep = spread / (numBullets - 1);
                angleOffset = -halfSpread + angleStep * i;
            }

            float finalAngle = baseAngle + angleOffset;

            Quaternion bulletRotation = Quaternion.Euler(0, 0, finalAngle - 90f);
            GameObject bulletObj = Instantiate(bulletPrefab, emitterTransform.position, bulletRotation);

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.speed = bulletSpeed;
                bullet.velocity = Quaternion.Euler(0, 0, angleOffset) * Vector2.up;
                bullet.rotation = bulletRotation.eulerAngles.z;
                bullet.lifetime = bulletLifetime;
            }
        }
    }
}
