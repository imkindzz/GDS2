using UnityEngine;

public class SwordSwingArcImpl : MonoBehaviour
{
    GameObject prefab;
    Transform origin;
    float startAngle, endAngle, duration, radius, bulletSpeed, bulletLifetime;
    int bulletsTotal;

    float t;
    int spawned;

    public void Setup(GameObject prefab, Transform origin, float startAngle, float endAngle, float duration, int bulletsTotal, float radius, float bulletSpeed, float bulletLifetime)
    {
        this.prefab = prefab;
        this.origin = origin;
        this.startAngle = startAngle;
        this.endAngle = endAngle;
        this.duration = Mathf.Max(0.01f, duration);
        this.bulletsTotal = Mathf.Max(1, bulletsTotal);
        this.radius = Mathf.Max(0f, radius);
        this.bulletSpeed = Mathf.Max(0f, bulletSpeed);
        this.bulletLifetime = Mathf.Max(0.02f, bulletLifetime);
    }

    void Update()
    {
        if (!prefab || !origin) { Destroy(gameObject); return; }

        t += Time.deltaTime;
        float u = Mathf.Clamp01(t / duration);
        int shouldHave = Mathf.CeilToInt(u * bulletsTotal);

        while (spawned < shouldHave)
        {
            float k = (bulletsTotal == 1) ? 0.5f : (float)spawned / (bulletsTotal - 1);
            float ang = Mathf.Lerp(startAngle, endAngle, k);
            SpawnAtAngle(ang);
            spawned++;
        }

        if (t >= duration) Destroy(gameObject);
    }

    void SpawnAtAngle(float angleDeg)
    {
        Vector2 dir = new Vector2(Mathf.Cos(angleDeg * Mathf.Deg2Rad), Mathf.Sin(angleDeg * Mathf.Deg2Rad));
        Vector3 pos = origin.position + (Vector3)(dir * radius);
        Quaternion rot = Quaternion.Euler(0f, 0f, angleDeg - 90f);

        GameObject g = Instantiate(prefab, pos, rot);
        var b = g.GetComponent<Bullet>();
        if (b)
        {
            b.speed = bulletSpeed;
            b.velocity = Vector2.up;        // your Bullet uses local +Y as forward
            b.rotation = rot.eulerAngles.z;
            b.lifetime = bulletLifetime;
        }
    }
}
