using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // only if using TextMeshPro

public class GrazeManager : MonoBehaviour
{
    public static GrazeManager Instance; // so other scripts can add graze easily
    public int grazeScore = 0;

    public TextMeshProUGUI grazeText; // assign in inspector

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
