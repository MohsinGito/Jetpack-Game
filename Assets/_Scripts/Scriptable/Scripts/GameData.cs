using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "SO/GameData")]
public class GameData : ScriptableObject
{

    #region Main Attributes

    public bool testBuild;
    public bool resetGame;

    [Header("-- In Game Data --")]
    public int scoresBoost;
    public int coinsScores;
    public int sessionScores;
    public int gameEarnedScores;
    public List<GameStage> gameStages;
    public List<GameCharacter> gameCharacters;
    public List<GameEnemy> gameFlyingEnemies;
    public List<GameEnemy> gameCrawlingEnemies;

    [Header("-- Player Selection --")]
    [HideInInspector] public GameStage selectedStage;
    [HideInInspector] public GameCharacter selectedCharacter;

    [Header("-- Game Settings --")]
    public bool sfxOn;
    public bool musicOn;
    public bool gameInitialized;

    [HideInInspector] public bool restartGame;

    #endregion

    #region Main Methods

    public void CheckGameUnlockedElements()
    {
        for (int i = 0; i < gameStages.Count; i++)
            gameStages[i].unLocked = gameEarnedScores >= gameStages[i].scoresCriteria;

        for (int i = 0; i < gameCharacters.Count; i++)
            gameCharacters[i].unLocked = gameEarnedScores >= gameCharacters[i].scoresCriteria;
    }

    #endregion

}

[System.Serializable]
public class GameStage
{
    public string stageName;
    public Sprite platform;
    public Sprite layerOne;
    public Sprite layerTwo;
    public Sprite layerThree;
    public Sprite groundSprite;
    public Sprite stageUISprite;
    public Color backgroundColor;
    public bool unLocked;
    public int scoresCriteria;
}

[System.Serializable]
public class GameCharacter
{
    public string characterName;
    public Sprite idleSprite;
    public Sprite jumpSprite;
    public Sprite dieSprite;
    public bool unLocked;
    public int scoresCriteria;
}

[System.Serializable]
public struct GameEnemy
{
    public string enemyName;
    public List<Sprite> animationSprites;
}