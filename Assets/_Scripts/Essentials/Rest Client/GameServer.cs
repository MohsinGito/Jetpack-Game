using System.Collections;
using RestManager;
using UnityEngine;

public class GameServer : Singleton<GameServer>
{

    #region Main Attriutes

    [Header("-- API Calling --")]
    [SerializeField] bool initilizeAd;
    [SerializeField] bool sendScoresInfo;

    [Header("-- For Testing API --")]
    [SerializeField] bool testSend;

    #endregion

    #region Private Attributes

    private string site;
    private string testURL = "https://knifegame.s3.ap-southeast-2.amazonaws.com/index.html?session=9eb84564-734a-421d-b85e-78428847a4c6";
    private AddInfo adsInfoBody;
    private ScoreInfoGS scoresInfoBody;

    #endregion

    #region Public Methods

    public void InitializeAdd(int _addId)
    {
        if (!initilizeAd) { return; }

        StartCoroutine(StartInitializingAdd());
        IEnumerator StartInitializingAdd()
        {
            if(testSend)
            {
                adsInfoBody = new AddInfo(_addId.ToString(), "02200ff0-2473-416c-815b-404b9e0d5510");
            }
            else
            {
                var parameters = URLParameters.GetSearchParameters();
                if (parameters.TryGetValue("session", out site))
                {
                    Debug.Log(site);
                    adsInfoBody = new AddInfo(_addId.ToString(), site);
                }
                else
                {
                    Debug.Log("No Parameters");
                }
            }

            int index = APIManager.Instance.Post<AddInfo>(adsInfoBody, API.ADD_PLAY_URL);
            yield return new WaitUntil(() => APIManager.Instance.RequestCompleted(index));
            APIManager.Instance.GetResponse(index);
        }
    }

    public void SendScoresInfo(int _scores)
    {
        if (!sendScoresInfo) { return; }

        StartCoroutine(StartSendingScoresInfo());
        IEnumerator StartSendingScoresInfo()
        {
            if (testSend)
            {
                scoresInfoBody = new ScoreInfoGS(_scores.ToString(), testURL);
            }
            else
            {
                var parameters = URLParameters.GetSearchParameters();
                if (parameters.TryGetValue("session", out site))
                {
                    Debug.Log(site);
                    scoresInfoBody = new ScoreInfoGS(_scores.ToString(), site);
                }
                else
                {
                    Debug.Log("No Parameters");
                }
            }

            int index = APIManager.Instance.Post<ScoreInfoGS>(scoresInfoBody, API.SCORES_URL);
            yield return new WaitUntil(() => APIManager.Instance.RequestCompleted(index));
            APIManager.Instance.GetResponse(index);
        }
    }

    #endregion
}

[System.Serializable]
public class ScoreInfoGS
{
    public string game_session;
    public string score;

    public ScoreInfoGS(string _sc, string _gs)
    {
        score = _sc;
        game_session = _gs;
    }
}


[System.Serializable]
public class AddInfo
{
    public string game_session;
    public string ad_id;

    public AddInfo(string _sc, string _gs)
    {
        ad_id = _sc;
        game_session = _gs;

    }
}