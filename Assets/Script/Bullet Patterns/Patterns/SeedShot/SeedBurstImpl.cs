using UnityEngine;

public class SeedBurstImpl : MonoBehaviour
{
    GameObject miniPrefab;
    float baseAngle;
    float explodeTime;
    int miniCount;
    float miniSpeed;
    float miniLifetime;
    float miniScale;

    float t;

    public void Setup(GameObject miniPrefab, float baseAngle, float explodeTime, int miniCount, float miniSpeed, float miniLifetime, float miniScale)
    {
        this.miniPrefab = miniPrefab;
        this.baseAngle = baseAngle;
        this.explodeTime = explodeTime;
        this.miniCount = Mathf.Max(1, miniCount);
        this.miniSpeed = miniSpeed;
        this.miniLifetime = miniLifetime;
        this.miniScale = Mathf.Clamp(miniScale, 0.1f, 1f);
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t >= explodeTime)
        {
            float step = 360f / miniCount;
            Vector3 pos = transform.position;

            for (int i = 0; i < miniCount; i++)
            {
                float ang = baseAngle + step * i;
                Quaternion rot = Quaternion.Euler(0f, 0f, ang - 90f);
                GameObject mini = Instantiate(miniPrefab, pos, rot);

                if (miniScale != 1f) mini.transform.localScale *= miniScale;

                Bullet b = mini.GetComponent<Bullet>();
                if (b != null)
                {
                    b.speed = miniSpeed;
                    b.velocity = Vector2.up;
                    b.rotation = rot.eulerAngles.z;
                    b.lifetime = miniLifetime;
                }
            }

            Destroy(gameObject);
        }
    }
}
