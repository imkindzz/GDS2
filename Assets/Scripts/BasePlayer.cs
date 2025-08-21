using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    private Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw(horizontalAxis),
            Input.GetAxisRaw(verticalAxis)
        ).normalized;

        rb.velocity = input * speed;
    }
}
public class PlayerHealth2D : MonoBehaviour
{
    public int health = 100;
    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"Player took {amount} damage. Health: {health}");
    }
}