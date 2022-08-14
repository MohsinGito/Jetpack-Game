using GameControllers;
using UnityEngine;

public class MapElementManager : GameState
{

    #region Public Attributes

    [Header("-- Map Elements --")]
    public ParralexEffect groundParrallex;
    public EnemyController enemyController;
    public MissileController missileController;

    #endregion

    #region Private Attributes

    private UIManager uiManager;
    private PlayerController player;
    private EnvironmentManager envManager;

    #endregion

    #region Public Methods

    public void Init(EnvironmentManager _environmentManager, UIManager _uiManger, PlayerController _player)
    {
        player = _player;
        uiManager = _uiManger;
        envManager = _environmentManager;
    }

    public override void OnGameStart()
    {
        //groundParrallex.Init();
        enemyController.Init(player.transform, envManager);
        missileController.Init(envManager, uiManager, player.transform);
    }

    public override void OnGameEnd()
    {
        enemyController.GameEnded();
        missileController.GameEnded();
        groundParrallex.parallexSpeed = 0;
    }

    public void DestroyAllElements()
    {
        missileController.DestroyAllMissiles();
        enemyController.DestroyEnemies();
    }

    #endregion

}