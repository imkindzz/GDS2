using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyShooter2D : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform player;
    [SerializeField] private ParticleBulletEmitter2D bulletEmitter; // particle-based bullets
    private Rigidbody2D rb;

    [Header("Patterns")]
    [SerializeField] private MovementPattern2D movementPattern;
    [SerializeField] private ShootingPattern2D shootingPattern;

    private float t; // pattern time

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // For top-down shooters you usually want no gravity and no physics rotation
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    // Do physics in FixedUpdate
    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        t += dt;

        // movement
        if (movementPattern)
        {
            Vector2 v = movementPattern.EvaluateVelocity(transform, player, t);

            if (rb.bodyType == RigidbodyType2D.Kinematic)
            {
                // Kinematic bodies don't advance from velocity automatically
                rb.MovePosition(rb.position + v * dt);
            }
            else
            {
                rb.velocity = v; // Dynamic body
            }

            // rotate to face velocity if moving
            if (v.sqrMagnitude > 0.001f)
            {
                float z = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90f;
                rb.MoveRotation(z); // use physics-friendly rotation
            }
        }

        // shooting (ok to tick here; it's tied to pattern time)
        if (shootingPattern && bulletEmitter)
        {
            shootingPattern.Tick(this, bulletEmitter, transform, player, t);
        }
    }
}

