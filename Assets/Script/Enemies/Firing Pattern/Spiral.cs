using UnityEngine;

[CreateAssetMenu(menuName = "Shooter2D/Shooting/Spiral")]
public class Shoot_Spiral : ShootingPattern2D
{
    public float rpm = 60f;            // rotations per minute
    public float bulletsPerSecond = 10f;
    public float bulletSpeed = 8f;
    public float bulletLifetime = 2.5f;
    public float startAngleDeg = 0f;

    private float fireAccum;
    private float angle;

    public override void Tick(MonoBehaviour owner, ParticleBulletEmitter2D emitter, Transform self, Transform player, float t)
    {
        // advance angle
        angle = startAngleDeg + (t * rpm * 360f / 60f);

        // emit at a fixed rate
        fireAccum += Time.deltaTime * bulletsPerSecond;
        while (fireAccum >= 1f)
        {
            fireAccum -= 1f;
            float a = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            emitter.Fire(self.position, dir, bulletSpeed, bulletLifetime);
            angle += 360f / bulletsPerSecond * (rpm / 60f); // smooth spin
        }
    }
}

