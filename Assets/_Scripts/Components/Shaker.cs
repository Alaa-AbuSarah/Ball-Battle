using DG.Tweening;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField]private float duration = 0.25f;
    [SerializeField]private float strength = 5f;
    [SerializeField] private Ease ease = Ease.OutBack;

    public void Shake() => transform.DOShakePosition(duration, strength).SetEase(ease);
}
