using Proyecto26;
using UnityEngine;
using Newtonsoft.Json;

public class GameServer : Singleton<GameServer>
{

    #region Main Attriutes

    [SerializeField] bool canDebug;

    [Header("-- API Calling --")]
    [SerializeField] string adUrl;
    [SerializeField] string scoresURL;

    #endregion

    #region Private Attributes

    private int scores;
    private string scoresServerURL = "https://knifegame.s3.ap-southeast-2.amazonaws.com/index.html?session=9eb84564-734a-421d-b85e-78428847a4c6";
    private AddInfo adsInfoBody;
    private ScoreInfoGS scoresInfoBody;

    #endregion

    #region Public Methods

    public void SendAPIData(int _addId, int _scores)
    {
        scores = _scores;
        adsInfoBody = new AddInfo(_addId.ToString(), "02200ff0-2473-416c-815b-404b9e0d5510");
        RestClient.Post<AddInfo>(adUrl, JsonConvert.SerializeObject(adsInfoBody), WhenAddCompleted);
    }

    private void WhenAddCompleted(RequestException exception, ResponseHelper response, AddInfo body)
    {
        PrintResponse(response);
        scoresInfoBody = new ScoreInfoGS(scores.ToString(), scoresServerURL);
        RestClient.Post<ScoreInfoGS>(scoresURL, JsonConvert.SerializeObject(scoresInfoBody), WhenScoresSentCompleted);
    }

    private void WhenScoresSentCompleted(RequestException exception, ResponseHelper response, ScoreInfoGS body)
    {
        PrintResponse(response);
    }

    private void PrintResponse(ResponseHelper response)
    {
        if (!canDebug)
            return;

        Debug.Log("<--- <b>Request Response From Server</b> --->");
        Debug.Log("<color=yellow>Status Code: " + response.StatusCode + "</color>");
        Debug.Log("<color=green>Data: " + response.Data + "</color>");
        Debug.Log("<color=red>Error: " + response.Error + "</color>");
        Debug.Log("<------------------------------------>");
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