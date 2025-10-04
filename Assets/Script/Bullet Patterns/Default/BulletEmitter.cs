using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public List<BulletPattern> bulletPatterns = new List<BulletPattern>();

    public float fireRate = 1f; 
    private float fireCooldown = 0f;

    [Header("Sound")]
    public AudioClip bulletSFX;                   
    private AudioSource audioSource;

    private int currentPatternIndex = 0;

    private Transform playerTransform;

    public bool aimAtPlayer;
    public Vector3 aimPos = new Vector3(0,0,0);
    private Animator animator;
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        animator = GetComponent<Animator>();
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

            PlayBulletSound();

            // trigger animation
            if (animator != null)
                animator.SetTrigger("Shoot");

            currentPatternIndex = (currentPatternIndex + 1) % bulletPatterns.Count;
            fireCooldown = 1f / fireRate;
        }
    }

    public void Emit()
    {
        bulletPatterns[currentPatternIndex].Emit(transform, bulletPrefab);
        currentPatternIndex = (currentPatternIndex + 1) % bulletPatterns.Count;
    }

    private void PlayBulletSound()
    {
        if (bulletSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(bulletSFX);
        }
    }
}
