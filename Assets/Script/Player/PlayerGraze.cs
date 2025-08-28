using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;  

public class PlayerGraze : MonoBehaviour
{
    public Collider2D damageHitbox;
    public Collider2D grazeHitbox;

    public int grazeScore = 0;
    public TMP_Text grazeScoreText; 

    private void Start()
    {
        UpdateGrazeUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (grazeHitbox.IsTouching(other) && !damageHitbox.IsTouching(other))
            {
                StartCoroutine(HandleGraze(other));
            }
        }
    }

    private System.Collections.IEnumerator HandleGraze(Collider2D bullet)
    {
        yield return new WaitUntil(() => !grazeHitbox.IsTouching(bullet));

        if (!damageHitbox.IsTouching(bullet))
        {
            grazeScore++;
            UpdateGrazeUI();
        }
    }

    private void UpdateGrazeUI()
    {
        if (grazeScoreText != null)
        {
            grazeScoreText.text = "Graze Score: " + grazeScore;
        }
    }
}



