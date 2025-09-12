using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSummonSoul : MonoBehaviour
{
    [SerializeField] private GameObject soulBody; //the body of the soul summoned
    [SerializeField] private bool isSummoned = false; //whether or not the soul is summoned
    [SerializeField] private float summonDuration = 1f; //time taken for how long the soul stay summoned for
    [SerializeField] private float summonCooldown = 1f; //the cooldown duration after the summoning ended
    [SerializeField] private float returnSpeed = 1f; //the speed of when the soul returns to the main body

    [Header("Soul Link")]
    [SerializeField] private float linkWidth = 1f; //the width of the soul link
    [SerializeField, Range(0, 1)] private float linkMaxOpacity = 1f; //the maximum opacity of the soul link when summoned
    [SerializeField, Range(0, 1)] private float linkMinOpacity = 0f; //the minium opacity of the soul link as the summoning duration decreases

    private PlayerSoulMovement soulMovement;
    private SpriteRenderer spSoulLink;

    private float summonTimer = 0f; //timer that tracks the time taken when the soul is summoned
    private bool onReturn = false; //whether or not the soul returns to the main body
    
    private bool canSummon = true; //whether or not the soul can be summoned
    private Coroutine summonCooldownRoutine; //the cooldown routine

    private GameObject soulLink; //the link between the soul and the main body
    private bool soulLinkMirrorZ = true; //whether or not the soul link is mirrored

    #region Unity methods
    void Awake()
    {
        soulMovement = soulBody.GetComponent<PlayerSoulMovement>();

        soulLink = soulBody.transform.GetChild(0).gameObject;
        spSoulLink = soulLink.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
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
            summonTimer += Time.deltaTime;
            
            DecreaseSoulLinkOpacity();

            if (Input.GetKeyDown(KeyCode.R) || summonTimer >= summonDuration) RemoveSoul();
        }

        if (onReturn) ReturnSoul();

        StretchSoulLink();
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

            StartSummonCooldown(summonCooldown);
        }
    }
    #endregion

    #region Soul cooldown methods
    //starts the summon cooldown
    private void StartSummonCooldown(float duration)
    {
        canSummon = false;

        if (summonCooldownRoutine != null) StopCoroutine(summonCooldownRoutine);
        summonCooldownRoutine = StartCoroutine(CooldownRoutine(duration));
    }

    //ends the summon cooldown
    private void EndSummonCooldown()
    {
        canSummon = true;

        if (summonCooldownRoutine != null)
        {
            StopCoroutine(summonCooldownRoutine);
            summonCooldownRoutine = null;
        }
    }

    //summon cooldown coroutine
    private IEnumerator CooldownRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        EndSummonCooldown();
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
        float t = summonTimer / summonDuration;
        float newOpacity = Mathf.Lerp(linkMaxOpacity, linkMinOpacity, t);

        Color linkColor = spSoulLink.color;
        linkColor.a = newOpacity;
        spSoulLink.color = linkColor;
    }
    #endregion
}
