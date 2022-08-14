using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Audio;

public class GameManager : MonoBehaviour
{

    #region Public Attributes

    public GameData gameData;

    [Header("-- Main Scripts --")]
    public PlayerController playerController;
    public EnvironmentManager environmentManager;
    public MapElementManager mapElementManager;
    public UIManager uiManager;

    #endregion

    #region Main Methods

    private void Start()
    {
        gameData.sessionCoins = 0;
        uiManager.Init(gameData, this);
    }

    public void GameStarted()
    {
        environmentManager.Init(gameData, mapElementManager);
        playerController.Init(environmentManager, gameData.selectedCharacter.controller, uiManager);
        mapElementManager.Init(environmentManager, uiManager, playerController);
        AudioController.Instance.PlayAudio(AudioName.GAMEPLAY_BG_MUSIC);

        GameSession.CacheScripts();
        GameSession.StartGame();
    }

    #endregion

}