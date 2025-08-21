using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover2D : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] Transform player;
    Rigidbody2D rb;

    [Header("Stats")]
    [SerializeField] float health = 60f;

    [Header("Patterns")]
    [SerializeField] MovementPattern2D movementPattern;

    [Header("Boundary")]
    [SerializeField] Vector2 boundsCenter = Vector2.zero;
    [SerializeField] Vector2 boundsSize = new Vector2(20f, 12f);
    [SerializeField] float edgePadding = 0.02f;
    [SerializeField] float reentryMargin = 0.25f;

    [Header("Bounce")]
    [SerializeField] float bounceCooldown = 0.06f;

    float t, dirSign = 1f, bounceTimer;
    int flipX = 1, flipY = 1;
    bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    void FixedUpdate()
    {
        if (isDead || !movementPattern) return;

        t += Time.fixedDeltaTime;
        if (bounceTimer > 0f) bounceTimer -= Time.fixedDeltaTime;

        Vector2 v = movementPattern.EvaluateVelocity(transform, player, t * dirSign);
        v = new Vector2(v.x * flipX, v.y * flipY);

        Vector2 pos = rb.position;
        Vector2 nextPos = pos + v * Time.fixedDeltaTime;

        Vector2 min, max; GetBounds(out min, out max);
        Vector2 clamped = new Vector2(Mathf.Clamp(nextPos.x, min.x, max.x), Mathf.Clamp(nextPos.y, min.y, max.y));

        bool hitX = !Mathf.Approximately(clamped.x, nextPos.x);
        bool hitY = !Mathf.Approximately(clamped.y, nextPos.y);

        if (hitX || hitY)
        {
            Vector2 inward = Vector2.zero;
            if (hitX) inward.x = (nextPos.x < min.x) ? 1f : -1f;
            if (hitY) inward.y = (nextPos.y < min.y) ? 1f : -1f;

            Vector2 inside = new Vector2(
                Mathf.Clamp(clamped.x + inward.x * edgePadding, min.x, max.x),
                Mathf.Clamp(clamped.y + inward.y * edgePadding, min.y, max.y)
            );

            if (bounceTimer <= 0f)
            {
                if (hitX) flipX = -flipX;
                if (hitY) flipY = -flipY;
                dirSign = -dirSign;
                bounceTimer = bounceCooldown;
            }

            rb.MovePosition(inside);
        }
        else
        {
            rb.MovePosition(nextPos);
        }

        Vector2 p = rb.position;
        if (p.x > min.x + reentryMargin && p.x < max.x - reentryMargin &&
            p.y > min.y + reentryMargin && p.y < max.y - reentryMargin)
        {
            bounceTimer = Mathf.Min(bounceTimer, 0.02f);
        }

        if (v.sqrMagnitude > 0.0001f)
        {
            float z = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(z);
        }
    }

    void GetBounds(out Vector2 min, out Vector2 max)
    {
        Vector2 half = boundsSize * 0.5f;
        min = boundsCenter - half + Vector2.one * edgePadding;
        max = boundsCenter + half - Vector2.one * edgePadding;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        health -= amount;
        if (health <= 0f) Die();
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        var col = GetComponent<Collider2D>(); if (col) col.enabled = false;
        Destroy(gameObject, 0.25f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(new Vector3(boundsCenter.x, boundsCenter.y, 0f), new Vector3(boundsSize.x, boundsSize.y, 0.1f));
    }
}


