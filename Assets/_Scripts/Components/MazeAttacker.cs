using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class MazeAttacker : MonoBehaviour
{
    [SerializeField] private float time = 60f;
    [SerializeField] private List<Transform> ballPoints = new List<Transform>();
    [SerializeField] private List<Transform> mazes = new List<Transform>();

    [Space]

    [SerializeField] private Transform ball = null;
    [SerializeField] private Transform gate = null;
    [SerializeField] private TMP_Text text = null;
    [SerializeField] private GameObject winPanel = null;
    [SerializeField] private GameObject losePanel = null;

    private NavMeshAgent agent = null;
    private MazeAttackerStates states = MazeAttackerStates.None;
    private Animator animator = null;

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        mazes[Random.Range(0, mazes.Count)].gameObject.SetActive(true);
        ball.GetComponent<Ball>().Owner = null;
        ball.gameObject.SetActive(true);
        ball.position = ballPoints[Random.Range(0, ballPoints.Count)].position;

        agent.SetDestination(ball.position);
        states = MazeAttackerStates.ChaseBall;
    }

    private void Update()
    {
        if (time <= 0 || states == MazeAttackerStates.None) return;

        time -= Time.deltaTime;

        if (time <= 0)
        {
            states = MazeAttackerStates.None;
            text.gameObject.SetActive(false);
            animator.SetTrigger("Die");
            losePanel.SetActive(true);
            agent.Stop();
        }

        text.text = ((int)time).ToString();


        switch (states)
        {
            case MazeAttackerStates.None:
                break;
            case MazeAttackerStates.ChaseBall:
                ChaseBall();
                break;
            case MazeAttackerStates.Attack:
                Attack();
                break;
            default:
                break;
        }
        
    }

    private void Attack()
    {
        ball.position = transform.position + transform.forward * 2 + transform.up;

        if (Vector3.Distance(transform.position, gate.position) < agent.stoppingDistance+0.5f) 
        {
            states = MazeAttackerStates.None;
            text.gameObject.SetActive(false);
            animator.SetTrigger("Cheer");
            winPanel.SetActive(true);
        }
    }

    private void ChaseBall()
    {
        if (Vector3.Distance(transform.position, ball.position) < agent.stoppingDistance + 0.5f)
        {
            agent.SetDestination(gate.position);
            states = MazeAttackerStates.Attack;
            AudioManager.Instance?.PlayClick();
        }
    }
}
