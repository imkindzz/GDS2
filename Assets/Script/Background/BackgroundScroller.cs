using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Background Settings")]
    public Sprite[] backgroundSprites; 
    public float scrollSpeed = 2f;

    [Header("Sorting Settings")]
    public string sortingLayerName = "Default";
    public int orderInLayer = 0;              

    private float backgroundHeight;
    private bool hasSpawnedNext = false; // prevents multiple spawns

    private void Start()
    {
        // Get height from the sprite’s bounds
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            backgroundHeight = sr.bounds.size.y;
            // Applying sorting settings to the starting background too
            sr.sortingLayerName = sortingLayerName;
            sr.sortingOrder = orderInLayer;
        }
        else
        {
            Debug.LogError("No SpriteRenderer found on background!");
        }
    }

    private void Update()
    {
        // Move background down
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Spawn the next background earlier so it doesnt leave fucking gaps
        if (!hasSpawnedNext && transform.position.y < 0f)
        {
            SpawnNextBackground();
            hasSpawnedNext = true;
        }

        // Destroy once it's fully off the screen
        if (transform.position.y < -backgroundHeight)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnNextBackground()
    {
        // Pick a random sprite from list
        Sprite randomSprite = backgroundSprites[Random.Range(0, backgroundSprites.Length)];

        // Spawn new background directly above this one
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + backgroundHeight, transform.position.z);

        GameObject newBG = new GameObject("Background");
        SpriteRenderer sr = newBG.AddComponent<SpriteRenderer>();
        sr.sprite = randomSprite;

        // Apply sorting layer and order so it stops overlapping like a bum
        sr.sortingLayerName = sortingLayerName;
        sr.sortingOrder = orderInLayer;

        BackgroundScroller scroller = newBG.AddComponent<BackgroundScroller>();
        scroller.backgroundSprites = backgroundSprites;
        scroller.scrollSpeed = scrollSpeed;
        scroller.sortingLayerName = sortingLayerName;
        scroller.orderInLayer = orderInLayer;

        newBG.transform.position = spawnPos;
    }
}







