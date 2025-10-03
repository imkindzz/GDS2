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

    public bool aimAtPlayer;
    public Vector3 aimPos = new Vector3(0,0,0);
    private Animator animator;
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

            if (aimAtPlayer) 
            {
                Vector3? playerPos = playerTransform != null ? playerTransform.position : (Vector3?)null;

                bulletPatterns[currentPatternIndex].Emit(transform, bulletPrefab, playerPos);
            }
            else
            {

                bulletPatterns[currentPatternIndex].Emit(transform, bulletPrefab, aimPos);
            }
            
            // trigger animation
            if (animator != null)
                animator.SetTrigger("Shoot");
            animator = GetComponent<Animator>();

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
