using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{   

    private List<GameObject> Phases = new List<GameObject>();

    
    public static TextMeshProUGUI bossPhaseText;

    private BossUIManager bossUIManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        bossUIManager = FindFirstObjectByType<BossUIManager>();

        bossUIManager.activateBossUI();
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

        bossPhaseText.text = text;
    }
}
