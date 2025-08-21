using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public List<BulletPattern> bulletPatterns = new List<BulletPattern>();

    public float fireRate = 1f; 
    private float fireCooldown = 0f;

    private int currentPatternIndex = 0;

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }
    void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f && bulletPatterns.Count > 0)
        {
            Vector3? playerPos = playerTransform != null ? playerTransform.position : (Vector3?)null;

            bulletPatterns[currentPatternIndex].Emit(transform, bulletPrefab, playerPos);

            currentPatternIndex = (currentPatternIndex + 1) % bulletPatterns.Count;
            fireCooldown = 1f / fireRate;
        }
    }

    public void Emit()
    {
        bulletPatterns[currentPatternIndex].Emit(transform, bulletPrefab);
        currentPatternIndex = (currentPatternIndex + 1) % bulletPatterns.Count;
    }
}
