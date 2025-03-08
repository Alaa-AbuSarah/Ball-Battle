using System.Collections;
using UnityEngine;

public class SimpleEnemyTeam : Team
{
    [SerializeField] private float actionTime = 2f;
    [SerializeField] private float rang = 8f;

    private IEnumerator TakeAction()
    {
        Vector3 pos = transform.position + new Vector3(Random.Range(-rang, rang), 0, Random.Range(-rang, rang));
        SpawnCharacter(pos);
        yield return new WaitForSeconds(actionTime);

        if (GameManager.Instance.States == GameStates.Active)
            StartCoroutine(TakeAction());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rang);
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * -rang);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rang);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * -rang);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * rang);
    }

    protected override void OnStartRound() => StartCoroutine(TakeAction());
}
