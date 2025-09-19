using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotate : MonoBehaviour
{
    [Range(0f, 360f)]
    public float setRotationZ = 0f;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, setRotationZ);
    }
}
