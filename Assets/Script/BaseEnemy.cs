using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy2D : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform player;
    [SerializeField] private ParticleSystem hitEffect;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;   // used by OverlapCircle
    [SerializeField] private LayerMask attackHitMask; // what the attack can hit (e.g., Player)

    [Header("Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private int damage = 10;

    [Header("AI")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float walkPointRange = 6f;
    [SerializeField] private float timeBetweenAttacks = 0.75f;
    [SerializeField] private float sightRange = 8f;
    [SerializeField] private float attackRange = 1.4f;
    [SerializeField] private float repathEvery = 0.25f; // how often to update chase direction

    private Rigidbody2D rb;
    private Vector2 spawnPos;
    private Vector2 walkPoint;
    private bool walkPointSet;
    private bool alreadyAttacked;
    private bool takeDamage;
    private bool isDead;
    private float repathTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        spawnPos = transform.position;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        bool playerInSight = Physics2D.OverlapCircle(transform.position, sightRange, playerLayer);
        bool playerInAttack = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttack) Patrol();
        else if (playerInSight && !playerInAttack) ChasePlayer();
        else if (playerInAttack && playerInSight) AttackPlayer();
        else if (!playerInSight && takeDamage) ChasePlayer();
    }

    private void FixedUpdate()
    {
        // velocity is applied in Patrol/Chase; keep z=0 facing
        var rot = transform.eulerAngles; rot.z = AngleToFace(rb.velocity);
        if (rb.velocity.sqrMagnitude > 0.001f) transform.eulerAngles = rot;
    }

    private void Patrol()
    {
        if (!walkPointSet) walkPoint = RandomPointAround(spawnPos, walkPointRange, out walkPointSet);

        Vector2 to = walkPoint - (Vector2)transform.position;
        if (to.sqrMagnitude < 0.5f * 0.5f) { walkPointSet = false; rb.velocity = Vector2.zero; return; }

        rb.velocity = to.normalized * moveSpeed * 0.4f; // slower wander
    }

    private void ChasePlayer()
    {
        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            rb.velocity = dir * moveSpeed;
            repathTimer = repathEvery;
        }
    }

    private void AttackPlayer()
    {
        rb.velocity = Vector2.zero; // stop to attack

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;

            // Simple “melee” overlap check in front
            Vector2 origin = (Vector2)transform.position;
            Vector2 dir = ((Vector2)player.position - origin).normalized;
            Vector2 hitPos = origin + dir * attackRange * 0.9f;
            var hit = Physics2D.OverlapCircle(hitPos, 0.4f, attackHitMask);

            if (hit)
            {
                // Example:
                // var hp = hit.GetComponent<PlayerHealth2D>();
                // if (hp) hp.TakeDamage(damage);
            }

            Invoke(nameof(ResetAttack), Mathf.Max(0.05f, timeBetweenAttacks));
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        if (hitEffect) hitEffect.Play();

        StopAllCoroutines();
        StartCoroutine(DamageAggro());

        if (health <= 0f) Die();
    }

    private System.Collections.IEnumerator DamageAggro()
    {
        takeDamage = true;
        yield return new WaitForSeconds(2f);
        takeDamage = false;
    }

    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        var col = GetComponent<Collider2D>(); if (col) col.enabled = false;
        Destroy(gameObject, 0.2f);
    }

    private Vector2 RandomPointAround(Vector2 center, float radius, out bool ok)
    {
        ok = true;
        var r = Random.insideUnitCircle * radius;
        return center + r;
    }

    private float AngleToFace(Vector2 v)
    {
        if (v.sqrMagnitude < 0.0001f) return transform.eulerAngles.z;
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90f; // 2D “up” forward
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
