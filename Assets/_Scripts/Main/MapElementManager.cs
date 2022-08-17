using GameControllers;
using UnityEngine;

public class MapElementManager : GameState
{

    #region Public Attributes

    [Header("-- Map Elements --")]
    public MapBackground backGroundParrallex;
    public EnemyController enemyController;
    public MissileController missileController;

    #endregion

    #region Private Attributes

    private UIManager uiManager;
    private PlayerController player;
    private EnvironmentManager envManager;

    #endregion

    #region Public Methods

    public void Init(EnvironmentManager _environmentManager, UIManager _uiManger, PlayerController _player, GameMap _mapData)
    {
        player = _player;
        uiManager = _uiManger;
        envManager = _environmentManager;

        backGroundParrallex.Init(envManager, _mapData);
        enemyController.Init(player.transform, envManager);
        missileController.Init(envManager, uiManager, player.transform);
    }

    public override void OnGameEnd()
    {
        enemyController.GameEnded();
        missileController.GameEnded();
    }

    public void DestroyAllElements()
    {
        missileController.DestroyAllMissiles();
        enemyController.DestroyEnemies();
    }

    #endregion

}