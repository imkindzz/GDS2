using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyShooter2D : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform player;
    [SerializeField] private ParticleBulletEmitter2D bulletEmitter; // particle-based bullets
    private Rigidbody2D rb;

    [Header("Stats")]
    [SerializeField] private float health = 60f;

    [Header("Patterns")]
    [SerializeField] private MovementPattern2D movementPattern;
    [SerializeField] private ShootingPattern2D shootingPattern;

    private float t; // pattern time
    private bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    void Update()
    {
        if (isDead) return;
        t += Time.deltaTime;

        // movement
        if (movementPattern)
        {
            Vector2 v = movementPattern.EvaluateVelocity(transform, player, t);
            rb.velocity = v;
            // rotate to face velocity if moving
            if (v.sqrMagnitude > 0.001f)
            {
                float z = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0, 0, z);
            }
        }

        // shooting
        if (shootingPattern && bulletEmitter)
        {
            shootingPattern.Tick(this, bulletEmitter, transform, player, t);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        health -= amount;
        if (health <= 0f) Die();
    }

    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        var col = GetComponent<Collider2D>(); if (col) col.enabled = false;
        Destroy(gameObject, 0.25f);
    }
}
