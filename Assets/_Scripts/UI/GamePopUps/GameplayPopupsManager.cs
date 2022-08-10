using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameplayPopupsManager : MonoBehaviour
{

    #region Main Attributes

    public GameObject popUpBG;
    public List<GamePopUpInfo> gamePopUps;

    private GameData gameData;
    private Dictionary<PopUp, GamePopUp> gamePopUpInfos;

    #endregion

    #region Main Methods

    public void Init(GameData _gameData)
    {
        gameData = _gameData;

        gamePopUpInfos = new Dictionary<PopUp, GamePopUp>();
        foreach (GamePopUpInfo popUp in gamePopUps)
        {
            gamePopUpInfos[popUp.type] = popUp.script;
            gamePopUpInfos[popUp.type].Init(gameData);
        }

        DisplayPopUp(PopUp.NONE);
    }

    public void DisplayPopUp(PopUp _popUpType, UnityAction _callback = null, bool _enableBg = true)
    {
        foreach (KeyValuePair<PopUp, GamePopUp> popUp in gamePopUpInfos)
            popUp.Value.gameObject.SetActive(false);

        if (_popUpType == PopUp.NONE)
            return;

        popUpBG.SetActive(_enableBg);
        gamePopUpInfos[_popUpType].gameObject.SetActive(true);
        gamePopUpInfos[_popUpType].SetAction(_callback);
        gamePopUpInfos[_popUpType].Display();
    }

    public void HidePopUp(PopUp _popUpType, bool _enableBg = false)
    {
        if (_popUpType == PopUp.NONE)
            return;

        popUpBG.SetActive(_enableBg);
        gamePopUpInfos[_popUpType].Hide();
    }

    #endregion

}

public enum PopUp
{
    NONE,
    MAP_SELECTION,
    CHARACTER_SELECTION,
    GAME_END,
    SETTINGS,
    GAME_PAUSE
}

[System.Serializable]
public class GamePopUpInfo
{
    public PopUp type;
    public GamePopUp script;
}