using System;
using UnityEngine;

public class Defender : Character
{
    [SerializeField] private DefenderStates states = DefenderStates.None;

    [Header("References")]
    [SerializeField] private Animator animator = null;

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

    private void Start()
    {
        transform.LookAt(Ball.Instance.transform, Vector3.up);
        animator.SetTrigger("Spawn");
    }

    protected override void OnActivate()
    {
        states = DefenderStates.Spawning;
        startPosition = transform.position;
    }

    private void Update()
    {
        if (GameManager.Instance.States != GameStates.Active) return;

        switch (states)
        {
            case DefenderStates.None:
                break;
            case DefenderStates.Spawning:
                Spawning();
                break;
            case DefenderStates.Standby:
                Standby();
                break;
            case DefenderStates.Chase:
                Chase();
                break;
            case DefenderStates.Inactive:
                Inactive();
                break;
            case DefenderStates.Move:
                Move();
                break;
        }
    }

    private void Spawning()
    {
        states = DefenderStates.Standby;
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
            animator.SetTrigger("Run");
            states = DefenderStates.Move;
            Inactivate();
        }
    }

    private void Inactive()
    {

    }

    private void Move()
    {

        Move(startPosition, moveSpeed);
        if (Vector3.Distance(transform.position, startPosition) <= 0.1f)
        {
            transform.LookAt(_team.opponentGate.position, Vector3.up);
            animator.SetTrigger("Idle");
            states = DefenderStates.Inactive;
        }
    }

    protected override void OnFinishRound(bool win)
    {
        states = DefenderStates.None;
    }

    protected override void OnInactivate()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, startChaseDistance);
    }

    protected override void ReActivate()
    {
        states = DefenderStates.Standby;
    }
}
