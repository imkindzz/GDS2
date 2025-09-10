using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text multiplierText;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        UpdateScoreUI();
    }

    public void AddScore(int basePoints)
    {
        float multiplier = 1f;

        if (PlayerStatusInstanceExists()) // Avoid null errors
        {
            multiplier = PlayerStatus.Instance.GetScoreMultiplier();
        }

        int finalPoints = Mathf.RoundToInt(basePoints * multiplier);
        score += finalPoints;

        UpdateScoreUI();
    }


    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();

        if (multiplierText != null && PlayerStatus.Instance != null)
        {
            float mult = PlayerStatus.Instance.GetScoreMultiplier();
            multiplierText.text = "Multiplier: x" + mult.ToString("0.0");
        }
    }


    private bool PlayerStatusInstanceExists()
    {
        return PlayerStatus.Instance != null;
    }

    private void Update()
    {
        UpdateMultiplierUI();
    }

    private void UpdateMultiplierUI()
    {
        if (multiplierText != null && PlayerStatus.Instance != null)
        {
            float mult = PlayerStatus.Instance.GetScoreMultiplier();
            multiplierText.text = "x" + mult.ToString("0.0");

            if (mult >= 5f)
                multiplierText.color = Color.white;
            else
                multiplierText.color = Color.white;
        }
    }
}

