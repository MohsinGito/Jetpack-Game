using DG.Tweening;
using UnityEngine;
using Utilities.Audio;
using System.Collections.Generic;

public class GameplayPopupsManager : MonoBehaviour
{

    #region Public Attributes

    public GameObject BG;

    [Header("Game PopUps")]
    public GamePopUp stageSelectionPopUp;
    public GamePopUp caharacterSelectionPopUp;
    public GamePopUp gameEndPopUp;
    public GamePopUp settingsPopUp;
    public GamePopUp gamePausePopUp;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private List<GamePopUp> gamePopUpList;

    #endregion

    #region Initializing Methods

    public void Init(GameData _gameData)
    {
        gameData = _gameData;

        stageSelectionPopUp.Init(gameData);
        caharacterSelectionPopUp.Init(gameData);
        gameEndPopUp.Init(gameData);
        settingsPopUp.Init(gameData);
        gamePausePopUp.Init(gameData);

        gamePopUpList = new List<GamePopUp>();
        gamePopUpList.Add(stageSelectionPopUp);
        gamePopUpList.Add(caharacterSelectionPopUp);
        gamePopUpList.Add(gameEndPopUp);
        gamePopUpList.Add(settingsPopUp);
        gamePopUpList.Add(gamePausePopUp);
    }

    #endregion

    #region UI PopUps Displaying

    public void ShowStageSelectionScreen()
    {
        EnablePopUp(stageSelectionPopUp);
        stageSelectionPopUp.SetAction(ShowCharacterSelectionScreen);
    }

    public void ShowCharacterSelectionScreen()
    {
        EnablePopUp(caharacterSelectionPopUp);
        caharacterSelectionPopUp.SetAction( () =>
        {
            caharacterSelectionPopUp.Hide();
            DOVirtual.DelayedCall(0.25f, delegate { EnablePopUp(null); });
        });
    }

    public void ShowSettingsScreen()
    {
        EnablePopUp(settingsPopUp);
        settingsPopUp.SetAction(() =>
        {
            settingsPopUp.Hide();
            DOVirtual.DelayedCall(0.25f, delegate { EnablePopUp(null); });
            AudioController.Instance.PlayAudio(AudioName.UI_SFX);
        });
        AudioController.Instance.PlayAudio(AudioName.UI_SFX);
    }

    public void ShowPauseScreen()
    {
        EnablePopUp(gamePausePopUp);
        gamePausePopUp.SetAction(() =>
        {
            gamePausePopUp.Hide();
            DOVirtual.DelayedCall(0.25f, delegate { EnablePopUp(null); });
            AudioController.Instance.PlayAudio(AudioName.UI_SFX);
        });
        AudioController.Instance.PlayAudio(AudioName.UI_SFX);
    }

    public void ShowGameEndingScreen()
    {
        EnablePopUp(gameEndPopUp);
        gameEndPopUp.SetAction(() =>
        {
            gameEndPopUp.Hide();
            DOVirtual.DelayedCall(0.25f, delegate { EnablePopUp(null); });
        });
        AudioController.Instance.PlayAudio(AudioName.WIN_SFX);
    }

    #endregion

    #region Private Methods

    private void EnablePopUp(GamePopUp _popUpToEnable)
    {
        if(_popUpToEnable == null)
        {
            foreach (GamePopUp popUp in gamePopUpList)
                popUp.gameObject.SetActive(false);

            return;
        }

        foreach(GamePopUp popUp in gamePopUpList)
        {
            if (popUp.name == _popUpToEnable.name)
            {
                popUp.gameObject.SetActive(true);
                popUp.Display();
            }
            else
            {
                popUp.gameObject.SetActive(false);
            }
        }
    }

    #endregion

}