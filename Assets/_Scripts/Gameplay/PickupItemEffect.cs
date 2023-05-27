using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PickupItemEffect : MonoBehaviour
{
    [Header("Fade settings")]
    public float fadeSpeed = 1f;
    public float interval = 2f;

    [Header("Rotation settings")]
    public float rotationSpeed = 1f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found!");
            return;
        }
    }

    private void OnEnable()
    {
        FadeIn();
        Rotate();
    }

    private void OnDisable()
    {
        spriteRenderer.DOKill();
        transform.DOKill();
    }

    private void FadeIn()
    {
        spriteRenderer.DOFade(0, fadeSpeed).OnComplete(() => DOVirtual.DelayedCall(interval, FadeOut));
    }

    private void FadeOut()
    {
        spriteRenderer.DOFade(1, fadeSpeed).OnComplete(() => DOVirtual.DelayedCall(interval, FadeIn));
    }

    private void Rotate()
    {
        transform.DORotate(new Vector3(0, 0, 360), rotationSpeed, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
    }
}
