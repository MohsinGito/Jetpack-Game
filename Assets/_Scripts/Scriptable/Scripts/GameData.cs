using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "SO/GameData")]
public class GameData : ScriptableObject
{

    #region Main Attributes

    public bool resetGame;

    [Header("-- In Game Data --")]
    public int gameEarnedCoins;
    public float shieldDuration;
    public float magnetDuration;
    public List<GameMap> gameStages;
    public List<GameCharacter> gameCharacters;
    public List<ArrayLayout> coinsPatterns;

    [Header("-- Game Settings --")]
    public bool sfxOn;
    public bool musicOn;
    public bool gameInitialized;

    // -- PLAYER SELECTION
    [HideInInspector] public GameMap selectedMap;
    [HideInInspector] public GameCharacter selectedCharacter;
    [HideInInspector] public int sessionCoins;
    [HideInInspector] public bool restartGame;

    #endregion

    #region Main Methods

    public void CheckGameUnlockedElements()
    {

        if(resetGame)
        {
            sessionCoins = 0;
            gameEarnedCoins = 0;
            selectedMap = new GameMap();
            selectedCharacter = new GameCharacter();
        }

        for (int i = 0; i < gameCharacters.Count; i++)
            gameCharacters[i].unLocked = gameEarnedCoins >= gameCharacters[i].scoresCriteria;
    }

    public ArrayLayout GetNewCoinsPattern()
    {
        return coinsPatterns[Random.Range(0, coinsPatterns.Count)];
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