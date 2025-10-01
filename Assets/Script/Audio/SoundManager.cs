using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SfxSoundName
{
    //goblin
    GoblinBossExertion, GoblinCannon, GoblinClubThrow, GoblinClubThrowImpact, 
    GoblinGrowl1, GoblinGrowl2, GoblinRatty1, GoblinRatty2, GoblinRatty3, 
    GoblinSpearThrow,

    //player
    GhostAttack, GhostMovement, PlayerHit, PlayerWarp,

    //other
    AmbienceJungle, DeathSound
}

public class SoundManager : MonoBehaviour
{
    // Singleton instance
    public static SoundManager instance { get; private set; }

    [SerializeField] private List<AudioClip> sfxSoundClips = new List<AudioClip>(); //stores the sounds, where the index align with the variables in the SoundName enum

    [Header("Volume Settings")]
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 1.0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Multiple SoundManager instances found! Destroying the duplicate.");
            Destroy(gameObject);
        }
    }

    #region Public player
    //plays the sfx sound
    public AudioSource PlaySfxSound(SfxSoundName soundName, AudioSource audioSource, bool loop = false)
    {
        return CreateSound(sfxSoundClips[(int)soundName], sfxVolume, loop, audioSource);
    }
    #endregion

    #region Sound player
    //plays the sound
    private void PlaySound(AudioClip clip, AudioSource audioSource, float volume)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    //plays a sound loop
    private void PlayLoop(AudioClip clip, AudioSource audioSource, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    //creates the sound in the scene where it can be stored under specific gameobjects
    private AudioSource CreateSound(AudioClip clip, float volume, bool loop, AudioSource audioSource)
    {
        if (loop) PlayLoop(clip, audioSource, volume);
        else PlaySound(clip, audioSource, volume);

        return audioSource;
    }
    #endregion
}