using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Audio;

public class UIManager : MonoBehaviour
{

    #region Public Attributes

    public Transform dummy;
    public GameplayPopupsManager gameplayPopupsManager;

    [Header("-- UI Elements --")]
    public Button pauseButton;
    public RectTransform missleIndecator;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private GameManager gameManager;
    private RectTransform mainCanvas;

    #endregion

    #region Public Methods

    public void Init(GameData _gameData, GameManager _gameManager)
    {
        gameData = _gameData;
        gameManager = _gameManager;
        mainCanvas = GetComponent<RectTransform>();

        gameplayPopupsManager.Init(_gameData);
        pauseButton.onClick.AddListener(PauseButtonAction);

        SetUpInitialPopUps();
    }

    private void Update()
    {
        MissleIndication(dummy, true);
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
        gameplayPopupsManager.DisplayPopUp(PopUp.MAP_SELECTION, delegate
        {
            gameplayPopupsManager.DisplayPopUp(PopUp.CHARACTER_SELECTION, delegate
            {
                gameManager.GameStarted();
                gameplayPopupsManager.HidePopUp(PopUp.CHARACTER_SELECTION);
                DOVirtual.DelayedCall(0.25f, delegate { gameplayPopupsManager.DisplayPopUp(PopUp.NONE); });
            });
        });
    }

    private void PauseButtonAction()
    {
        gameplayPopupsManager.DisplayPopUp(PopUp.GAME_PAUSE, delegate
        {
            gameplayPopupsManager.HidePopUp(PopUp.GAME_PAUSE);
            DOVirtual.DelayedCall(0.25f, delegate { gameplayPopupsManager.DisplayPopUp(PopUp.NONE); });
        });

        AudioController.Instance.PlayAudio(AudioName.UI_SFX);
    }

    #endregion

    #region Utilities

    public void MissleIndication(Transform pos, bool visibility)
    {
        missleIndecator.gameObject.SetActive(visibility);
        missleIndecator.anchoredPosition = new Vector2(missleIndecator.anchoredPosition.x,
            Helper.WorldToUI(mainCanvas, pos.position).y);
    }

    #endregion

}