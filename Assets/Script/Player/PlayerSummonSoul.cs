using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSummonSoul : MonoBehaviour
{
    [SerializeField] private GameObject soulBody; //the body of the soul summoned
    [SerializeField] private bool isSummoned = false; //whether or not the soul is summoned
    [SerializeField] private float summonDuration = 1f; //time taken for how long the soul stay summoned for
    [SerializeField] private float returnSpeed = 1f; //the speed of when the soul returns to the main body

    [Header("Soul Link")]
    [SerializeField] private GameObject soulLink; //the link between the soul and the main body
    [SerializeField] private float linkWidth = 1f; //the width of the soul link

    private PlayerSoulMovement soulMovement;

    private float summonTimer = 0f; //timer that tracks the time taken when the soul is summoned
    private bool onReturn = false; //whether or not the soul returns to the main body

    private bool soulLinkMirrorZ = true; //whether or not the soul link is mirrored

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
    #endregion
}
