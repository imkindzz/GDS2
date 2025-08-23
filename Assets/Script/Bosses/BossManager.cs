using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossManager : MonoBehaviour
{   
    [SerializeField]
    public static TextMeshProUGUI bossText;

    private List<GameObject> Phases = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        bossText = FindFirstObjectByType<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void updateBossPhaseText(string phase, string bossName)
    {
        string text = bossName
            + " - "
            + phase;

        bossText.text = text;
    }
}
