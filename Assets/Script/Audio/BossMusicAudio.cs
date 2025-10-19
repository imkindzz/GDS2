using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicAudio : MonoBehaviour
{
    [SerializeField] private MusicName bossMusic;

    void Start()
    {
        StartCoroutine(SoundManager.instance.PlayMusicPreloaded(bossMusic));
    }
}
