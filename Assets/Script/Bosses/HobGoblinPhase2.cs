using UnityEngine;

public class HobGoblinPhase2 : MonoBehaviour
{
    public Transform clubStartpos;
    public Vector2 club1pos;
    public Vector2 club2pos;

    private BossState currentState;
    private float stateTimer;
    private Animator animator;

    public enum BossState
    {
        Idle,
        Club1,
        Club2,
        Return,
        Attack
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        EnterState(BossState.Club1);
    }

    void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                break;

            case BossState.Club1:
                MoveTo(club1pos, BossState.Attack);
                break;

            case BossState.Club2:
                MoveTo(club2pos, BossState.Attack);
                break;

            case BossState.Return:
                if (clubStartpos != null)
                    MoveTo(clubStartpos.position, Random.value < 0.5f ? BossState.Club1 : BossState.Club2);
                break;

            case BossState.Attack:
                HandleAttack();
                break;
        }
    }

    void EnterState(BossState newState)
    {
        currentState = newState;
        stateTimer = 0f;

        switch (newState)
        {
            case BossState.Club1:
            case BossState.Club2:
                GetComponent<GoblinAudio>()?.PlayAction();
                break;

            case BossState.Attack:
                GetComponent<BulletEmitter>().enabled = true;

                if (animator != null)
                    animator.SetTrigger("Throw");

                break;

            case BossState.Return:
                GetComponent<BulletEmitter>().enabled = false;
                break;
        }
    }

    void MoveTo(Vector2 target, BossState nextState)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, 2f * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.01f)
        {
            EnterState(nextState);
        }
    }

    void HandleAttack()
    {
        stateTimer += Time.deltaTime;

        if (stateTimer >= 2f)
        {
            EnterState(BossState.Return);
        }
    }
}
