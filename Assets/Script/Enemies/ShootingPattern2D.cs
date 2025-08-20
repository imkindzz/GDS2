// ShootingPattern2D.cs
using UnityEngine;

public abstract class ShootingPattern2D : ScriptableObject
{
    public abstract void Tick(MonoBehaviour owner, ParticleBulletEmitter2D emitter, Transform self, Transform player, float t);
}
