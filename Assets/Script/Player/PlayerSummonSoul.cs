using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSummonSoul : MonoBehaviour
{
    [SerializeField] private GameObject soulBody; //the body of the soul summoned
    [SerializeField] private bool isSummoned = false; //whether or not the soul is summoned
    [SerializeField] private float returnSpeed = 1f; //the speed of when the soul returns to the main body

    [Header("Idle State")]
    [SerializeField] private float idleDuration = 1f; //the duration it takes when the player is idle
    [SerializeField] private GameObject idleUIRoot; //the gameobject that displays the idle timer
    [SerializeField] private Image idleUIImage; //the image of the idle timer

    [Header("Cooldown")]
    [SerializeField] private float summonCooldown = 1f; //the cooldown duration after the summoning ended
    [SerializeField] private GameObject cooldownUIRoot; //the gameobject that displays the summon cooldown
    [SerializeField] private Image cooldownUIImage; //the image of the summon cooldown

    [Header("Soul Link")]
    [SerializeField] private float linkWidth = 1f; //the width of the soul link
    [SerializeField, Range(0, 1)] private float linkMaxOpacity = 1f; //the maximum opacity of the soul link when summoned
    [SerializeField, Range(0, 1)] private float linkMinOpacity = 0f; //the minium opacity of the soul link as the summoning duration decreases

    private PlayerSoulMovement soulMovement;
    private SpriteRenderer spSoulLink;
    private bool onReturn = false; //whether or not the soul returns to the main body
    
    private bool canSummon = true; //whether or not the soul can be summoned
    private bool onCooldown = false; //whether or not the soul summon is on cooldown
    private float cooldownTimer = 0; //time taken when on the cooldown
    private bool onIdle = false; //whether or not the soul is idle
    private float idleTimer = 0; //time taken when the soul is idle

    private bool firstMoveMade = false; //whether or not the first movement input has been made

    private GameObject soulLink; //the link between the soul and the main body
    private bool soulLinkMirrorZ = true; //whether or not the soul link is mirrored

    #region Unity methods
    void Awake()
    {
        soulMovement = soulBody.GetComponent<PlayerSoulMovement>();

        soulLink = soulBody.transform.Find("Soul Link")?.gameObject;
        spSoulLink = soulLink ? soulLink.GetComponent<SpriteRenderer>() : null;
    }

    void Start()
    {
        ShowCooldownUI(false);
        ShowIdleTimerUI(false);

        if (isSummoned) SummonSoul();
        else soulBody.SetActive(false);
    }

    void Update()
    {
        if (!isSummoned && !onReturn && canSummon && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            SummonSoul();
        }

        if (isSummoned)
        {
            if (idleDuration > 0)
            {
                //prevents showing the idle timer on the first movement
                if (!firstMoveMade) firstMoveMade = soulMovement.input.sqrMagnitude >= 0.0001f;

                //handles the idle state
                if (firstMoveMade)
                {
                    if (soulMovement.input.sqrMagnitude < 0.0001f)
                    {
                        if (!onIdle) StartIdleTimer();
                        OnIdleTime();
                        DecreaseSoulLinkOpacity();
                    }
                    else
                    {
                        StopIdleTimer();
                        ResetSoulLinkOpacity();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R)) RemoveSoul();
        }

        if (onReturn) ReturnSoul();

        if (onCooldown) OnSummonCooldown();

        StretchSoulLink();
    }
    #endregion

    #region Soul summoning methods
    //summons the soul into the game
    private void SummonSoul()
    {
        isSummoned = true;

        soulBody.transform.position = transform.position;
        soulBody.SetActive(true);

        soulMovement.enabled = true;
        firstMoveMade = false;
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

            if (summonCooldown > 0) StartSummonCooldown();
        }
    }
    #endregion

    #region Idle soul methods
    //displays the idle timer UI
    private void ShowIdleTimerUI(bool show)
    {
        if (idleUIRoot) idleUIRoot.SetActive(show);
    }

    //updates the UI of the idle timer
    private void UpdateIdleTimerUI()
    {
        float fillAmount = 1f - (idleTimer / idleDuration);
        if (idleUIImage) idleUIImage.fillAmount = fillAmount;
    }

    //starts the idle timer
    private void StartIdleTimer()
    {
        onIdle = true;
        idleTimer = 0f;

        ShowIdleTimerUI(true);
    }

    //stops the idle timer the summon cooldown
    private void StopIdleTimer()
    {
        onIdle = false;
        ShowIdleTimerUI(false);
    }

    //when on summon cooldown
    private void OnIdleTime()
    {
        idleTimer += Time.deltaTime;
        UpdateIdleTimerUI();

        if (idleTimer >= idleDuration)
        {
            onIdle = false;
            ShowIdleTimerUI(false);

            RemoveSoul();
        }
    }
    #endregion

    #region Summon cooldown methods
    //displays the cooldown timer UI
    private void ShowCooldownUI(bool show)
    {
        if (cooldownUIRoot) cooldownUIRoot.SetActive(show);
    }

    //updates the UI of the summon cooldown timer
    private void UpdateCooldownUI()
    {
        float fillAmount = 1f - (cooldownTimer / summonCooldown);
        if (cooldownUIImage) cooldownUIImage.fillAmount = fillAmount;
    }

    //starts the summon cooldown
    private void StartSummonCooldown()
    {
        canSummon = false;

        onCooldown = true;
        cooldownTimer = 0f;

        ShowCooldownUI(true);
    }

    //when on summon cooldown
    private void OnSummonCooldown()
    {
        cooldownTimer += Time.deltaTime;
        UpdateCooldownUI();

        if (cooldownTimer >= summonCooldown)
        {
            onCooldown = false;
            canSummon = true;
            ShowCooldownUI(false);
        }
    }
    #endregion

    #region Soul link methods
    //stretches the soul link
    private void StretchSoulLink()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = soulBody.transform.position;

        Vector3 centerPos = (startPosition + endPosition) / 2f;
        soulLink.transform.position = centerPos;

        Vector3 direction = endPosition - startPosition;
        direction = Vector3.Normalize(direction);
        soulLink.transform.right = direction;
        
        if (soulLinkMirrorZ) soulLink.transform.right *= -1f;

        Vector3 scale = new Vector3(1, linkWidth, 1);
        scale.x = Vector3.Distance(startPosition, endPosition);
        soulLink.transform.localScale = scale;
    }

    //decreases the soul link opacity overtime
    private void DecreaseSoulLinkOpacity()
    {
        float t = idleTimer / idleDuration;
        float newOpacity = Mathf.Lerp(linkMaxOpacity, linkMinOpacity, t);

        Color linkColor = spSoulLink.color;
        linkColor.a = newOpacity;
        spSoulLink.color = linkColor;
    }

    //reset soul link color
    private void ResetSoulLinkOpacity()
    {
        Color linkColor = spSoulLink.color;
        linkColor.a = 1;
        spSoulLink.color = linkColor;
    }
    #endregion
}
