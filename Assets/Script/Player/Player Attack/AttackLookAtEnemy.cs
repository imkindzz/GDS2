using UnityEngine;

public class AttackLookAtEnemy : MonoBehaviour
{
    [HideInInspector] public Transform soul;       // center of the soul sprite
    [HideInInspector] public Transform enemy;      // center of the enemy sprite

    private void Update()
    {
        if (soul == null || enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        // Direction and distance between soul and enemy
        Vector3 direction = enemy.position - soul.position;
        float distance = direction.magnitude;

        // Position the beam at the midpoint
        transform.position = soul.position + direction / 2f;

        // Rotate the beam to face the enemy
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Scale the beam along Y (or X depending on your sprite orientation)
        // Assuming the sprite’s original length = 1 unit along Y
        transform.localScale = new Vector3(transform.localScale.x, distance, transform.localScale.z);
    }
}








