using UnityEngine;

public class LaserSweeper2D : MonoBehaviour
{
    [Header("Target")]
    public Transform boss;

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f; 
    public float maxAngleDelta = 90f; 

    public enum RotationDirection { Clockwise, CounterClockwise }
    public RotationDirection startingDirection = RotationDirection.Clockwise;

    private float startAngle;
    private int direction; 
    private bool sweepingAway = true;

    void Start()
    {
        if (boss == null)
        {
            Debug.LogError("Boss not assigned!");
            enabled = false;
            return;
        }

        
        Vector2 toLaser = (Vector2)(transform.position - boss.position);
        startAngle = Mathf.Atan2(toLaser.y, toLaser.x) * Mathf.Rad2Deg;

        
        direction = (startingDirection == RotationDirection.Clockwise) ? -1 : 1;
    }

    void Update()
    {
       
        transform.RotateAround(boss.position, Vector3.forward, direction * rotationSpeed * Time.deltaTime);

        
        Vector2 toLaserNow = (Vector2)(transform.position - boss.position);
        float currentAngle = Mathf.Atan2(toLaserNow.y, toLaserNow.x) * Mathf.Rad2Deg;

        
        float deltaAngle = Mathf.DeltaAngle(startAngle, currentAngle);

        if (sweepingAway && Mathf.Abs(deltaAngle) >= maxAngleDelta)
        {
            
            direction *= -1;
            sweepingAway = false;
        }
        else if (!sweepingAway && Mathf.Abs(deltaAngle) <= 0.1f)
        {
            
            direction *= -1;
            sweepingAway = true;
        }
    }

}
