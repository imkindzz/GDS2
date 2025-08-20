using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : StatusBase
{
    #region Health methods
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Debug.Log("Enemy is taking damage");
    }
    #endregion

    #region State methods
    public override void OnDeathState()
    {
        base.OnDeathState();
    }
    #endregion
}
