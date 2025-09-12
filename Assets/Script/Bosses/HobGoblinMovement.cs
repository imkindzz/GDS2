using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HobGoblinMovement : MonoBehaviour
{
    public float radius = 5f;           
    public float speed = 1f;           
    private Transform parentTransform; 
    private Vector3 centerPosition;     
    private float angle = 0f;           

    void Start()
    {
        if (transform.parent != null)
        {
            parentTransform = transform.parent;
            centerPosition = parentTransform.position; 
        }
        
    }

    void Update()
    {

        angle += speed * Time.deltaTime;

        
        float x = centerPosition.x + Mathf.Cos(angle) * radius;
        float z = centerPosition.z + Mathf.Sin(angle) * radius;

        
        parentTransform.position = new Vector3(x, parentTransform.position.y, z);
    }
}
