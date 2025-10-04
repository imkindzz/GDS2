using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource attackAudioSource; //the audioSource that makes the attack sounds
    [SerializeField] private AudioSource movementAudioSource; //the audioSource that makes the movement sounds
    [SerializeField] private AudioSource hurtAudioSource; //the audioSource that makes the hurt sounds
    [SerializeField] private AudioSource warpAudioSource; //the audioSource that makes the warp sounds

    void Awake()
    {
        if (attackAudioSource) attackAudioSource.playOnAwake = false;
        else Debug.LogWarning("attackAudioSource AudioSource in GoblinAudio.cs is null");
        if (movementAudioSource) movementAudioSource.playOnAwake = false;
        else Debug.LogWarning("movementAudioSource AudioSource in GoblinAudio.cs is null");
        if (warpAudioSource) warpAudioSource.playOnAwake = false;
        else Debug.LogWarning("warpAudioSource AudioSource in GoblinAudio.cs is null");
    }

    #region Loop sounds
    //plays the attack sfx
    public void PlayAttackLoop()
    {
        SoundManager.instance.PlaySound(SfxSoundName.GhostAttack, attackAudioSource, true);
    }

    //stop the attack sfx
    public void StopAttackLoop()
    {
        attackAudioSource.Stop();
    }

    //plays the movement sfx
    public void PlayMovementLoop()
    {
        SoundManager.instance.PlaySound(SfxSoundName.GhostMovement, movementAudioSource, true);
    }

    //stop the attack sfx
    public void StopMovementLoop()
    {
        movementAudioSource.Stop();
    }
    #endregion

    #region One-shot sounds
    //plays the hurt sfx
    public void PlayHurt()
    {
        SoundManager.instance.PlaySound(SfxSoundName.PlayerHit, hurtAudioSource);
    }

    //plays the warp sfx
    public void PlayWarp()
    {
        SoundManager.instance.PlaySound(SfxSoundName.PlayerWarp, warpAudioSource);
    }
    #endregion
}
