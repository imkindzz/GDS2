using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // no grav
    }

    void Update()
    {
        // wasd
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // diagonal speed
        moveInput = new Vector2(moveX, moveY).normalized;

        // apply movement
        rb.velocity = moveInput * moveSpeed;
    }
}
