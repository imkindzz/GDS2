using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStatus : StatusBase
{

    public GameObject nextPhase;


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
        if (nextPhase != null) {
            nextPhase.SetActive(true);
            string phase = nextPhase.name;
            string bossName = nextPhase.transform.parent.gameObject.name;
            BossManager.updateBossPhaseText(phase, bossName);
        }

        base.OnDeathState();
    }
    #endregion
}
