using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected Team _team = null;
    public bool Active = false;

    public abstract int Points { get; }
    protected abstract float TimeToActivate { get; }
    protected abstract float TimeToReactivate { get; }
    protected abstract CharacterType Type { get; }

    public bool IsClear = true;

    public virtual void Activate(Team team, Vector3 position)
    {
        gameObject.SetActive(true);
        IsClear = false;
        Active = true;
        _team = team;
        transform.position = position;
        StartCoroutine(ActivateWait());
    }

    private IEnumerator ActivateWait() 
    {
        yield return new WaitForSeconds(TimeToActivate);
        OnActivate();
    }
    protected abstract void OnActivate();
    public async virtual void Inactivate()
    {
        Active = false;
        OnInactivate();
        await Task.Delay((int)(TimeToReactivate * 1000));
        OnInactivate();
    }
    protected abstract void OnInactivate();

    public virtual void Clear()
    {
        _team = null;
        Active = false;
        IsClear = true;
        gameObject.SetActive(false);
    }
}
