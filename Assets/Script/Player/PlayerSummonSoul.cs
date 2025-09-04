using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSummonSoul : MonoBehaviour
{
    [SerializeField] private GameObject soulBody; //the body of the soul summoned
    [SerializeField] private bool isSummoned = false; //whether or not the soul is summoned
    [SerializeField] private float summonDuration = 1f; //time taken for how long the soul stay summoned for

    private float summonTimer = 0f; //timer that tracks the time taken when the soul is summoned

    #region Unity methods
    void Start()
    {
        if (isSummoned) SummonSoul();
        else RemoveSoul();
    }

    void Update()
    {
        if (!isSummoned && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            SummonSoul();
        }

        if (isSummoned)
        {
            summonTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.R) || summonTimer >= summonDuration) RemoveSoul();
        }
    }
    #endregion

    #region Soul summoning methods
    //summons the soul into the game
    private void SummonSoul()
    {
        isSummoned = true;
        summonTimer = 0f;

        soulBody.transform.position = transform.position;
        soulBody.SetActive(true);
    }

    //removes the soul from the game
    private void RemoveSoul()
    {
        isSummoned = false;

        soulBody.SetActive(false);
    }
    #endregion
}
