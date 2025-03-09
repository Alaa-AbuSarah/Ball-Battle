using UnityEngine;

public class Defender : Character
{
    [SerializeField] private DefenderStates states = DefenderStates.None;

    [Header("References")]
    [SerializeField] private Animator animator = null;
    [SerializeField] private AudioClip startClip = null;
    [SerializeField] private AudioClip catchClip = null;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 1f;
    [SerializeField] private float startChaseDistance = 2f;
    [SerializeField] private float endChaseDistance = 1f;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 2f;

    private Attacker target = null;
    private Vector3 startPosition = Vector3.zero;
    public override int Points => 3;
    protected override float TimeToActivate => 0.5f;
    protected override float TimeToReactivate => 4;
    protected override CharacterType Type => CharacterType.Defender;

    protected override void onEnable() => transform.LookAt(Ball.Instance.transform, Vector3.up);

    protected override void OnActivate()
    {
        startPosition = transform.position;
        states = DefenderStates.Standby;
        AudioManager.Instance?.PlaySFX(startClip);
    }

    private void Update()
    {
        if (GameManager.Instance.States != GameStates.Active) return;

        switch (states)
        {
            case DefenderStates.None:
                break;
            case DefenderStates.Standby:
                Standby();
                break;
            case DefenderStates.Chase:
                Chase();
                break;
            case DefenderStates.Move:
                Move();
                break;
        }
    }

    private void Standby()
    {
        if (target != null) 
        {
            animator.SetTrigger("Run");
            states = DefenderStates.Chase;
        }

        if (Ball.Instance.Owner != null && Vector3.Distance(transform.position, Ball.Instance.Owner.transform.position) <= startChaseDistance)
            target = Ball.Instance.Owner;
    }

    private void Chase()
    {
        Move(target.transform.position, chaseSpeed);

        if (!target.Active)
        {
            states = DefenderStates.Move;
            target = null;
            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= endChaseDistance)
        {
            target.Inactivate();
            target = null;
            states = DefenderStates.Move;
            AudioManager.Instance?.PlaySFX(catchClip);
            Inactivate();
        }
    }

    private void Move()
    {
        Move(startPosition, moveSpeed);

        if (Vector3.Distance(transform.position, startPosition) <= 0.1f)
        {
            transform.LookAt(_team.opponentGate.position, Vector3.up);
            animator.SetTrigger("Idle");
            if (!Active)
                states = DefenderStates.None;
        }
    }

    protected override void OnFinishRound(bool win)
    {
        ResetAnimator();
        animator.SetTrigger(win ? "Cheer" : "Die");
        states = DefenderStates.None;
    }

    protected override void OnInactivate() { }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, startChaseDistance);
    }

    protected override void ReActivate()
    {
        states = DefenderStates.Standby;
    }

    private void ResetAnimator()
    {
        animator.ResetTrigger("Spawn");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Cheer");
        animator.ResetTrigger("Die");
    }
}
