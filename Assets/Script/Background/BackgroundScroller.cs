using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Background Settings")]
    public Sprite[] backgroundSprites;
    public float scrollSpeed = 2f;

    [Header("Sorting Settings")]
    public string sortingLayerName = "Default";
    public int orderInLayer = 0;

    [Header("Prop Settings")]
    public Sprite[] propSprites;              // Sprites to spawn as props
    public int minProps = 1;                  // Minimum number of props per background
    public int maxProps = 3;                  // Maximum number of props per background
    [Range(0f, 1f)]
    public float propSpawnChance = 0.5f;      // Chance (per background) to spawn props
    public float propScaleMin = 0.8f;         // Scale variation
    public float propScaleMax = 1.2f;

    private float backgroundHeight;
    private float backgroundWidth;
    private bool hasSpawnedNext = false; // prevents multiple spawns

    private void Start()
    {
        // Get size from the sprite’s bounds
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            backgroundHeight = sr.bounds.size.y;
            backgroundWidth = sr.bounds.size.x;

            // Applying sorting settings to the starting background too
            sr.sortingLayerName = sortingLayerName;
            sr.sortingOrder = orderInLayer;

            // Try spawning props here
            TrySpawnProps();
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

        // Spawn the next background earlier so it doesn't leave gaps
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

        // Apply sorting layer and order
        sr.sortingLayerName = sortingLayerName;
        sr.sortingOrder = orderInLayer;

        BackgroundScroller scroller = newBG.AddComponent<BackgroundScroller>();
        scroller.backgroundSprites = backgroundSprites;
        scroller.scrollSpeed = scrollSpeed;
        scroller.sortingLayerName = sortingLayerName;
        scroller.orderInLayer = orderInLayer;
        scroller.propSprites = propSprites;
        scroller.minProps = minProps;
        scroller.maxProps = maxProps;
        scroller.propSpawnChance = propSpawnChance;
        scroller.propScaleMin = propScaleMin;
        scroller.propScaleMax = propScaleMax;

        newBG.transform.position = spawnPos;
    }

    private void TrySpawnProps()
    {
        if (propSprites.Length == 0 || Random.value > propSpawnChance)
            return;

        int propCount = Random.Range(minProps, maxProps + 1);

        for (int i = 0; i < propCount; i++)
        {
            Sprite chosenProp = propSprites[Random.Range(0, propSprites.Length)];

            GameObject prop = new GameObject("Prop");
            SpriteRenderer sr = prop.AddComponent<SpriteRenderer>();
            sr.sprite = chosenProp;

            // Match sorting (props always appear above background)
            sr.sortingLayerName = sortingLayerName;
            sr.sortingOrder = orderInLayer + 1;

            // Random position inside background bounds
            float randX = Random.Range(-backgroundWidth / 2f, backgroundWidth / 2f);
            float randY = Random.Range(-backgroundHeight / 2f, backgroundHeight / 2f);
            prop.transform.parent = transform; // parent to background
            prop.transform.localPosition = new Vector3(randX, randY, 0f);

            // Random scale
            float scale = Random.Range(propScaleMin, propScaleMax);
            prop.transform.localScale = new Vector3(scale, scale, 1f);

            // Random chance to flip horizontally
            if (Random.value < 0.5f)
            {
                prop.transform.localScale = new Vector3(-prop.transform.localScale.x, prop.transform.localScale.y, 1f);
            }
        }
    }
}








