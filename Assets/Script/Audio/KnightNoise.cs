using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightNoise : MonoBehaviour
{
    [SerializeField] private SfxSoundName swordSwingSfx;

    //plays the sword swing sound
    public void PlaySwordSwingSfx()
    {
        SoundManager.instance.PlaySound(swordSwingSfx);
    }
}
