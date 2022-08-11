using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Audio;

public class UIManager : MonoBehaviour
{

    #region Public Attributes

    public GameplayPopupsManager gameplayPopupsManager;

    [Header("-- UI Elements --")]
    public Button pauseButton;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private GameManager gameManager;

    #endregion

    #region Public Methods

    public void Init(GameData _gameData, GameManager _gameManager)
    {
        gameData = _gameData;
        gameManager = _gameManager;

        gameplayPopupsManager.Init(_gameData);
        pauseButton.onClick.AddListener(PauseButtonAction);

        SetUpInitialPopUps();
    }

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

    #endregion

    #region Private Methods

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

}