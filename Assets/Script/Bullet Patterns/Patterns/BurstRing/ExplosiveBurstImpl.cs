using UnityEngine;

public class SingularityPulseImpl : MonoBehaviour
{
    GameObject prefab;
    float travelTime, coreDuration, pulseInterval, pulseSpeed, pulseLifetime, coreScale, pulseScale, spinPerPulse;
    int pulses, bulletsPerPulse;

    float t, nextPulseTime, spin;
    bool coreMode;
    Bullet seedBullet;

    public void Setup(GameObject prefab, float travelTime, float coreDuration, int pulses, float pulseInterval, int bulletsPerPulse, float pulseSpeed, float pulseLifetime, float coreScale, float pulseScale, float spinPerPulse)
    {
        this.prefab = prefab;
        this.travelTime = travelTime;
        this.coreDuration = coreDuration;
        this.pulses = Mathf.Max(1, pulses);
        this.pulseInterval = Mathf.Max(0.01f, pulseInterval);
        this.bulletsPerPulse = Mathf.Max(3, bulletsPerPulse);
        this.pulseSpeed = pulseSpeed;
        this.pulseLifetime = pulseLifetime;
        this.coreScale = Mathf.Max(0.1f, coreScale);
        this.pulseScale = Mathf.Max(0.1f, pulseScale);
        this.spinPerPulse = spinPerPulse;
        seedBullet = GetComponent<Bullet>();
        nextPulseTime = travelTime;
    }

    void Update()
    {
        t += Time.deltaTime;

        if (!coreMode && t >= travelTime)
        {
            coreMode = true;
            if (seedBullet)
            {
                seedBullet.speed = 0f;
                seedBullet.velocity = Vector2.zero;
            }
            if (coreScale != 1f) transform.localScale *= coreScale;
            nextPulseTime = t;
        }

        if (coreMode && pulses > 0 && t >= nextPulseTime && t <= travelTime + coreDuration + pulses * pulseInterval + 0.01f)
        {
            EmitRing();
            pulses--;
            spin += spinPerPulse;
            nextPulseTime += pulseInterval;
        }

        if (coreMode && t >= travelTime + coreDuration + (Mathf.Max(0, pulses) * pulseInterval))
        {
            Destroy(gameObject);
        }
    }

    void EmitRing()
    {
        Vector3 pos = transform.position;
        float step = 360f / bulletsPerPulse;
        for (int i = 0; i < bulletsPerPulse; i++)
        {
            float ang = spin + step * i;
            Quaternion rot = Quaternion.Euler(0f, 0f, ang - 90f);
            GameObject g = Instantiate(prefab, pos, rot);
            if (pulseScale != 1f) g.transform.localScale *= pulseScale;
            var b = g.GetComponent<Bullet>();
            if (b)
            {
                b.speed = pulseSpeed;
                b.velocity = Vector2.up;
                b.rotation = rot.eulerAngles.z;
                b.lifetime = pulseLifetime;
            }
        }
    }
}
