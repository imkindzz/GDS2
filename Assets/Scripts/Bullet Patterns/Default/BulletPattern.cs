using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletPattern : ScriptableObject
{
    // Bullet prefab assigned in emitter, passed here to pattern
    public abstract void Emit(Transform emitterTransform, GameObject bulletPrefab, Vector3? targetPosition = null);
}
