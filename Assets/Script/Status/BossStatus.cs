using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStatus : StatusBase
{

    public GameObject nextPhase;
    private bool _pointsGiven = false;


    [Header("Score Settings")]
    [SerializeField] private int pointsOnDeath = 100;

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
            string phase = /*nextPhase.name*/ "Phase 2";
            string bossName = /*nextPhase.transform.parent.gameObject.name*/ "HobGoblin";
            BossManager.updateBossPhaseText(phase, bossName);


        }

        base.OnDeathState();
    }
    #endregion
}
