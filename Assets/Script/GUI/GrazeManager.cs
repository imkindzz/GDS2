using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // only if using TextMeshPro

public class GrazeManager : MonoBehaviour
{
    public static GrazeManager Instance; 
    public int grazeScore = 0;

    public TextMeshProUGUI grazeText;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        grazeText.text = "Graze: " + grazeScore;
    }

    public void AddGraze(int amount)
    {
        grazeScore += amount;
    }
}
