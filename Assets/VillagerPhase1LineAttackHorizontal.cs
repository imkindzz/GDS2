using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerPhase1LineAttackHorizontal : MonoBehaviour
{
    public int numberOfLines = 9;
    public float screenHeight = 10f; 
    private List<float> linePositions = new List<float>();

    public GameObject redLine;
    public float lineWidth = 10f; 

    public GameObject bulletLeft;
    public GameObject bulletRight;
    public int bulletsPerLine = 20;
    public float spacing = 0.5f;

    public float lineFlashDuration = 3f;
    private List<GameObject> lineGameObjects = new List<GameObject>();

    IEnumerator LineAttack()
    {
        CalculateLinePositions();

        
        foreach (float y in linePositions)
        {
            Vector3 linePos = new Vector3(0f, y, 0f);
            GameObject line = Instantiate(redLine, linePos, Quaternion.identity);
            line.transform.localScale = new Vector3(lineWidth, 0.1f, 1f); 
            lineGameObjects.Add(line);
        }

        yield return new WaitForSeconds(lineFlashDuration);

        
        foreach (GameObject line in lineGameObjects)
        {
            Destroy(line);
        }
        lineGameObjects.Clear();

        yield return new WaitForSeconds(0.35f);

        
        foreach (float y2 in linePositions)
        {
            for (int i = 0; i < bulletsPerLine; i++)
            {
                float x = i * spacing - ((bulletsPerLine - 1) * spacing / 2);

                Vector3 bulletRightPos = new Vector3(x - 10, y2, 0f); 
                Instantiate(bulletRight, bulletRightPos, Quaternion.identity);

                Vector3 bulletLeftPos = new Vector3(x + 10, y2, 0f); 
                Instantiate(bulletLeft, bulletLeftPos, Quaternion.identity);
            }
        }
    }

    void CalculateLinePositions()
    {
        linePositions.Clear();

        float minY = -screenHeight / 2f;
        float maxY = screenHeight / 2f;
        float minSpacing = 1.0f; 

        int attempts = 0;
        int maxAttempts = 1000;

        while (linePositions.Count < numberOfLines && attempts < maxAttempts)
        {
            float randomY = Random.Range(minY, maxY);
            bool tooClose = false;

            foreach (float existingY in linePositions)
            {
                if (Mathf.Abs(existingY - randomY) < minSpacing)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                linePositions.Add(randomY);
            }

            attempts++;
        }
    }

    public void StartLineAttack()
    {
        StartCoroutine(LineAttack());
    }

    void Start()
    {
        StartCoroutine(LineAttack());
    }

    void Update()
    {

    }
}
