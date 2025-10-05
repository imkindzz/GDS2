using UnityEngine;

public class SingularityPulseImpl : MonoBehaviour
{
    GameObject prefab;
    float travelTime, coreDuration, pulseInterval, pulseSpeed, pulseLifetime, coreScale, pulseScale, spinPerPulse;
    int pulses, bulletsPerPulse;

    float t, nextPulseTime, spin;
    bool coreMode;
    Bullet seedBullet;

    // ---- Flash config ----
    [SerializeField] float flashLeadTime = 0.35f;  // seconds before a pulse to start flashing
    [SerializeField] float flashInterval = 0.08f;  // toggle rate

    bool flashing;
    float nextFlashToggleTime;

    // ---- Color handling (SpriteRenderer or Material) ----
    SpriteRenderer sr;
    Color originalSpriteColor = Color.white;

    Renderer rend;
    MaterialPropertyBlock mpb;
    int colorId = Shader.PropertyToID("_Color");
    int baseColorId = Shader.PropertyToID("_BaseColor"); // URP/HDRP Lit uses _BaseColor
    bool useBaseColor;     // true if material has _BaseColor, else _Color
    Color originalMatColor = Color.white;

    //sound
    [SerializeField] private SfxSoundName explodeSfx = SfxSoundName.GoblinClubThrowImpact;

    void Awake()
    {
        // Prefer SpriteRenderer if present
        sr = GetComponent<SpriteRenderer>();
        if (sr) originalSpriteColor = sr.color;

        // Fallback to generic Renderer with MPB (no material instantiation)
        if (!sr)
        {
            rend = GetComponent<Renderer>();
            if (rend)
            {
                mpb = new MaterialPropertyBlock();
                // Decide which color property to use
                var mat = rend.sharedMaterial;
                useBaseColor = mat && mat.HasProperty(baseColorId);
                int id = useBaseColor ? baseColorId : colorId;

                rend.GetPropertyBlock(mpb);
                originalMatColor = mpb.HasFloat(id) ? mpb.GetColor(id) :
                                   (mat && mat.HasProperty(id) ? mat.GetColor(id) : Color.white);
                SetRendererColor(originalMatColor); // ensure MPB initialized
            }
        }
    }

    public void Setup(GameObject prefab, float travelTime, float coreDuration, int pulses, float pulseInterval, int bulletsPerPulse, float pulseSpeed, float pulseLifetime, float coreScale, float pulseScale, float spinPerPulse)
    {
        this.prefab = prefab;
        this.travelTime = Mathf.Max(0.01f, travelTime);
        this.coreDuration = Mathf.Max(0f, coreDuration);
        this.pulses = Mathf.Max(1, pulses);
        this.pulseInterval = Mathf.Max(0.01f, pulseInterval);
        this.bulletsPerPulse = Mathf.Max(3, bulletsPerPulse);
        this.pulseSpeed = pulseSpeed;
        this.pulseLifetime = Mathf.Max(0.01f, pulseLifetime);
        this.coreScale = Mathf.Max(0.1f, coreScale);
        this.pulseScale = Mathf.Max(0.1f, pulseScale);
        this.spinPerPulse = spinPerPulse;

        seedBullet = GetComponent<Bullet>();
        nextPulseTime = this.travelTime;

        // Ensure flash window is sensible relative to the interval
        flashLeadTime = Mathf.Clamp(flashLeadTime, 0f, this.pulseInterval * 0.9f);
    }

    void Update()
    {
        t += Time.deltaTime;

        // Enter core mode (stop & scale)
        if (!coreMode && t >= travelTime)
        {
            coreMode = true;
            if (seedBullet)
            {
                seedBullet.speed = 0f;
                seedBullet.velocity = Vector2.zero;
            }
            if (coreScale != 1f) transform.localScale *= coreScale;
            nextPulseTime = t; // first pulse immediately on entering core
        }

        if (coreMode && pulses > 0)
        {
            // Start flashing shortly before each pulse
            if (!flashing && t >= nextPulseTime - flashLeadTime)
            {
                flashing = true;
                nextFlashToggleTime = t; // toggle immediately
            }

            // Toggle warning color while in the flash window
            if (flashing && t >= nextFlashToggleTime)
            {
                ToggleWarnColor();
                nextFlashToggleTime += flashInterval;
            }

            // Fire pulse when time arrives
            if (t >= nextPulseTime)
            {
                // Ensure we’re back to original color for the pulse
                SetOriginalColor();
                flashing = false;

                EmitRing();
                pulses--;
                spin += spinPerPulse;
                nextPulseTime += pulseInterval;
            }
        }

        // Lifetime end for the core object; Bullet.lifetime may also handle this,
        // but this explicit destroy keeps things tidy.
        if (coreMode && t >= travelTime + coreDuration + Mathf.Max(0, pulses) * pulseInterval)
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
            SoundManager.instance.PlaySound(explodeSfx);

            float ang = spin + step * i;
            Quaternion rot = Quaternion.Euler(0f, 0f, ang - 90f);
            GameObject g = Instantiate(prefab, pos, rot);

            if (pulseScale != 1f) g.transform.localScale *= pulseScale;

            var b = g.GetComponent<Bullet>();
            if (b)
            {
                b.speed = pulseSpeed;
                b.velocity = Vector2.up; // local up
                b.rotation = rot.eulerAngles.z;
                b.lifetime = pulseLifetime;
            }
        }
    }

    // ---- Color helpers ----
    void ToggleWarnColor()
    {
        if (sr)
        {
            sr.color = (sr.color == originalSpriteColor) ? Color.red : originalSpriteColor;
            return;
        }
        if (rend)
        {
            int id = useBaseColor ? baseColorId : colorId;
            // Read current
            rend.GetPropertyBlock(mpb);
            Color cur = mpb.HasFloat(id) ? mpb.GetColor(id) : originalMatColor;
            SetRendererColor(cur.Equals(originalMatColor) ? Color.red : originalMatColor);
        }
    }

    void SetOriginalColor()
    {
        if (sr)
        {
            sr.color = originalSpriteColor;
        }
        else if (rend)
        {
            SetRendererColor(originalMatColor);
        }
    }

    void SetRendererColor(Color c)
    {
        if (!rend) return;
        int id = useBaseColor ? baseColorId : colorId;
        rend.GetPropertyBlock(mpb);
        mpb.SetColor(id, c);
        rend.SetPropertyBlock(mpb);
    }

    void OnDestroy()
    {
        // Restore in case of pooling / shared visuals
        SetOriginalColor();
    }
}

