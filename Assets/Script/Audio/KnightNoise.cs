using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightNoise : MonoBehaviour
{
    [SerializeField] private SfxSoundName clingsfx;
    [SerializeField] private bool startCling = true;
    [SerializeField] private SfxSoundName swordSwingSfx;

    void Start()
    {
        if (startCling) SoundManager.instance.PlaySound(clingsfx);
    }

    //plays the sword swing sound
    public void PlaySwordSwingSfx()
    {
        SoundManager.instance.PlaySound(swordSwingSfx);
    }
}
