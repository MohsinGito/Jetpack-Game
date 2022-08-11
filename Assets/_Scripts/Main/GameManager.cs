using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Public Attributes

    public GameData gameData;

    [Header("-- Main Scripts --")]
    public PlayerController playerController;
    public EnvironmentManager environmentManager;
    public UIManager uiManager;

    #endregion

    #region Main Methods

    private void Start()
    {
        uiManager.Init(gameData, this);
        environmentManager.Init(gameData);
        playerController.Init(gameData.selectedCharacter.controller);
    }

    public void GameStarted()
    {
        GameSession.StartGame();
    }

    #endregion

}