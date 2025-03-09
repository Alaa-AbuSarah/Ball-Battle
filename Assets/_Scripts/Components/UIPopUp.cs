using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIPopUp : MonoBehaviour
{
    [SerializeField] private AudioClip clip = null;

    [SerializeField] private Vector2 startAnchor = Vector2.zero;
    [SerializeField] private Vector2 endAnchor = Vector2.zero;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private Ease ease = Ease.OutBack;

    private RectTransform rect = null;
    private void OnEnable()
    {
        if (rect is null)
            rect = GetComponent<RectTransform>();

        rect.anchoredPosition = startAnchor;
        rect.DOAnchorPos(endAnchor, duration).SetEase(ease);

        if (clip != null)
            AudioManager.Instance.PlaySFX(clip, 0.75f);
    }
}
