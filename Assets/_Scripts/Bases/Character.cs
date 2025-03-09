using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    protected Team _team = null;
    public bool Active = false;
    [SerializeField] private Image image = null;

    [Space]

    [SerializeField] private UnityEvent onSpawn = null;
    [SerializeField] private UnityEvent onActivate = null;
    [SerializeField] private UnityEvent onInactivate = null;
    [SerializeField] private UnityEvent onReActivate = null;

    public abstract int Points { get; }
    protected abstract float TimeToActivate { get; }
    protected abstract float TimeToReactivate { get; }
    protected abstract CharacterType Type { get; }

    public bool IsClear = true;

    private float timer = 0;

    private void OnEnable()
    {
        GameManager.FinishRound -= FinishRound;
        GameManager.FinishRound += FinishRound;

        onEnable();
    }
    protected virtual void onEnable() { }
    private void OnDisable() => GameManager.FinishRound -= FinishRound;
    public virtual void Activate(Team team, Vector3 position)
    {
        onSpawn?.Invoke();
        gameObject.SetActive(true);
        IsClear = false;
        Active = true;
        _team = team;
        transform.position = position;
        image.color = _team.color;
        StartCoroutine(ActivateWait());
    }

    private IEnumerator ActivateWait() 
    {
        yield return new WaitForSeconds(TimeToActivate);
        onActivate?.Invoke();
        OnActivate();
    }
    protected abstract void OnActivate();
    protected abstract void ReActivate();
    public virtual void Inactivate()
    {
        Active = false;
        onInactivate?.Invoke();
        OnInactivate();
        timer = 0;
        StartCoroutine(InactivateWait());
    }

    private IEnumerator InactivateWait()
    {
        while (timer < TimeToReactivate)
        {
            timer += Time.deltaTime;
            image.fillAmount = timer / TimeToReactivate;
            yield return new WaitForEndOfFrame();
        }

        Active = true;
        if (GameManager.Instance.States == GameStates.Active) 
        {
            onReActivate?.Invoke();
            ReActivate();
        }
    }
    protected abstract void OnInactivate();

    public virtual void Clear()
    {
        _team = null;
        Active = false;
        IsClear = true;
        gameObject.SetActive(false);
    }

    private void FinishRound(Team team) => OnFinishRound(team == _team);
    protected abstract void OnFinishRound(bool win);

    protected void Move(Vector3 target, float speed)
    {
        transform.LookAt(target, Vector3.up);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
