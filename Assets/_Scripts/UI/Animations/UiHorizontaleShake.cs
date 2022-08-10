using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityCore.Scene;
using UnityEngine;

public class UiHorizontaleShake : MonoBehaviour
{

    #region Public Attributes

    [SerializeField] float shakeForce;
    [SerializeField] float shakeSpeed;
    [SerializeField] float shakeDuration;
    [SerializeField] RectTransform uiRect;
    [SerializeField] bool shakeOnEnable;
    [SerializeField] bool endlessShake;

    private bool canShake;
    private Vector2 newPos;

    #endregion

    #region Main Methods

    private void OnEnable()
    {
        if(shakeOnEnable)
            Shake();

        SceneController.OnSceneChange += ResetShakeAnim;
    }

    public void Shake()
    {
        StartCoroutine(ShakeTimer());
        IEnumerator ShakeTimer()
        {
            canShake = true;
            yield return new WaitForSeconds(shakeDuration);
            canShake = endlessShake;
        }

        ShakeLeft();
    }

    private void ShakeLeft()
    {
        if (!canShake)
            return;

        newPos = new Vector2(uiRect.anchoredPosition.x + shakeForce, uiRect.anchoredPosition.y);
        uiRect.DOAnchorPos(newPos, shakeSpeed).OnComplete(ShakeRight);
    }

    private void ShakeRight()
    {
        if (!canShake)
            return;

        newPos = new Vector2(uiRect.anchoredPosition.x - shakeForce, uiRect.anchoredPosition.y);
        uiRect.DOAnchorPos(newPos, shakeSpeed).OnComplete(ShakeLeft);
    }

    private void ResetShakeAnim()
    {
        uiRect.DOKill();
        SceneController.OnSceneChange -= ResetShakeAnim;
    }

    #endregion

}
