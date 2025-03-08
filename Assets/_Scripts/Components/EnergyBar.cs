using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyBar : MonoBehaviour
{
    public string Title { get => text.text; set => text.text = value; }
    public int Value = 0;

    [Space]

    [SerializeField] private TMP_Text text = null;
    [SerializeField] private Slider slider = null;
    [SerializeField] private float regenerationTime = 0.5f;

    public void Activate()
    {
        StopCoroutine(Generae());
        Value = 0;
        StartCoroutine(Generae());
    }

    public void Stop() => StopCoroutine(Generae());
    private IEnumerator Generae()
    {
        WaitForSeconds wait = new WaitForSeconds(regenerationTime);

        while (GameManager.Instance.States == GameStates.Active)
        {
            yield return wait;
            if (Value < 6)
                Value++;
            slider.value = Value;
        }
    }
}
