using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParallaxBackground : MonoBehaviour
{
    public float parallaxAmount;
    private RectTransform rectTransform;
    private Vector2 initialPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        Vector2 mouse = Input.mousePosition;
        float x = (mouse.x / Screen.width - 0.5f) * 2f;
        float y = (mouse.y / Screen.height - 0.5f) * 2f;

        Vector2 offset = new Vector2(x, y) * parallaxAmount;
        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            initialPos + offset,
            Time.deltaTime * 5f
        );

    }
}
