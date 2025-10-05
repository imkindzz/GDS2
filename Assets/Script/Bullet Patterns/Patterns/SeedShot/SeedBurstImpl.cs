using UnityEngine;

public class SeedBurstImpl : MonoBehaviour
{
    // Provided by Setup
    GameObject miniPrefab;
    float baseAngle;
    float explodeTime;
    int miniCount;
    float miniSpeed;
    float miniLifetime;
    float miniScale;

    // Flash config (tweak as needed)
    [SerializeField] float flashLeadTime = 0.4f;   // start flashing this many seconds before explode
    [SerializeField] float flashInterval = 0.08f;  // how quickly to toggle color

    //sounds
    [SerializeField] private SfxSoundName explodeSfx = SfxSoundName.VillagerDynamiteLessDramatic; //the sound of when the explosion happens

    // Internals
    float t;
    bool flashing;
    float nextFlashAt;

    SpriteRenderer sr;
    Color originalColor = Color.white;

    void Awake()
    {
        // Cache SpriteRenderer if present
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }

    public void Setup(GameObject miniPrefab, float baseAngle, float explodeTime, int miniCount, float miniSpeed, float miniLifetime, float miniScale)
    {
        this.miniPrefab = miniPrefab;
        this.baseAngle = baseAngle;
        this.explodeTime = Mathf.Max(0.01f, explodeTime);
        this.miniCount = Mathf.Max(1, miniCount);
        this.miniSpeed = miniSpeed;
        this.miniLifetime = miniLifetime;
        this.miniScale = Mathf.Clamp(miniScale, 0.1f, 1f);

        // Ensure the flash window is valid
        flashLeadTime = Mathf.Clamp(flashLeadTime, 0f, this.explodeTime * 0.9f);
    }

    void Update()
    {
        t += Time.deltaTime;

        // Start flashing in the last flashLeadTime seconds
        if (!flashing && t >= explodeTime - flashLeadTime)
        {
            flashing = true;
            nextFlashAt = t; // flash immediately
        }

        // Toggle red/original while flashing
        if (flashing && t >= nextFlashAt)
        {
            if (sr != null)
            {
                sr.color = (sr.color == originalColor) ? Color.red : originalColor;
            }
            nextFlashAt += flashInterval;
        }

        // Explode
        if (t >= explodeTime)
        {
            Split();
        }
    }

    void Split()
    {
        float step = 360f / miniCount;
        Vector3 pos = transform.position;

        SoundManager.instance.PlaySound(explodeSfx);

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

        // Restore original color (in case object pools reuse this)
        if (sr != null) sr.color = originalColor;

        Destroy(gameObject);
    }
}

