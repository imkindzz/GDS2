using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : StatusBase
{
    [SerializeField] private GameObject soulBody; //the player's soul body

    [Header("Heart Settings")]
    [SerializeField] private List<Image> heartImages; // drag your 5 heart images here
    private int currentHearts;
    private int maxHearts = 5;

    void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();
    }

    #region Health methods
    public override void TakeHealth(float amount)
    {
        base.TakeHealth(amount);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        Debug.Log("Player is taking damage");

        if (noHealth && currentHearts > 0)
        {
            currentHearts--;           // lose a heart
            currentHealth = maxHealth; // reset health for next heart
            UpdateHeartsUI();

            if (currentHearts <= 0)
            {
                OnDeathState(); // player dies if no hearts left
            }
        }
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

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].gameObject.SetActive(i < currentHearts);
        }
    }

}
