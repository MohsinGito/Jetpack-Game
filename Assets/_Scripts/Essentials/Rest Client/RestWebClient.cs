using System;
using System.Collections;
using System.Collections.Generic;
using RestClient.Classes;
using UnityEngine;
using UnityEngine.Networking;
 
namespace RestClient
{
    public class RestWebClient : Singleton<RestWebClient>
    {
        private const string defaultContentType = "application/json";
        public IEnumerator HttpGet(string url, System.Action<Response, int> callback, int apiId = -1)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                    }, apiId);
                }

                if (webRequest.isDone)
                {
                    string data = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    Debug.Log("Data: " + data);
                    callback(new Response
                    {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        Data = data
                    }, apiId);
                }

                DisposeWebrequest(webRequest);
            }
        }

        public IEnumerator HttpDelete(string url, System.Action<Response> callback)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Delete(url))
            {
                yield return webRequest.SendWebRequest();

                if(webRequest.isNetworkError){
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error
                    });
                }
                
                if(webRequest.isDone)
                {
                    callback(new Response {
                        StatusCode = webRequest.responseCode
                    });
                }

                DisposeWebrequest(webRequest);
            }
        }

        public IEnumerator HttpPost(string url, string body, System.Action<Response,int> callback, IEnumerable<RequestHeader> headers = null, int apiId = -1)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Post(url, body))
            {
                if(headers != null)
                {
                    foreach (RequestHeader header in headers)
                    {
                        webRequest.SetRequestHeader(header.Key, header.Value);
                    }
                }

                webRequest.uploadHandler.contentType = defaultContentType;
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));

                yield return webRequest.SendWebRequest();

                if(webRequest.isNetworkError)
                {
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error
                    }, apiId);
                }
                
                if(webRequest.isDone)
                {
                    string data = string.Empty;
                    long statusCode = webRequest.responseCode;
                    string error = webRequest.error;

                    if (webRequest.downloadHandler.data == null)
                    {
                        data = "No Data Found";
                    }
                    else
                    {
                        data = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    }

                    DisposeWebrequest(webRequest);
                    callback(new Response
                    {
                        StatusCode = statusCode,
                        Error = error,
                        Data = data
                    }, apiId);
                }
            }
        }

        public IEnumerator HttpPut(string url, string body, System.Action<Response> callback, IEnumerable<RequestHeader> headers = null)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Put(url, body))
            {
                if(headers != null)
                {
                    foreach (RequestHeader header in headers)
                    {
                        webRequest.SetRequestHeader(header.Key, header.Value);
                    }
                }

                webRequest.uploadHandler.contentType = defaultContentType;
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));

                yield return webRequest.SendWebRequest();

                if(webRequest.isNetworkError)
                {
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                    });
                }
                
                if(webRequest.isDone)
                {
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                    });
                }

                DisposeWebrequest(webRequest);
            }
        }

        public IEnumerator HttpHead(string url, System.Action<Response> callback)
        {
            using(UnityWebRequest webRequest = UnityWebRequest.Head(url))
            {
                yield return webRequest.SendWebRequest();
                
                if(webRequest.isNetworkError){
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                    });
                }
                
                if(webRequest.isDone)
                {
                    var responseHeaders = webRequest.GetResponseHeaders();
                    callback(new Response {
                        StatusCode = webRequest.responseCode,
                        Error = webRequest.error,
                        Headers = responseHeaders
                    });
                }

                DisposeWebrequest(webRequest);
            }
        }

        private void DisposeWebrequest(UnityWebRequest _webRequest)
        {
            _webRequest.disposeUploadHandlerOnDispose = true;
            _webRequest.disposeDownloadHandlerOnDispose = true;
            _webRequest.disposeCertificateHandlerOnDispose = true;

            _webRequest.Abort();
            _webRequest.Dispose();
        }
    }
}