using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Vector2 velocity;
    public float rotation;

    public float lifetime = 0.1f;  // lifetime in seconds
    private float lifeTimer;

    public void Start()
    {
        transform.rotation = Quaternion.Euler(0,0,rotation);
        lifeTimer = lifetime;
    }
        

    void Update()
    {
        transform.Translate(velocity * speed * Time.deltaTime);

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
