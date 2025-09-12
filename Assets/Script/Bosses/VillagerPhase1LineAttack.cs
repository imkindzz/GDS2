using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerPhase1LineAttack : MonoBehaviour
{
    public int numberOfLines = 9;
    public float screenWidth = 10f;
    private List<float> linePositions = new List<float>();

    public GameObject redLine;
    public float lineHeight = 10f;

    public GameObject bulletUp;
    public GameObject bulletDown;
    public int bulletsPerLine = 20;
    public float spacing = 0.5f;

    public float lineFlashDuration = 3f;
    private List<GameObject> lineGameObjects = new List<GameObject>();


    IEnumerator LineAttack()
    {
        //yield return new WaitForSeconds(2f);

        CalculateLinePositions();

        foreach (float x in linePositions)
        {
            Vector3 linePos = new Vector3(x, 0f, 0f);
            GameObject line = Instantiate(redLine, linePos, Quaternion.identity);
            line.transform.localScale = new Vector3(0.1f, lineHeight, 1f);
            lineGameObjects.Add(line);
        }

        yield return new WaitForSeconds(lineFlashDuration);

        foreach (GameObject line in lineGameObjects)
        {
            Destroy(line);
        }
        lineGameObjects.Clear();

        yield return new WaitForSeconds(0.35f);

        foreach (float x2 in linePositions)
        {
            for (int i = 0; i <bulletsPerLine; i++)
            {
                
                float y = i * spacing - ((bulletsPerLine - 1) * spacing / 2);

                Vector3 bulletDownPos = new Vector3(x2, y + 10, 0f);
                Instantiate(bulletDown, bulletDownPos, Quaternion.identity);
                Vector3 bulletUpPos = new Vector3(x2, y - 10, 0f);
                Instantiate(bulletUp, bulletUpPos, Quaternion.identity);
                
                
            }
        }
    }
    void CalculateLinePositions()
    {


        linePositions.Clear();

        float minX = -screenWidth / 2f;
        float maxX = screenWidth / 2f;
        float minSpacing = 1.0f; // Minimum distance between lines to avoid overlap

        int attempts = 0;
        int maxAttempts = 1000;

        while (linePositions.Count < numberOfLines && attempts < maxAttempts)
        {
            float randomX = Random.Range(minX, maxX);
            bool tooClose = false;

            foreach (float existingX in linePositions)
            {
                if (Mathf.Abs(existingX - randomX) < minSpacing)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                linePositions.Add(randomX);
            }

            attempts++;
        }
    }

    public void StartLineAttack()
    {
        StartCoroutine(LineAttack());
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LineAttack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
