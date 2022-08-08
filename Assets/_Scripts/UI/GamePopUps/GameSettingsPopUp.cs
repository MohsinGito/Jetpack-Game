using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Audio;
using Utilities.Data;

public class GameSettingsPopUp : GamePopUp
{

    #region Public Attributes

    [Header("Settings PopUp Elements")]
    public ZoomInOutPopUp popUpAnim;
    public Button cancelBtn;
    public Button sfxOnBtn;
    public Button sfxOffBtn;
    public Button musicOnBtn;
    public Button musicOffBtn;

    #endregion

    #region Private Attributes

    private GameData gameData;
    private UnityAction callbackEvent;

    #endregion

    #region Public Attributes

    public override void Init(GameData _gameData)
    {
        gameData = _gameData;
        DOTween.KillAll();

        InitializaVolumeButtons();
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

    private void InitializaVolumeButtons()
    {
        ChangeSFXVolume(gameData.sfxOn);
        ChangeMusicVolume(gameData.musicOn);

        cancelBtn.onClick.AddListener(Cancel);
        sfxOnBtn.onClick.AddListener(delegate { ChangeSFXVolume(true); });
        sfxOffBtn.onClick.AddListener(delegate { ChangeSFXVolume(false); });
        musicOnBtn.onClick.AddListener(delegate { ChangeMusicVolume(true); });
        musicOffBtn.onClick.AddListener(delegate { ChangeMusicVolume(false); });
    }

    private void Cancel()
    {
        callbackEvent?.Invoke();
    }

    private void ChangeSFXVolume(bool state)
    {
        gameData.sfxOn = state;
        DataController.Instance.Sfx = state ? 1 : 0;
        AudioController.Instance.PlayAudio(AudioName.UI_SFX);

        if (state)
        {
            sfxOnBtn.GetComponent<Image>().enabled = true;
            sfxOffBtn.GetComponent<Image>().enabled = false;
            AudioController.Instance.UnMuteTrack("Gameplay SFX");
            AudioController.Instance.UnMuteTrack("Win/Loos SFX");
        }
        else
        {
            sfxOnBtn.GetComponent<Image>().enabled = false;
            sfxOffBtn.GetComponent<Image>().enabled = true;
            AudioController.Instance.MuteTrack("Gameplay SFX");
            AudioController.Instance.MuteTrack("Win/Loos SFX");
        }
    }

    private void ChangeMusicVolume(bool state)
    {
        gameData.musicOn = state;
        DataController.Instance.Music = state ? 1 : 0;
        AudioController.Instance.PlayAudio(AudioName.UI_SFX);

        if (state)
        {
            musicOnBtn.GetComponent<Image>().enabled = true;
            musicOffBtn.GetComponent<Image>().enabled = false;
            AudioController.Instance.UnMuteTrack("Background Music");
        }
        else
        {
            musicOnBtn.GetComponent<Image>().enabled = false;
            musicOffBtn.GetComponent<Image>().enabled = true;
            AudioController.Instance.MuteTrack("Background Music");
        }
    }

    #endregion

}