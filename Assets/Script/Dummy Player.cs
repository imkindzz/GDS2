using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimplePlayer2D : MonoBehaviour
{
    public float speed = 6f;
    private Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity = input * speed;
    }
}
