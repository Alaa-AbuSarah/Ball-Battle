using UnityEngine;

public class Ball : Singleton<Ball>
{
    public Attacker Owner = null;

    public void Take(Attacker attacker) 
    {
        if (Owner != null) return;
        Owner = attacker;
    }

    private void LateUpdate()
    {
        if (Owner != null)
            transform.position = Owner.ballPosition.position;
    }
}
