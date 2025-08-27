using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSoulMovement : PlayerMovement
{
    [Header("Wave Movement")]
    public float waveAmplitude = 5f; //the height of the wave
    public float waveFrequency = 3f; //the speed of wave cycle

    float waveTime = 0f; //the time passed using the wave

    #region Player input methods
    //moves the player
    public override void MovePlayer()
    {
        Vector2 waveOffset = Vector2.zero; //the wave that is added to the movement input
        if (!input.Equals(Vector2.zero))
        {
            waveTime += Time.deltaTime;
            float waveMomentum = Mathf.Sin(waveTime * waveFrequency) * waveAmplitude;
            Vector2 waveDirection = new Vector2(-input.y, input.x);

            waveOffset = waveDirection * waveMomentum;
        }
        else waveTime = 0f;

            base.MovePlayer();

        rb.velocity += waveOffset;
    }
    #endregion
}
