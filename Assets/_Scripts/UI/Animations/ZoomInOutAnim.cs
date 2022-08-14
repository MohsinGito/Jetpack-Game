using DG.Tweening;
using UnityCore.Scene;
using UnityEngine;

public class ZoomInOutAnim : MonoBehaviour
{
    public float zoomFactor;
    public float speed;
    public bool isGameObject;

    private RectTransform m_Rect;
    private Vector3 originalScale;

    private void Awake()
    {
        if (!isGameObject)
        {
            m_Rect = GetComponent<RectTransform>();
            originalScale = m_Rect.localScale;
        }
        else
        {
            originalScale = transform.localScale;
        }
    }

    private void OnEnable()
    {
        ZoomIn();
    }

    private void OnDisable()
    {
        if (isGameObject)
            transform.DOKill();
        else
            m_Rect.DOKill();
    }

    private void ZoomIn()
    {
        if (isGameObject)
            transform.DOScale(originalScale + (Vector3.one * zoomFactor), speed).OnComplete(ZoomOut);
        else
            m_Rect.DOScale(originalScale + (Vector3.one * zoomFactor), speed).OnComplete(ZoomOut);
    }

    private void ZoomOut()
    {
        if (isGameObject)
            transform.DOScale(originalScale, speed).OnComplete(ZoomIn);
        else
            m_Rect.DOScale(originalScale, speed).OnComplete(ZoomIn);
    }
}
