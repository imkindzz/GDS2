using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGoblinSuicide : MonoBehaviour
{
    public void PlaySound()
    {
        SoundManager.Instance.PlayGoblinSpearThrow();
    }
}
