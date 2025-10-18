using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicAudio : MonoBehaviour
{
    [SerializeField] private MusicName bossMusic;

    void Start()
    {
        SoundManager.instance.PlayMusic(bossMusic);
    }
}
