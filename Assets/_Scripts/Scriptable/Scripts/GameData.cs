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
    public List<GameMap> gameStages;
    public List<GameCharacter> gameCharacters;

    [Header("-- Player Selection --")]
    [HideInInspector] public GameMap selectedMap;
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
        for (int i = 0; i < gameCharacters.Count; i++)
            gameCharacters[i].unLocked = gameEarnedScores >= gameCharacters[i].scoresCriteria;
    }

    #endregion

}

[System.Serializable]
public class GameMap
{
    public string stageName;
    public Sprite layerOne;
    public Sprite layerTwo;
    public Sprite layerThree;
    
    [HideInInspector] 
    public int mapIndex;
}

[System.Serializable]
public class GameCharacter
{
    public string characterName;
    public Sprite uiSprite;
    public RuntimeAnimatorController controller;
    public bool unLocked;
    public int scoresCriteria;

    [HideInInspector]
    public int characterIndex;
}