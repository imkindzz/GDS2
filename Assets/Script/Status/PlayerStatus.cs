using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : StatusBase
{
    [SerializeField] private GameObject soulBody; //the player's soul body

    [Header("Invinciblity State")]
    [SerializeField] private float invincibilityDuration = 1.5f;
    [SerializeField] private float flashDuration = 1.5f;
    [SerializeField] private float flashInterval = 0.1f;

    [Header("Heart Settings")]
    [SerializeField] private List<Image> heartImages; 
    private int currentHearts;
    private int maxHearts = 5;

    [Header("Streak System")]
    [SerializeField] private float streakIncreaseRate = 1f; 
    [SerializeField] private int maxStreak = 100;
    [SerializeField] private Image streakFillBar; 
    [SerializeField] private GameObject maxStreakFlames;

    [Header("Damage Flash")]
    [SerializeField] private SpriteRenderer damageFlashSprite; 
    [SerializeField] private float flashFadeDuration = 0.5f;   



    private SpriteRenderer spriteRenderer;

    private float currentStreak = 0f;
    private float streakTimer = 0f;
    private bool isTakingDamage = false;

    public static PlayerStatus Instance;

    //invincibility state
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    private Coroutine flashRoutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHearts = maxHearts;
        UpdateHeartsUI();
    }

    void Update()
    {
        HandleStreak();
    }

    #region Health methods
    public override void TakeHealth(float amount)
    {
        base.TakeHealth(amount);
    }

    public override void TakeDamage(float amount)
    {
        if (!isInvincible)
        {
            base.TakeDamage(amount);
            Debug.Log("Player is taking damage");

            SoundManager.instance.PlaySound(SfxSoundName.PlayerHit, transform);

            StartInvincibility(invincibilityDuration);
            

            isTakingDamage = true;

            if (damageFlashRoutine != null) StopCoroutine(damageFlashRoutine);
            damageFlashRoutine = StartCoroutine(DamageFlash());

            if (noHealth && currentHearts > 0)
            {
                currentHearts--;           // lose a heart
                currentHealth = maxHealth; // reset health for next heart
                UpdateHeartsUI();

                if (currentHearts <= 0)
                {
                    OnDeathState(); // player dies if no hearts left
                }
            }
        }
    }
    #endregion

    #region State methods
    public override void OnDeathState()
    {
        SoundManager.instance.PlayMusic(MusicName.Death);
        SoundManager.instance.PlaySound(SfxSoundName.DeathSound);

        EndInvincibility(); //ensures that the invisbility is gone before anything else

        this.gameObject.SetActive(false);
        soulBody.SetActive(false);

        Debug.Log("Player is dead");
    }
    #endregion

    #region Invincibility methods
    //starts the invincibility
    private void StartInvincibility(float duration)
    {
        isInvincible = true;
        invincibilityTimer = duration;

        // Start flash effect
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashEffect(duration));
    }

    //ends the invincibility
    private void EndInvincibility()
    {
        isInvincible = false;

        // Ensure sprite is fully visible
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

        // Stop flash effect if still running
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }
    }

    // Flash effect coroutine
    private IEnumerator FlashEffect(float duration)
    {
        if (spriteRenderer == null) yield break;

        float timer = 0;

        // Flash between visible and partially transparent
        while (timer < duration)
        {
            // Toggle visibility
            spriteRenderer.color = spriteRenderer.color.a >= 1.0f ?
                new Color(1f, 1f, 1f, 0.3f) : Color.white;

            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval;
        }

        // Ensure sprite is visible at the end
        spriteRenderer.color = Color.white;
        flashRoutine = null;

        EndInvincibility(); //ensures that the invincibility have ended
    }
    #endregion

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].gameObject.SetActive(i < currentHearts);
        }
    }

    private void HandleStreak()
    {
        if (isTakingDamage)
        {
            currentStreak = 0f;
            UpdateStreakUI();
            isTakingDamage = false;
            return;
        }

        streakTimer += Time.deltaTime;
        if (streakTimer >= 1f)
        {
            streakTimer = 0f;
            currentStreak += streakIncreaseRate;
            currentStreak = Mathf.Clamp(currentStreak, 0f, maxStreak);
            UpdateStreakUI();
        }
    }

    private void UpdateStreakUI()
    {
        if (streakFillBar != null)
        {
            float fillAmount = currentStreak / maxStreak;
            streakFillBar.fillAmount = fillAmount;
        }

        if (maxStreakFlames != null)
        {
            maxStreakFlames.SetActive(currentStreak >= maxStreak);
        }
    }
    public float GetScoreMultiplier()
    {
        float t = currentStreak / maxStreak;
        return Mathf.Lerp(1f, 5f, t);      
    }

    private void Awake()
    {
        Instance = this;
    }

    private Coroutine damageFlashRoutine;


    private IEnumerator DamageFlash()
    {
        if (damageFlashSprite == null) yield break;

        // start fully visible
        Color c = damageFlashSprite.color;
        c.a = 1f;
        damageFlashSprite.color = c;
        damageFlashSprite.gameObject.SetActive(true);

        float t = 0f;
        while (t < flashFadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / flashFadeDuration);
            c.a = alpha;
            damageFlashSprite.color = c;
            yield return null;
        }

        // ensure fully transparent and optionally disable
        c.a = 0f;
        damageFlashSprite.color = c;
        damageFlashSprite.gameObject.SetActive(false);
        damageFlashRoutine = null;
    }



}
