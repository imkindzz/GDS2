using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : StatusBase
{
    [SerializeField] private GameObject soulBody; //the player's soul body

    [Header("Heart Settings")]
    [SerializeField] private List<Image> heartImages; // drag your 5 heart images here
    private int currentHearts;
    private int maxHearts = 5;

    [Header("Streak System")]
    [SerializeField] private float streakIncreaseRate = 1f; // How many points per second
    [SerializeField] private int maxStreak = 100;
    [SerializeField] private Image streakFillBar; // The animated fill sprite
    [SerializeField] private GameObject maxStreakFlames; // The flame effect object

    private float currentStreak = 0f;
    private float streakTimer = 0f;
    private bool isTakingDamage = false;

    public static PlayerStatus Instance;

    void Start()
    {
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
        base.TakeDamage(amount);
        Debug.Log("Player is taking damage");
        isTakingDamage = true;

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
    #endregion

    #region State methods
    public override void OnDeathState()
    {
        //does something
        this.gameObject.SetActive(false);
        soulBody.SetActive(false);

        Debug.Log("Player is dead");
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
}
