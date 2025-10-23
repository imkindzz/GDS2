using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerNoise : MonoBehaviour
{
    [SerializeField] private bool startRatty = true; //plays a growl at the beginning

    void Start()
    {
        if (startRatty) PlayRandomRatty();
    }

    //plays a random goblin ratty sound
    public void PlayRandomRatty()
    {
        SfxSoundName[] soundArray = new SfxSoundName[] { SfxSoundName.GoblinRatty1, SfxSoundName.GoblinRatty2, SfxSoundName.GoblinRatty3 };
        SoundManager.instance.PlayRandomSound(soundArray, transform, false, 0.5f);
    }
}
