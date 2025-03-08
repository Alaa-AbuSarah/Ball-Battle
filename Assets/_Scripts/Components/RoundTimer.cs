using System.Collections;
using TMPro;
using UnityEngine;

public class RoundTimer : Singleton<RoundTimer>
{
    [SerializeField] private int _time = 0;
    [SerializeField] private TMP_Text text = null;

    public void StartTimer(int time)
    {
        StopCoroutine(TimeDown());
        _time = time;
        StartCoroutine(TimeDown());
    }

    public void Stop() => StopCoroutine(TimeDown());

    private IEnumerator TimeDown()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        text.text = _time.ToString();

        while (_time > 0 && GameManager.Instance.States == GameStates.Active)
        {
            yield return wait;
            _time--;
            text.text = _time.ToString();
        }

        if (GameManager.Instance.States == GameStates.Active)
            GameManager.Instance.Timeout();
    }
}
