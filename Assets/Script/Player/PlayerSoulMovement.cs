using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSoulMovement : PlayerMovement
{
    [Header("Wave Movement")]
    [SerializeField] private float waveAmplitude = 5f; //the height of the wave
    [SerializeField] private float waveFrequency = 3f; //the speed of wave cycle

    [Header("Drift Movement")]
    [Tooltip("Smaller = faster slowdown.")]
    [SerializeField] private float driftHalfLife = 0.25f; //half-life of drift speed in seconds
    [SerializeField] private float minDriftSpeed = 0.05f; //the minimum speed that the drift will stop at

    private float waveTime = 0f; //the time passed using the wave
    private Vector2 lastMoveVelocity; //the last velocity made before it changes
    private float driftTimer = 0f; //the time that passes for the drift

    #region Player input methods
    //moves the player
    public override void MovePlayer()
    {
        Vector2 waveOffset = Vector2.zero; //the wave that is added to the movement input
        bool hasInput = !input.Equals(Vector2.zero);

        if (hasInput)
        {
            waveTime += Time.deltaTime;
            float radians = (waveTime * waveFrequency) - (Mathf.PI / 2);
            float waveMomentum = Mathf.Sin(radians) * waveAmplitude;
            Vector2 waveDirection = new Vector2(-input.y, input.x);

            waveOffset = waveDirection * waveMomentum;
        }
        else waveTime = 0f;

        base.MovePlayer();

        if (hasInput)
        {
            lastMoveVelocity = rb.velocity; // cache straight movement for drift
            rb.velocity += waveOffset;
            driftTimer = 0f;
        }
        else
        {
            driftTimer += Time.deltaTime;

            //exponential decay that decreases the drift lifespan overtime
            float factor = (driftHalfLife <= 0f) ? 0f : Mathf.Pow(0.5f, driftTimer / driftHalfLife);

            Vector2 drift = lastMoveVelocity * factor;

            //measures the speed judging by the squared length of the drift
            if (drift.sqrMagnitude < (minDriftSpeed * minDriftSpeed)) drift = Vector2.zero;

            rb.velocity = drift;
        }

    }
    #endregion
}
