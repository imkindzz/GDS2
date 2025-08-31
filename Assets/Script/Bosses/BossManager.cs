using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{   

    private List<GameObject> Phases = new List<GameObject>();

    private Slider bossHealthSlider;
    private static TextMeshProUGUI bossPhaseText;

    public AudioClip bossMusic;
    

    
    // Start is called before the first frame update
    void Start()
    {
        GameObject BossUI = GameObject.Find("BossUI");

        if (BossUI != null) 
        {
            bossHealthSlider = BossUI.GetComponentInChildren<Slider>();
            bossHealthSlider.enabled = true;

            bossPhaseText = BossUI.GetComponentInChildren<TextMeshProUGUI>(true);
            bossPhaseText.gameObject.SetActive(true);
        }

        GameObject audioSource = GameObject.Find("Audio Source");
        audioSource.GetComponent<AudioSource>().clip = bossMusic;
        audioSource.GetComponent<AudioSource>().Play();


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
