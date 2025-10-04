using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGoblinCannon : MonoBehaviour
{
    public void PlaySound()
    {
        SoundManager.Instance.PlayGoblinCannon();
    }
}
