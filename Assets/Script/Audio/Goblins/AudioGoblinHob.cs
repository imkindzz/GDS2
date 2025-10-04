using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGoblinHob : MonoBehaviour
{
    public void PlaySpearThrowSound()
    {
        SoundManager.Instance.PlayGoblinSpearThrow();
    }

    public void PlayClubThrowSound()
    {
        SoundManager.Instance.PlayGoblinClubThrow();
    }

    public void PlayClubThrowSoundImpact()
    {
        SoundManager.Instance.PlayGoblinClubThrowImpact();
    }
}
