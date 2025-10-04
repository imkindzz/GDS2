using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource attackAudioSource; //the audioSource that makes the attack sounds
    private AudioSource movementAudioSource; //the audioSource that makes the movement sounds

    void Awake()
    {
        if (attackAudioSource) attackAudioSource.playOnAwake = false;
        else Debug.LogWarning("attackAudioSource AudioSource in GoblinAudio.cs is null");
        if (movementAudioSource) movementAudioSource.playOnAwake = false;
        else Debug.LogWarning("movementAudioSource AudioSource in GoblinAudio.cs is null");
    }

    #region Loop sounds
    //plays the attack sfx
    public void PlayAttackLoop()
    {
        attackAudioSource = SoundManager.instance.PlaySound(SfxSoundName.GhostAttack, transform, true);
    }

    //stop the attack sfx
    public void StopAttackLoop()
    {
        SoundManager.instance.StopSoundLoop(attackAudioSource);
    }

    //plays the movement sfx
    public void PlayMovementLoop()
    {
        movementAudioSource = SoundManager.instance.PlaySound(SfxSoundName.GhostMovement, transform, true);
    }

    //stop the attack sfx
    public void StopMovementLoop()
    {
        SoundManager.instance.StopSoundLoop(movementAudioSource);
    }
    #endregion

    #region One-shot sounds
    //plays the hurt sfx
    public void PlayHurt()
    {
        SoundManager.instance.PlaySound(SfxSoundName.PlayerHit, transform);
    }

    //plays the warp sfx
    public void PlayWarp()
    {
        SoundManager.instance.PlaySound(SfxSoundName.PlayerWarp, transform);
    }
    #endregion
}
