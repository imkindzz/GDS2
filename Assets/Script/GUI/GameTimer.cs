using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public bool countUp = true;    
    public float startTime = 0f;  
    public float countdownTime = 60f; 

    [Header("UI")]
    public TextMeshProUGUI timerText; 

    private float currentTime;
    private bool timerRunning = true;

    private void Start()
    {
        currentTime = countUp ? startTime : countdownTime;
    }

    private void Update()
    {
        if (!timerRunning) return;

        // Update timer
        if (countUp)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                timerRunning = false;
                OnTimerEnd();
            }
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer Ended!");
    }

    public void PauseTimer() => timerRunning = false;
    public void ResumeTimer() => timerRunning = true;
    public void ResetTimer()
    {
        currentTime = countUp ? startTime : countdownTime;
        timerRunning = true;
    }
}

