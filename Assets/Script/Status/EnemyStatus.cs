using System;
using UnityEngine;

public class EnemyStatus : StatusBase
{
    // Spawner listens to this
    public event Action<EnemyStatus> Died;

    private bool _deathEventFired;

    [Header("Score Settings")]
    [SerializeField] private int pointsOnDeath = 100;

    // Your base should: reduce HP in TakeDamage and call OnDeathState() when HP <= 0.
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Debug.Log("Enemy is taking damage");
    }
    public override void OnDeathState()
    {
        // Fire BEFORE base in case base destroys the GameObject.
        if (!_deathEventFired)
        {
            _deathEventFired = true;

            ScoreManager.Instance.AddScore(pointsOnDeath);
            Debug.Log($"Enemy died! Awarded {pointsOnDeath} points.");

            // Add souls to the Soul Bar when enemy dies
            if (SoulBarManager.Instance != null)
            {
                SoulBarManager.Instance.AddSouls(10f); 
            }

            Died?.Invoke(this);
        }

        base.OnDeathState(); 
    }

}

