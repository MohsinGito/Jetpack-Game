using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Audio;
using static GamePopUp;

public class StageSelectionScreen : GamePopUp
{

    #region Public Attributes

    [Header("Stage Selection PopUp Elements")]
    public Button startButton;
    public ZoomInOutPopUp popUpAnim;
    public RectTransform listParent;
    public GameObject uiPrefab;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private List<MapItem> itemsList;
    private UnityAction callbackEvent;

    #endregion

    #region Public Methods

    public override void Init(GameData _gameData)
    {
        gameData = _gameData;
        startButton.onClick.AddListener(delegate 
        { 
            callbackEvent?.Invoke();
            AudioController.Instance.PlayAudio(AudioName.UI_SFX);
        });

        SetUpDisplayList();
    }

    public override void Display()
    {
        popUpAnim.Animate(true);
        MapSelected(gameData.selectedMap.mapIndex);
    }

    public override void Hide()
    {
        popUpAnim.Animate(false);
    }

    public override void SetAction(UnityAction _callback)
    {
        callbackEvent = _callback;
    }

    #endregion

    #region Private Methods

    private void SetUpDisplayList()
    {
        itemsList = new List<MapItem>();

        for (int i = 0; i < gameData.gameStages.Count; i++)
        {
            gameData.gameStages[i].mapIndex = i;
            itemsList.Add(new MapItem(i, gameData.gameStages[i], MapSelected, 
                Instantiate(uiPrefab, listParent).GetComponent<RectTransform>()));
        }
    }

    private void MapSelected(int _stageIndex)
    {
        if (itemsList[_stageIndex].selectIcon.enabled)
            return;

        foreach(MapItem item in itemsList)
            item.selectIcon.enabled = false;


        itemsList[_stageIndex].selectIcon.enabled = true;
        gameData.selectedMap = gameData.gameStages[_stageIndex];
    }

    #endregion

}

public struct MapItem
{
    public RectTransform parent;
    public Image layer1;
    public Image layer2;
    public Image layer3;
    public Image selectIcon;
    public Button selectBtn;

    public MapItem(int _stageIndex, GameMap _mapInfo, ButtonEvent _onClickEvent, RectTransform _parent)
    {
        parent = _parent;

        layer1 = parent.GetChild(1).GetComponent<Image>();
        layer2 = parent.GetChild(2).GetComponent<Image>();
        layer3 = parent.GetChild(3).GetComponent<Image>();
        selectIcon = parent.GetChild(4).GetComponent<Image>();
        selectBtn = parent.GetChild(3).GetComponent<Button>();

        layer1.sprite = _mapInfo.layerOne;
        layer2.sprite = _mapInfo.layerTwo;
        layer3.sprite = _mapInfo.layerThree;
        selectBtn.onClick.AddListener(delegate { _onClickEvent(_stageIndex); });
    }
}