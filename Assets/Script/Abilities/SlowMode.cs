using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class SlowMode : MonoBehaviour
{
    [Header("Slow Mode Settings")]
    public float slowMultiplier = 0.4f;
    public float smoothTime = 0.1f;

    private PlayerMovement playerMovement;
    private float originalSpeed;
    private float targetSpeed;
    private float currentVelocity = 0f;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        originalSpeed = playerMovement.speed;
    }

    void Update()
    { // spacebar to slow
        if (Input.GetKey(KeyCode.Space))
        {
            targetSpeed = originalSpeed * slowMultiplier;
        }
        else
        {
            targetSpeed = originalSpeed;
        }

        playerMovement.speed = Mathf.SmoothDamp(
            playerMovement.speed,
            targetSpeed,
            ref currentVelocity,
            smoothTime
        );
    }
}