using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GrazeUI : MonoBehaviour
{
    public TextMeshProUGUI text; 
    Vector3 originalScale;
    Color originalColor;

    void Awake()
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();
        originalScale = text.transform.localScale;
        originalColor = text.color;
        text.text = "Graze: 0";
    }

    public void OnGraze(int newScore)
    {
        text.text = "Graze: " + newScore;
        StopAllCoroutines();
        StartCoroutine(Pop());
    }

    System.Collections.IEnumerator Pop()
    {
        text.transform.localScale = originalScale * 1.4f;
        text.color = Color.yellow;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f; // ~0.17s
            text.transform.localScale = Vector3.Lerp(text.transform.localScale, originalScale, t);
            text.color = Color.Lerp(text.color, originalColor, t);
            yield return null;
        }

        text.transform.localScale = originalScale;
        text.color = originalColor;
    }
}



