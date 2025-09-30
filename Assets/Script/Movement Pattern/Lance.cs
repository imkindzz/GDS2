using UnityEngine;

[CreateAssetMenu(menuName = "MovementPatterns/ChargePauseRandomLocked2D")]
public class ChargePauseRandomLocked2D : MovementPattern2D
{
    [SerializeField] float chargeSpeed = 10f;
    [SerializeField] Vector2 chargeTimeRange = new Vector2(0.45f, 0.75f);
    [SerializeField] Vector2 pauseTimeRange = new Vector2(0.25f, 0.45f);
    [SerializeField] float aimJitterDeg = 25f;
    [SerializeField, Range(0f, 1f)] float randomDashProbability = 0.35f;

    public override Vector2 EvaluateVelocity(Transform self, Transform player, float t)
    {
        var impl = self.GetComponent<ChargePauseRandomLockedImpl>();
        if (!impl)
        {
            impl = self.gameObject.AddComponent<ChargePauseRandomLockedImpl>();
            impl.Init(chargeSpeed, chargeTimeRange, pauseTimeRange, aimJitterDeg, randomDashProbability, player);
        }
        return impl.Eval();
    }
}

public class ChargePauseRandomLockedImpl : MonoBehaviour
{
    float chargeSpeed;
    Vector2 chargeTimeRange, pauseTimeRange;
    float aimJitterDeg, randomDashProb;

    Transform player;

    float timer, chargeTime, pauseTime, cycle;
    Vector2 dashDir = Vector2.down;
    public bool IsCharging { get; private set; } = true;

    public void Init(float speed, Vector2 cRange, Vector2 pRange, float jitterDeg, float randProb, Transform target)
    {
        chargeSpeed = speed;
        chargeTimeRange = new Vector2(Mathf.Max(0.01f, Mathf.Min(cRange.x, cRange.y)), Mathf.Max(0.01f, Mathf.Max(cRange.x, cRange.y)));
        pauseTimeRange = new Vector2(Mathf.Max(0f, Mathf.Min(pRange.x, pRange.y)), Mathf.Max(0f, Mathf.Max(pRange.x, pRange.y)));
        aimJitterDeg = jitterDeg;
        randomDashProb = Mathf.Clamp01(randProb);
        player = target;
        timer = 0f;
        RollNewCycle(true);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cycle) { timer -= cycle; RollNewCycle(false); }
        IsCharging = timer < chargeTime;
    }

    void RollNewCycle(bool first)
    {
        chargeTime = Random.Range(chargeTimeRange.x, chargeTimeRange.y);
        pauseTime = Random.Range(pauseTimeRange.x, pauseTimeRange.y);
        cycle = chargeTime + pauseTime;

        if (player && (Random.value > randomDashProb))
        {
            Vector2 to = (Vector2)player.position - (Vector2)transform.position;
            if (to.sqrMagnitude < 1e-6f) to = Vector2.up;
            float baseAng = Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg;
            float jitter = Random.Range(-aimJitterDeg, aimJitterDeg);
            float ang = (baseAng + jitter) * Mathf.Deg2Rad;
            dashDir = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)).normalized;
        }
        else
        {
            float ang = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            dashDir = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
        }
    }

    public Vector2 Eval()
    {
        return IsCharging ? dashDir * chargeSpeed : Vector2.zero;
    }
}
