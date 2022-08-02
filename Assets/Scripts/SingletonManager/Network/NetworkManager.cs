using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    #region Singelton
    private static NetworkManager _instance;
    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NetworkManager>();
                if (FindObjectsOfType<NetworkManager>().Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong " +
                        " - there should never be more than 1 singleton!" +
                        " Reopening the scene might fix it.");
                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject go = new GameObject("NetworkManager");
                    _instance = go.AddComponent<NetworkManager>();
                }
            }

            return _instance;
        }
    }
    #endregion


    void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }

#region �⺻ APIƲ
    private const string _baseUrl = "http://ec2-3-38-74-157.ap-northeast-2.compute.amazonaws.com:8080";
    private IEnumerator API_Post<Request>(string path , Request request)
    {
        string json = JsonUtility.ToJson(request);
        Debug.Log(json);
        using (UnityWebRequest www = UnityWebRequest.Post(_baseUrl+path, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if(www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else if(www.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }

    }
    private IEnumerator API_Post<Request,Response>(string path, Request request, Action<Response> callback)
    {
        Debug.Log("API_Post");
        string json = JsonUtility.ToJson(request);
        Debug.Log(json);
        using (UnityWebRequest www = UnityWebRequest.Post(_baseUrl + path, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else if (www.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Response res = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                callback(res);
                string tempjson = JsonUtility.ToJson(res);
                Debug.Log(tempjson);
            }
            www.Dispose();
        }

    }
    private IEnumerator API_PostWithToken<Request, Response>(string path, Request request, Action<Response> callback)
    {
        Debug.Log("API_Post");
        string json = JsonUtility.ToJson(request);
        Debug.Log(json);
        using (UnityWebRequest www = UnityWebRequest.Post(_baseUrl + path, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + UserData.token);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else if (www.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Response res = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                callback(res);
                string tempjson = JsonUtility.ToJson(res);
                Debug.Log(tempjson);
            }
            www.Dispose();
        }

    }

    IEnumerator API_Get<Response>(string path, Action<Response> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(_baseUrl+path))
        {
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + UserData.token);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else if (www.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Response res = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                string resJson = JsonUtility.ToJson(res);
                Debug.Log(resJson);
                callback(res);
            }
        }
    }

    IEnumerator API_Delete(string path)
    {
        using (UnityWebRequest www = UnityWebRequest.Delete(_baseUrl + path))
        {
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + UserData.token);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else if (www.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    #endregion

    #region POST
    public void API_Login(TemporaryRequest req)
    {
        Debug.Log("API_Login");
        string path = "/v1/auth/temporary";
        StartCoroutine(API_Post<TemporaryRequest, TemporaryResponse>(path, req, res=>
        {
            UserData.SetString("Token", res.token);
        }));
    }

    public void API_CreateSingleNetworkBrain(CreateSingleNetworkBrainRequest req)
    {
        Debug.Log("API_CreateSingleNetworkBrain");
        string path = "/v1/experiment/single/network/brain";
        StartCoroutine(API_PostWithToken<CreateSingleNetworkBrainRequest, CreateSingleNetworkBrainResponse>(path, req, res =>
        {

        }));
    }
    public void API_CreateSingleNetworkChannel(CreateSingleNetworkChannelRequest req)
    {
        Debug.Log("API_CreateSingleNetworkBrain");
        string path = "/v1/experiment/single/network/channel";
        StartCoroutine(API_PostWithToken<CreateSingleNetworkChannelRequest, CreateSingleNetworkChannelResponse>(path, req, res =>
        {

        }));
    }
    #endregion

    #region GET
    public void API_LoadUserData()
    {
        string path = "/v1/experiment/single/network";
        StartCoroutine(API_Get<SingleNetworkResponse>(path, res => 
        {
            TextAsset textAsset = Resources.Load<TextAsset>("temp");
            SingleNetworkResponse re = JsonUtility.FromJson<SingleNetworkResponse>(textAsset.text);
            
            string resJson = JsonUtility.ToJson(re);
            Debug.LogError(resJson);
        }));
    }
    #endregion
}
