using System.Collections.Generic;
using Newtonsoft.Json;
using RestClient;
using RestClient.Classes;
using UnityEngine;
using Utilities;

namespace RestManager
{
    public class APIManager : Singleton<APIManager>
    {

        #region Main Attributes

        public bool canDebug;

        [Header("Base Information")]
        [SerializeField] List<RequestHeader> headers;
        [SerializeField] List<string> apiCalls;

        private List<ApiRequest> apiResponses = new List<ApiRequest>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Creating A Post Request On Rest API Client And Adding
        /// That Call To The Data List So That Plyaer Can Track That Call
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <param name="call"></param>
        /// <returns></returns>
        public int Post<T>(T body, API call)
        {
            apiResponses.ClearCache();
            int newAPIId = apiResponses.Count;  /* -- MAKING A NEW API REQUEST WITH NEW API ID -- */

            /* -- SENDING A POST REQUEST TO REST CLIENT -- */
            StartCoroutine(RestWebClient.Instance.HttpPost(apiCalls[(int)call],
            JsonConvert.SerializeObject(body), (r, p) => OnRequestComplete(r, p), headers, newAPIId));

            /* -- ADDING REQUEST INFO TO THE API-RESPONSES LIST -- */
            apiResponses.Add(new ApiRequest());
            return newAPIId;    /* -- SENDING REQUEST ID BACK TO USER SO THAT HE/SHE CAN TRACK THE REQUEST -- */
        }

        public int Get(API call)
        {
            int newAPIId = apiResponses.Count;  /* -- MAKING A NEW API REQUEST WITH NEW API ID -- */

            /* -- SENDING A GET REQUEST TO REST CLIENT -- */
            StartCoroutine(RestWebClient.Instance.HttpGet(apiCalls[(int)call],
             (r, q) => OnRequestComplete(r, q), newAPIId));
            
            return newAPIId;    /* -- SENDING REQUEST ID BACK TO USER SO THAT HE/SHE CAN TRACK THE REQUEST -- */
        }

        public Response GetResponse(int apiId)
        {
            PrintResponse(apiResponses[apiId].responseData);
            return apiResponses[apiId].responseData;    /* -- SEDNING RESPONSE TO USER OF ANY API REQUEST -- */
        }

        public T ValidateResponseData<T>(Response reseponse)
        {
            if (string.IsNullOrEmpty(reseponse.Error))  /* -- CHECKING IF THERE IS ANY ERROR OR NOT -- */
            {
                Debug.Log("<color=green>Request Completed Successfully</color>");
                return JsonConvert.DeserializeObject<T>(reseponse.Data);
            }
            else
            {
                Debug.Log("<color=red>Request Failed Due To Error : [" + reseponse.Error + "]</color>");
                return default(T);
            }
        }

        public bool RequestCompleted(int apiId)
        {
            return apiResponses[apiId].isRequestCompleted;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// On Request Completed We Need To Insert Response Data To The List
        /// So That Player Can Access It Later
        /// </summary>
        /// <param name="response"></param>
        /// <param name="apiId"></param>

        private void OnRequestComplete(Response response, int apiId = -1)
        {
            apiResponses[apiId].isRequestCompleted = true;
            apiResponses[apiId].responseData = response;
        }

        private void PrintResponse(Response response)
        {
            if (canDebug)
            {
                Debug.Log("<--- <b>Request Response From Server</b> --->");
                Debug.Log("<color=yellow>Status Code: " + response.StatusCode + "</color>");
                Debug.Log("<color=green>Data: " + response.Data + "</color>");
                Debug.Log("<color=red>Error: " + response.Error + "</color>");
                Debug.Log("<------------------------------------>");
            }
        }

        private void OnDisable()
        {
            apiResponses.Clear();
        }

        #endregion

    }
}

public enum API
{
    SCORES_URL,
    ADD_PLAY_URL
}