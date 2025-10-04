using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGoblinSpear : MonoBehaviour
{
    public void PlaySound()
    {
        SoundManager.Instance.PlayGoblinSpearThrow();
    }
}
