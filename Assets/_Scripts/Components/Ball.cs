using UnityEngine;

public class Ball : Singleton<Ball>
{
    public Attacker Owner = null;

    private bool passing = false;
    public void Take(Attacker attacker) 
    {
        if (Owner != null) return;
        Owner = attacker;
    }

    private void LateUpdate()
    {
        if (Owner is null) return;

        if (passing)
        {
            transform.position = Vector3.Lerp(transform.position, Owner.ballPosition.position, Time.deltaTime * 1.5f);

            if (Vector3.Distance(transform.position, Owner.ballPosition.position) <= 0.1f)
                passing = false;
        }
        else
            transform.position = Owner.ballPosition.position;
    }

    public void Pass(Attacker attacker) 
    {
        passing = true;
        Owner = attacker;
    }

    public void Respawn(Vector3 position) 
    {
        Owner = null;
        transform.position = position;
        gameObject.SetActive(true);
    }
}
