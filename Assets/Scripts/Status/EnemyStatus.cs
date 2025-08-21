using System;
using UnityEngine;

public class EnemyStatus : StatusBase
{
    // Spawner listens to this
    public event Action<EnemyStatus> Died;

    private bool _deathEventFired;

    // Your base should: reduce HP in TakeDamage and call OnDeathState() when HP <= 0.
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Debug.Log("Enemy is taking damage");
    }

    // Called exactly once by StatusBase when the enemy dies
    public override void OnDeathState()
    {
        // Fire BEFORE base in case base destroys the GameObject.
        if (!_deathEventFired)
        {
            _deathEventFired = true;
            Died?.Invoke(this);
        }

        base.OnDeathState(); // likely destroys/despawns
    }
}

