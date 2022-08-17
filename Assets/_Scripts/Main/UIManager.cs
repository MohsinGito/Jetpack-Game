using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Audio;
using System.Collections;
using System;
using GameControllers;
using TMPro;

public class UIManager : GameState
{

    #region Public Attributes

    public GameplayPopupsManager gameplayPopupsManager;

    [Header("-- UI Elements --")]
    public Button pauseButton;
    public TMP_Text gameCoinsText;
    public RectTransform missleIndecator;

    #endregion

    #region Private Attributes

    private float nextYPos;
    private float lockedYPos;
    private GameData gameData;
    private GameManager gameManager;
    private RectTransform mainCanvas;
    private Animator missileAnimator;

    #endregion

    #region Public Methods

    public void Init(GameData _gameData, GameManager _gameManager)
    {
        gameData = _gameData;
        gameManager = _gameManager;
        mainCanvas = GetComponent<RectTransform>();
        missileAnimator = missleIndecator.GetChild(0).GetComponent<Animator>();
        gameCoinsText.text = gameData.gameEarnedCoins + "";

        gameplayPopupsManager.Init(_gameData);
        pauseButton.onClick.AddListener(PauseButtonAction);
        gameData.CheckGameUnlockedElements();

        SetUpInitialPopUps();
    }

    public void DisplayGameplayUI()
    {
        gameCoinsText.text = gameData.sessionCoins + "";
        pauseButton.GetComponent<RectTransform>().parent.DOScale(Vector3.one, 0.25f);
        gameCoinsText.GetComponent<RectTransform>().parent.DOScale(Vector3.one, 0.25f);
    }

    public void IncrementSessionCoins()
    {
        gameData.sessionCoins += 1;
        gameData.gameEarnedCoins += 1;
        gameCoinsText.text = gameData.sessionCoins + "";
    }

    public override void OnPlayerDied()
    {
        HideIndication();
        DOVirtual.DelayedCall(1.5f, () =>
        {
            HideIndication();
            ShowGameEndPopUp();
            gameManager.EndGame();
        });
    }

    #endregion

    #region PopUps Managing Methods

    public void ShowGameEndPopUp()
    {
        gameplayPopupsManager.DisplayPopUp(PopUp.GAME_END, delegate
        {
            gameplayPopupsManager.HidePopUp(PopUp.GAME_END);
            DOVirtual.DelayedCall(0.25f, delegate { gameplayPopupsManager.DisplayPopUp(PopUp.NONE); });
            AudioController.Instance.PlayAudio(AudioName.UI_SFX);
        });

        AudioController.Instance.PlayAudio(AudioName.UI_SFX);
    }

    private void SetUpInitialPopUps()
    {
        if(gameData.restartGame)
        {
            gameManager.GameStarted();
            gameData.restartGame = false;
            gameplayPopupsManager.popUpBG.SetActive(false);
            return;
        }

        gameData.SelectRandomGameMap();
        gameplayPopupsManager.DisplayPopUp(PopUp.CHARACTER_SELECTION, delegate
        {
            gameManager.GameStarted();
            gameplayPopupsManager.HidePopUp(PopUp.CHARACTER_SELECTION);
            DOVirtual.DelayedCall(0.25f, delegate { gameplayPopupsManager.DisplayPopUp(PopUp.NONE); });
        });

        //gameplayPopupsManager.DisplayPopUp(PopUp.MAP_SELECTION, delegate
        //{
        //    gameplayPopupsManager.DisplayPopUp(PopUp.CHARACTER_SELECTION, delegate
        //    {
        //        gameManager.GameStarted();
        //        gameplayPopupsManager.HidePopUp(PopUp.CHARACTER_SELECTION);
        //        DOVirtual.DelayedCall(0.25f, delegate { gameplayPopupsManager.DisplayPopUp(PopUp.NONE); });
        //    });
        //});
    }

    private void PauseButtonAction()
    {
        Time.timeScale = 0;
        gameplayPopupsManager.DisplayPopUp(PopUp.GAME_PAUSE, delegate
        {
            Time.timeScale = 1;
            gameplayPopupsManager.HidePopUp(PopUp.GAME_PAUSE);
            DOVirtual.DelayedCall(0.25f, delegate { gameplayPopupsManager.DisplayPopUp(PopUp.NONE); });
        }, false);

        AudioController.Instance.PlayAudio(AudioName.UI_SFX);
    }

    #endregion

    #region Utilities

    public void HideIndication()
    {
        StopAllCoroutines();
        missileAnimator.CrossFade("Idle", 0);
        missleIndecator.gameObject.SetActive(false);
    }

    public void DisplayMissileIndication(float _indicationTime, Transform _relativeTransform, Action<float> callback)
    {
        StartCoroutine(StartDisplaying());
        IEnumerator StartDisplaying()
        {
            missileAnimator.CrossFade("Idle", 0);
            missleIndecator.gameObject.SetActive(true);

            while (_indicationTime > 0)
            {
                nextYPos = Mathf.Clamp(Helper.WorldToUI(mainCanvas, new Vector2(0, _relativeTransform.position.y)).y, -432f, 432f);
                missleIndecator.anchoredPosition = new Vector2(missleIndecator.anchoredPosition.x, nextYPos);

                _indicationTime -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            lockedYPos = _relativeTransform.position.y;
            missileAnimator.CrossFade("IndicationAnim", 0);

            yield return new WaitForSeconds(1f);

            missleIndecator.gameObject.SetActive(false);
            callback?.Invoke(lockedYPos);
        }
    }

    #endregion

}