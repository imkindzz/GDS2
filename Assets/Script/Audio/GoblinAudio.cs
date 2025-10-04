using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAudio : MonoBehaviour
{
    [SerializeField] private AudioSource voiceAudioSource; //the audioSource that makes the voice sounds

    [Header("Attack Sounds")]
    [SerializeField] private SfxSoundName attackSfx;
    [SerializeField] private AudioSource attackAudioSource; //the audioSource that makes the attack sounds

    void Awake()
    {
        if (voiceAudioSource) voiceAudioSource.playOnAwake = false;
        else Debug.LogWarning("voiceAudioSource AudioSource in GoblinAudio.cs is null");
        if (attackAudioSource) attackAudioSource.playOnAwake = false;
        else Debug.LogWarning("attackAudioSource AudioSource in GoblinAudio.cs is null");
    }

    void Start()
    {
        PlayRandomGrowl();
    }

    //plays the attack sfx
    public void PlayAttack()
    {
        SoundManager.instance.PlaySound(attackSfx, attackAudioSource);
    }

    //plays a random goblin growl sound
    public void PlayRandomGrowl()
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                SoundManager.instance.PlaySound(SfxSoundName.GoblinGrowl1, voiceAudioSource);
                break;
            case 1:
                SoundManager.instance.PlaySound(SfxSoundName.GoblinGrowl2, voiceAudioSource);
                break;
            default:
                break;
        }
    }

    //plays a random goblin ratty sound
    public void PlayRandomRatty()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                SoundManager.instance.PlaySound(SfxSoundName.GoblinRatty1, voiceAudioSource);
                break;
            case 1:
                SoundManager.instance.PlaySound(SfxSoundName.GoblinRatty2, voiceAudioSource);
                break;
            case 2:
                SoundManager.instance.PlaySound(SfxSoundName.GoblinRatty3, voiceAudioSource);
                break;
            default:
                break;
        }
    }
}
