using UnityEngine;

public class AttackOnPause2D : MonoBehaviour
{
    [SerializeField] Transform emitter;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] BulletPattern pattern;
    [SerializeField] Transform player;

    ChargePauseRandomLockedImpl move;
    bool wasCharging = true;

    void Awake()
    {
        if (!emitter) emitter = transform;
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
        move = GetComponent<ChargePauseRandomLockedImpl>();
    }

    void Update()
    {
        if (!move || !pattern || !bulletPrefab || !player) return;
        bool charging = move.IsCharging;
        if (wasCharging && !charging)
            pattern.Emit(emitter, bulletPrefab, player.position);
        wasCharging = charging;
    }
}
