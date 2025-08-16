using UnityEngine;

[CreateAssetMenu(menuName = "Shooter2D/Shooting/AimedBurst")]
public class Shoot_AimedBurst : ShootingPattern2D
{
    public float fireInterval = 0.6f;
    public int pellets = 3;
    public float spreadDeg = 15f;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 2.2f;

    private float timer;

    public override void Tick(MonoBehaviour owner, ParticleBulletEmitter2D emitter, Transform self, Transform player, float t)
    {
        timer -= Time.deltaTime;
        if (timer > 0f) return;
        timer = fireInterval;

        if (!player) return;

        Vector2 aim = ((Vector2)player.position - (Vector2)self.position).normalized;
        float half = (pellets - 1) * 0.5f;
        for (int i = 0; i < pellets; i++)
        {
            float offset = (i - half) * spreadDeg;
            float rad = offset * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(
                aim.x * Mathf.Cos(rad) - aim.y * Mathf.Sin(rad),
                aim.x * Mathf.Sin(rad) + aim.y * Mathf.Cos(rad)
            );
            emitter.Fire(self.position, dir, bulletSpeed, bulletLifetime);
        }
    }
}
