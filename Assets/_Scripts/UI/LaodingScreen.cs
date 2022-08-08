using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityCore.Scene;
using Utilities.Audio;

public class LaodingScreen : MonoBehaviour
{

    #region Public Attributes

    [Header("-- Scene Loading Info --")]
    public float laodingTime;
    public SceneType nextScene;

    [Header("-- Scene Loading UI --")]
    public Button playButton;
    public TMP_Text loadingPercentage;
    public Image loadingBarFill;
    public GameObject loadingScreen;

    #endregion

    #region Main Methods

    private void Awake()
    {
        playButton.gameObject.SetActive(false);
        playButton.onClick.AddListener(delegate 
        { 
            SceneController.Instance.Load(nextScene);
            AudioController.Instance.PlayAudio(AudioName.UI_SFX);
        });
    }

    private void Start()
    {
        StartCoroutine(DisplayLoadingProgess());
        AudioController.Instance.PlayAudio(AudioName.MENU_BG_MUSIC);
    }

    IEnumerator DisplayLoadingProgess()
    {
        float loadProgress = 0;
        while (loadProgress < laodingTime)
        {
            loadProgress += Time.deltaTime;
            loadingBarFill.fillAmount = (loadProgress / laodingTime);
            loadingPercentage.text = (int)((loadProgress / laodingTime) * 100f) + "%";
            yield return null;
        }

        loadingScreen.SetActive(false);
        playButton.gameObject.SetActive(true);
    }

    #endregion

}
