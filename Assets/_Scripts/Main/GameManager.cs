using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Audio;
using Utilities.Data;

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
        gameData.gameEarnedCoins = DataController.Instance.Coins;

        uiManager.Init(gameData, this);
    }

    public void GameStarted()
    {
        environmentManager.Init(gameData, mapElementManager);
        playerController.Init(environmentManager, gameData.selectedCharacter.controller, uiManager);
        mapElementManager.Init(environmentManager, uiManager, playerController, gameData.selectedMap);
        AudioController.Instance.PlayAudio(AudioName.GAMEPLAY_BG_MUSIC);

        GameSession.CacheScripts();
        GameSession.StartGame();
    }

    public void EndGame()
    {
        GameSession.EndGame();
        GameServer.Instance.SendAPIData(1, gameData.sessionCoins);
        DataController.Instance.Coins = gameData.gameEarnedCoins;
    }

    #endregion

}