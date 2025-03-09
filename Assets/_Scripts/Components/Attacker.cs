using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attacker : Character
{
    [SerializeField] private AttackerStates states = AttackerStates.None;

    [Header("References")]
    public Transform ballPosition = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private AudioClip startClip = null;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 1.5f;
    [SerializeField] private float chaseDistance = 1f;

    [Header("Attack")]
    [SerializeField] private float attackSpeed = 0.75f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private GameObject highlight = null;

    private Vector3 positioningPoint = Vector3.zero;

    public override int Points { get => 2; }
    protected override float TimeToActivate { get => 0.5f; }
    protected override float TimeToReactivate { get => 2.5f; }
    protected override CharacterType Type => CharacterType.Attacker;
    protected override void onEnable() => transform.LookAt(Ball.Instance.transform, Vector3.up);
    protected override void OnActivate()
    {
        ResetAnimator();
        animator.SetTrigger("Run");
        states = AttackerStates.Chase;
        AudioManager.Instance?.PlaySFX(startClip);
    }

    private void Update()
    {
        if (GameManager.Instance.States != GameStates.Active) return;

        switch (states)
        {
            case AttackerStates.None:
                break;
            case AttackerStates.Spawning:
                Spawning();
                break;
            case AttackerStates.Chase:
                Chase();
                break;
            case AttackerStates.Attacking:
                Attacking();
                break;
            case AttackerStates.Positioning:
                Positioning();
                break;
            case AttackerStates.Pass:
                Pass();
                break;
            case AttackerStates.Standby:
                Standby();
                break;
        }
    }

    private void Spawning() => states = AttackerStates.Chase;

    private void Chase()
    {
        if (Ball.Instance.Owner != null) 
        {
            Vector3 dir = (transform.position - _team.opponentGate.position).normalized;
            dir.x *= Random.Range(8, 15);
            dir.y = 0;
            dir.z *= Random.Range(-8, 8);
            positioningPoint = _team.opponentGate.position + dir;
            states = AttackerStates.Positioning;
            return;
        }

        Move(Ball.Instance.transform.position, chaseSpeed);

        if (Vector3.Distance(transform.position, Ball.Instance.transform.position) <= chaseDistance) 
        {
            Ball.Instance.Take(this);
            highlight.SetActive(true);
            states = AttackerStates.Attacking;
            AudioManager.Instance?.PlayClick();
        }
    }

    private void Attacking()
    {
        Move(_team.opponentGate.position, attackSpeed);

        if (Vector3.Distance(transform.position, _team.opponentGate.position) <= attackDistance) 
        {
            states = AttackerStates.None;
            highlight.SetActive(false);
            GameManager.Instance.FinishTheRound(_team);
            Ball.Instance.gameObject.SetActive(false);
        }
    }

    private void Positioning()
    {
        Move(positioningPoint, chaseSpeed);
        if (Vector3.Distance(transform.position, positioningPoint) <= 1) 
        {
            animator.SetTrigger("Idle");
            transform.LookAt(_team.opponentGate, Vector3.up);
            states = AttackerStates.Standby;
        }
    }

    private void Pass()
    {
        Attacker _ = _team.GetNextCharacter(this) as Attacker;

        if (_ is null)
        {
            GameManager.Instance.FinishTheRound(_team, false);
        }

        Ball.Instance.Pass(_);
        animator.SetTrigger("Idle");
        states = AttackerStates.None;
    }

    private void Standby()
    {
        if (Ball.Instance.Owner == this) 
        {
            animator.SetTrigger("Run");
            highlight.SetActive(true);
            states = AttackerStates.Attacking;
            AudioManager.Instance?.PlayClick();
        }

        if (Ball.Instance.Owner == null) 
        {
            animator.SetTrigger("Run");
            states = AttackerStates.Chase;
        }
    }

    protected override void OnInactivate()
    {
        animator.SetTrigger("Throw");
        highlight.SetActive(false);
        states = AttackerStates.Pass;
    }

    protected override void OnFinishRound(bool win)
    {
        ResetAnimator();
        animator.SetTrigger(win ? "Cheer" : "Die");
        states = AttackerStates.None;
    }

    protected override void ReActivate()
    {
        ResetAnimator();
        animator.SetTrigger("Idle");
        states = AttackerStates.Standby;
    }

    private void ResetAnimator() 
    {
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Spawn");
        animator.ResetTrigger("Cheer");
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Throw");
        animator.ResetTrigger("Die");
    }
}
