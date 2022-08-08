using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Audio;
using static GamePopUp;

public class StageSelectionScreen : GamePopUp
{

    #region Public Attributes

    [Header("Stage Selection PopUp Elements")]
    public ZoomInOutPopUp popUpAnim;
    public RectTransform listParent;
    public GameObject uiPrefab;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private List<StageItem> itemsList;
    private UnityAction callbackEvent;

    #endregion

    #region Public Methods

    public override void Init(GameData _gameData)
    {
        gameData = _gameData;

        DOTween.KillAll();
        SetUpDisplayList();
    }

    public override void Display()
    {
        popUpAnim.Animate(true);
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
        itemsList = new List<StageItem>();

        for (int i = 0; i < gameData.gameStages.Count; i++)
        {
            itemsList.Add(new StageItem(Instantiate(uiPrefab, listParent).GetComponent<RectTransform>()));
            itemsList[i].SetUpStage(i, gameData.gameStages[i], StageSelected);
        }
    }

    private void StageSelected(int _stageIndex)
    {
        AudioController.Instance.PlayAudio(AudioName.UI_SFX);
        gameData.selectedStage = gameData.gameStages[_stageIndex];
        callbackEvent?.Invoke();
    }

    #endregion

}

public struct StageItem
{
    public RectTransform parent;
    public Image displayImage;
    public TMP_Text displayText;
    public TMP_Text infoText;
    public Button onClickBtn;

    public StageItem(RectTransform _parent)
    {
        parent = _parent;

        displayImage = parent.GetChild(0).GetComponent<Image>();
        displayText = parent.GetChild(1).GetComponent<TMP_Text>();
        infoText = parent.GetChild(2).GetComponent<TMP_Text>();
        onClickBtn = parent.GetChild(3).GetComponent<Button>();
    }

    public void SetUpStage(int _stageIndex, GameStage _stageInfo, ButtonEvent _onClickEvent)
    {
        displayText.text = _stageInfo.stageName;
        displayImage.sprite = _stageInfo.stageUISprite;
        onClickBtn.interactable = _stageInfo.unLocked;
        infoText.gameObject.SetActive(!_stageInfo.unLocked);
        onClickBtn.onClick.AddListener(delegate { _onClickEvent(_stageIndex); });
        infoText.text = "Reach Up To " + _stageInfo.scoresCriteria + " Scores To Unlock This Stage!";
    }

}