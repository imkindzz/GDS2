using UnityEngine;
using UnityEngine.UI;

public class SoulBarManager : MonoBehaviour
{
    public static SoulBarManager Instance;

    [Header("Soul Bar Settings")]
    [SerializeField] private Slider soulBar;       
    [SerializeField] private float maxSoul = 100f; 
    [SerializeField] private float gainAmount = 10f;

    private float currentSoul = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (soulBar != null)
        {
            soulBar.maxValue = maxSoul;
            soulBar.value = currentSoul;
        }
    }

    /// <summary>
    /// Call this when an enemy dies to increase the soul bar.
    /// </summary>
    public void AddSouls(float amount)
    {
        currentSoul += amount;
        if (currentSoul >= maxSoul)
        {
            currentSoul -= maxSoul;
            GivePlayerHeart();
        }
        UpdateSoulBarUI();
    }

    private void GivePlayerHeart()
    {
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus player = PlayerStatus.Instance;
            // Prevent going over max hearts
            var heartField = player.GetType().GetField("maxHearts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var currentField = player.GetType().GetField("currentHearts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            int maxHearts = (int)heartField.GetValue(player);
            int currentHearts = (int)currentField.GetValue(player);

            if (currentHearts < maxHearts)
            {
                currentHearts++;
                currentField.SetValue(player, currentHearts);

                // Call the private UpdateHeartsUI() method via reflection
                var updateMethod = player.GetType().GetMethod("UpdateHeartsUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                updateMethod?.Invoke(player, null);

                Debug.Log("Gained 1 heart from soul bar!");
            }
            else
            {
                Debug.Log("Soul bar filled, but hearts are already at max.");
            }
        }
    }

    private void UpdateSoulBarUI()
    {
        if (soulBar != null)
        {
            soulBar.value = currentSoul;
        }
    }

    /// <summary>
    /// Optional: reset soul bar manually.
    /// </summary>
    public void ResetSoulBar()
    {
        currentSoul = 0f;
        UpdateSoulBarUI();
    }
}


