using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinNoise : MonoBehaviour
{
    [SerializeField] private bool startGrowl = true; //plays a growl at the beginning

    void Start()
    {
        if (startGrowl) PlayRandomGrowl();
    }

    //plays a random goblin growl sound
    public void PlayRandomGrowl()
    {
        SfxSoundName[] soundArray = new SfxSoundName[] { SfxSoundName.GoblinGrowl1, SfxSoundName.GoblinGrowl2 };
        SoundManager.instance.PlayRandomSound(soundArray, transform);
    }

    //plays a random goblin ratty sound
    public void PlayRandomRatty()
    {
        SfxSoundName[] soundArray = new SfxSoundName[] { SfxSoundName.GoblinRatty1, SfxSoundName.GoblinRatty2, SfxSoundName.GoblinRatty3 };
        SoundManager.instance.PlayRandomSound(soundArray, transform);
    }
}
