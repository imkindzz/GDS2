using UnityEngine;

public class ParticleBulletEmitter2D : MonoBehaviour
{
    [Header("Particle Bullets")]
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private int damage = 5;
    [SerializeField] private LayerMask hitMask; // typically Player

    // Fire one particle as a bullet in a direction
    public void Fire(Vector2 origin, Vector2 dir, float speed, float lifetime = 2f)
    {
        if (!ps) return;
        dir = dir.normalized;

        var emit = new ParticleSystem.EmitParams
        {
            position = origin,
            velocity = dir * speed,
            startLifetime = lifetime
        };
        ps.Emit(emit, 1);
    }

    // Utility: ring of bullets
    public void FireCircle(Vector2 origin, int count, float speed, float lifetime = 2f, float startAngleDeg = 0f)
    {
        float step = 360f / Mathf.Max(1, count);
        for (int i = 0; i < count; i++)
        {
            float a = startAngleDeg + i * step;
            Vector2 dir = new Vector2(Mathf.Cos(a * Mathf.Deg2Rad), Mathf.Sin(a * Mathf.Deg2Rad));
            Fire(origin, dir, speed, lifetime);
        }
    }

    // Unity calls this if Collision module has "Send Collision Messages" enabled
    private void OnParticleCollision(GameObject other)
    {
        if (((1 << other.layer) & hitMask.value) == 0) return;

        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
        if (playerStatus) playerStatus.TakeDamage(damage);
    }
}

