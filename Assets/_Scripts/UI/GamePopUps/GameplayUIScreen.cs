using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameplayUIScreen : MonoBehaviour
{

    #region Main Attributes

    public ZoomInOutPopUp uiAnim;
    public TMP_Text scoresText;
    public TMP_Text livesText;
    public Button pauseButton;

    #endregion

    #region Private Attributes

    private int currentScores;
    private int scoresLeftToAdd;
    private GameData gameData;
    private GameplayPopupsManager popupsManager;

    #endregion

    #region Public Methods

    public void Init(GameData _gameData, GameplayPopupsManager _popupsManager)
    {
        gameData = _gameData;
        popupsManager = _popupsManager;

        gameData.sessionScores = 0;
        scoresText.text = gameData.sessionScores.ToString();
        pauseButton.onClick.AddListener(popupsManager.ShowPauseScreen);

        UpdateScoresUI();
    }

    public void Display()
    {
        uiAnim.Animate(true);
    }

    public void Hide()
    {
        uiAnim.Animate(false);
    }

    public void IncrementBoostScores()
    {
        scoresLeftToAdd += gameData.scoresBoost;
        gameData.sessionScores += gameData.scoresBoost;
    }

    public void IncrementCoinScores()
    {
        scoresLeftToAdd += gameData.coinsScores;
    }

    public void UpdateLivesUI(int lives)
    {
        livesText.text = "Lives Left : " + lives;
    }

    private void UpdateScoresUI()
    {
        StartCoroutine(UpdateUI());
        IEnumerator UpdateUI()
        {
            while(true)
            {
                if(scoresLeftToAdd > 0)
                {
                    currentScores += 1;
                    scoresLeftToAdd -= 1;
                    scoresText.text = currentScores.ToString();
                }
                yield return new WaitForSeconds(0.01f);  
            }
        }
    }

    #endregion

}