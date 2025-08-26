using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : StatusBase
{
    #region Health methods
    public override void TakeHealth(float amount)
    {
        base.TakeHealth(amount);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Debug.Log("Player is taking damage");
    }
    #endregion

    #region State methods
    public override void OnDeathState()
    {
        //does something
        this.gameObject.SetActive(false);
        Debug.Log("Player is dead");
    }
    #endregion
}
