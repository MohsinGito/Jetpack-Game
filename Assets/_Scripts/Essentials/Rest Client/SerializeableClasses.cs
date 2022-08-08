
using System.Collections.Generic;
using RestClient.Classes;

namespace RestClient.Classes
{
    [System.Serializable]
    public class RequestHeader
    {
        public string Key;

        public string Value;
    }

	public class Response
    {
        public long StatusCode { get; set; }

        public string Error { get;set; }

        public string Data { get; set; }  

        public Dictionary<string, string> Headers {get; set;} 
    }
}

namespace Utilities
{
    [System.Serializable]
    public class ApiRequest
    {
        public bool isRequestCompleted;
        public Response responseData;

        public ApiRequest()
        {
            responseData = null;
            isRequestCompleted = false;
        }
    }

    [System.Serializable]
    public class ResponseData
    {
        public string status;
        public string data;

        public ResponseData()
        {
            status = data = "N/A";
        }
    }
}