using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStatus : StatusBase
{

    public GameObject nextPhase;
    private bool _pointsGiven = false;

    
    [Header("Score Settings")]
    [SerializeField] private int pointsOnDeath = 1000;

    #region Health methods
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Debug.Log("Boss is taking damage");
    }
    #endregion

    #region State methods
    public override void OnDeathState()
    {
        if (nextPhase == null && !_pointsGiven)
        {
            _pointsGiven = true;
            ScoreManager.Instance.AddScore(pointsOnDeath);
            Debug.Log($"Enemy died! Awarded {pointsOnDeath} points.");
        }

        if (nextPhase != null) 
        {
            nextPhase.SetActive(true);

        }

        base.OnDeathState();
    }
    #endregion


    private void Start()
    {
        this.healthMeter = FindFirstObjectByType<Slider>();
    }
}
