using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Audio;

public class CharacterSelectionPopUp : GamePopUp
{
    #region Public Attributes

    [Header("Puase PopUp Elements")]
    public ZoomInOutPopUp popUpAnim;
    public Image displayImage;
    public TMP_Text displayName;
    public TMP_Text infoText;
    public Button leftArrowButton;
    public Button rightArrowButton;
    public Button selectCharacterButton;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private UnityAction callbackEvent;
    private int currentCharacterIndex;

    #endregion

    #region Public Methods

    public override void Init(GameData _gameData)
    {
        gameData = _gameData;
        currentCharacterIndex = 0;

        leftArrowButton.onClick.AddListener(MoveToLeft);
        rightArrowButton.onClick.AddListener(MoveToRight);
        selectCharacterButton.onClick.AddListener(SelectCharacter);

        DOTween.KillAll();
        SetUpCharacter();
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

    private void MoveToLeft()
    {
        if (currentCharacterIndex == 0)
            return;

        AudioController.Instance.PlayAudio(AudioName.UI_SFX);
        currentCharacterIndex -= 1;
        SetUpCharacter();
    }

    private void MoveToRight()
    {
        if (currentCharacterIndex == gameData.gameCharacters.Count - 1)
            return;

        AudioController.Instance.PlayAudio(AudioName.UI_SFX);
        currentCharacterIndex += 1;
        SetUpCharacter();
    }

    private void SetUpCharacter()
    {
        displayName.text = gameData.gameCharacters[currentCharacterIndex].characterName;
        displayImage.sprite = gameData.gameCharacters[currentCharacterIndex].idleSprite;
        displayImage.color = gameData.gameCharacters[currentCharacterIndex].unLocked ? Color.white : Color.black;
        selectCharacterButton.interactable = gameData.gameCharacters[currentCharacterIndex].unLocked;
        infoText.gameObject.SetActive(!gameData.gameCharacters[currentCharacterIndex].unLocked);
        infoText.text = "Reach Up To " + gameData.gameCharacters[currentCharacterIndex].scoresCriteria + " Scores To Unlock This Character!";
    }

    private void SelectCharacter()
    {
        AudioController.Instance.PlayAudio(AudioName.UI_SFX);
        gameData.selectedCharacter = gameData.gameCharacters[currentCharacterIndex];
        callbackEvent?.Invoke();
    }

    #endregion
}
