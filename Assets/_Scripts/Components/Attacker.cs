using System;
using UnityEngine;

public class Attacker : Character
{
    [SerializeField] private AttackerStates states = AttackerStates.None;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 1.5f;
    [SerializeField] private float chaseDistance = 1f;
    public Transform ballPosition = null;

    public override int Points { get => 2; }
    protected override float TimeToActivate { get => 0.5f; }
    protected override float TimeToReactivate { get => throw new System.NotImplementedException(); }
    protected override CharacterType Type => CharacterType.Attacker;

    protected override void OnActivate()
    {
        states = AttackerStates.Spawning;
    }

    private void Update()
    {
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
            case AttackerStates.Inactive:
                Inactive();
                break;
        }
    }

    private void Spawning()
    {
        states = AttackerStates.Chase;
    }

    private void Chase()
    {
        if (Ball.Instance.Owner != null) 
        {
            states = AttackerStates.Positioning;
            return;
        }

        transform.LookAt(Ball.Instance.transform, Vector3.up);
        transform.position += transform.forward * chaseSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, Ball.Instance.transform.position) <= chaseDistance) 
        {
            Ball.Instance.Take(this);
            states = AttackerStates.Attacking;
        }
    }

    private void Attacking()
    {
        
    }

    private void Positioning()
    {
        
    }

    private void Pass()
    {
        throw new NotImplementedException();
    }

    private void Inactive()
    {
        throw new NotImplementedException();
    }

    protected override void OnInactivate()
    {
        
    }
}
