using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class SoulBarManager : MonoBehaviour
{
    public static SoulBarManager Instance;

    [Header("Soul Bar Settings")]
    [SerializeField] private Slider soulBar;
    [SerializeField] private float maxSoul = 100f;
    [SerializeField] private float gainAmount = 10f;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI fullSoulText; 
    [SerializeField] private Image maxSoulIcon;     

    private float currentSoul = 0f;
    private bool soulBarIsFull = false;

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

        if (fullSoulText != null)
            fullSoulText.gameObject.SetActive(false);

        if (maxSoulIcon != null)
            maxSoulIcon.enabled = false;
    }

    private void Update()
    {
        if (soulBarIsFull && Input.GetKeyDown(KeyCode.F))
        {
            GivePlayerHeart();
            ResetSoulBar();
        }
    }

    public void AddSouls(float amount)
    {
        currentSoul += amount;
        if (currentSoul >= maxSoul)
        {
            currentSoul = maxSoul;
            OnSoulBarFull();
        }
        UpdateSoulBarUI();
    }

    private void OnSoulBarFull()
    {
        soulBarIsFull = true;
        if (fullSoulText != null)
            fullSoulText.gameObject.SetActive(true);

        if (maxSoulIcon != null)
            maxSoulIcon.enabled = true;
    }

    private void GivePlayerHeart()
    {
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus player = PlayerStatus.Instance;
            var heartField = player.GetType().GetField("maxHearts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var currentField = player.GetType().GetField("currentHearts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            int maxHearts = (int)heartField.GetValue(player);
            int currentHearts = (int)currentField.GetValue(player);

            if (currentHearts < maxHearts)
            {
                currentHearts++;
                currentField.SetValue(player, currentHearts);

                var updateMethod = player.GetType().GetMethod("UpdateHeartsUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                updateMethod?.Invoke(player, null);

                SoundManager.instance.PlaySound(SfxSoundName.GainHP);

                Debug.Log("Gained 1 heart from soul bar!");
            }
            else
            {
                Debug.Log("Soul bar full, but hearts already at max.");
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

    public void ResetSoulBar()
    {
        currentSoul = 0f;
        soulBarIsFull = false;

        if (fullSoulText != null)
            fullSoulText.gameObject.SetActive(false);

        if (maxSoulIcon != null)
            maxSoulIcon.enabled = false;

        UpdateSoulBarUI();
    }
}



