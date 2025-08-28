using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : StatusBase
{
    [SerializeField] private GameObject soulBody; //the player's soul body

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
        soulBody.SetActive(false);

        Debug.Log("Player is dead");
    }
    #endregion
}
