using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSummonSoul : MonoBehaviour
{
    [SerializeField] private GameObject soulBody; //the body of the soul summoned
    [SerializeField] private bool isSummoned = false; //whether or not the soul is summoned
    [SerializeField] private float summonDuration = 1f; //time taken for how long the soul stay summoned for
    [SerializeField] private float returnSpeed = 1f; //the speed of when the soul returns to the main body

    private PlayerSoulMovement soulMovement;

    private float summonTimer = 0f; //timer that tracks the time taken when the soul is summoned
    private bool onReturn = false; //whether or not the soul returns to the main body

    #region Unity methods
    void Awake()
    {
        soulMovement = soulBody.GetComponent<PlayerSoulMovement>();
    }

    void Start()
    {
        if (isSummoned) SummonSoul();
        else soulBody.SetActive(false);
    }

    void Update()
    {
        if (!isSummoned && !onReturn && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            SummonSoul();
        }

        if (isSummoned)
        {
            summonTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.R) || summonTimer >= summonDuration) RemoveSoul();
        }

        if (onReturn) ReturnSoul();
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

        soulMovement.enabled = true;
    }

    //removes the soul from the game
    private void RemoveSoul()
    {
        isSummoned = false;
        onReturn = true;

        soulMovement.enabled = false;
    }

    //returns the soul to the main body
    private void ReturnSoul()
    {
        soulBody.transform.position = Vector2.MoveTowards(soulBody.transform.position, transform.position, returnSpeed * Time.deltaTime);

        float distance = Vector2.Distance(soulBody.transform.position, transform.position);
        if (distance < 0.05f)
        {
            soulBody.SetActive(false);
            onReturn = false;
        }
    }
    #endregion
}
