using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityCore.Scene;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class ZoomInOutPopUp : ZoomBehaviour
{

    #region Main Attributes

    [Header("-- PopUp Animation -- ")]
    [SerializeField] float animationSpeed;
    [SerializeField] List<float> animScaleFactors;

    Vector3 originalSize;
    RectTransform m_RectTransfrom;
    CanvasGroup m_CanvasGroup;
    Coroutine animCoroutine;

    #endregion

    #region Main Methods

    private void Awake()
    {
        originalSize = Vector3.one;
        m_RectTransfrom = GetComponent<RectTransform>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        SceneController.OnSceneChange += ResetPopUp;

        ResetPopUp();
    }

    public void Animate(bool _state)
    {
        if(_state)
        {
            DisplayPopUp();
            DisplayComponents();
        }
        else
        {
            HidePopUp();
            HideComponents();
        }
    }

    private void DisplayPopUp()
    {
        StopPopUpAnimation();

        m_RectTransfrom.localScale = Vector3.zero;
        animCoroutine = StartCoroutine(Animate());

        IEnumerator Animate()
        {
            int factorIndex = 0;
            float animSpeed = animationSpeed;
            Vector3 animSize = Vector3.zero;

            if(animScaleFactors.Count == 0)
            {
                m_RectTransfrom.localScale = Vector3.one;
                yield return 0;
            }

            m_CanvasGroup.DOFade(1, animationSpeed);
            for (int i = 0; i < animScaleFactors.Count * 2 && m_RectTransfrom != null; i++)
            {
                if ((i + 1) % 2 == 0)
                {
                    animSpeed -= (animationSpeed * 0.1f);
                    animSize = originalSize;
                }
                else
                {
                    animSpeed -= (animationSpeed * 0.1f);
                    animSize = originalSize * animScaleFactors[factorIndex++];
                }

                if (i == 0) { m_RectTransfrom.localScale = animSize; continue; }
                yield return m_RectTransfrom.DOScale(animSize, animSpeed).WaitForCompletion();
            }
        }
    }

    private void HidePopUp()
    {
        StopPopUpAnimation();
        m_CanvasGroup.DOFade(0, animationSpeed);
        m_RectTransfrom.DOScale(originalSize * animScaleFactors[0], animationSpeed);
    }

    private void StopPopUpAnimation()
    {
        if (animCoroutine != null)
            StopCoroutine(animCoroutine);

        m_CanvasGroup.DOKill();
        m_RectTransfrom.DOKill();
    }

    public void ResetPopUp()
    {
        StopPopUpAnimation();
        StopComponentsAnimation();
        DOTween.KillAll();
    }

    private void OnDisable()
    {
        SceneController.OnSceneChange -= ResetPopUp;
    }

    #endregion

}
