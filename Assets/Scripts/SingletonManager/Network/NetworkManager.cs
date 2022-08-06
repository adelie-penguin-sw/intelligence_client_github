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

#region ???? API??
    //private const string _baseUrl = "http://ec2-3-38-74-157.ap-northeast-2.compute.amazonaws.com:8080";
    private const string _baseUrl = "http://ec2-3-39-5-11.ap-northeast-2.compute.amazonaws.com:8080";
    private IEnumerator API_Post<Request>(string path , Request request)
    {
        string json = JsonUtility.ToJson(request);
        using UnityWebRequest www = UnityWebRequest.Post(_baseUrl + path, json);
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (ErrorCheck(www))
            {
                Debug.Log(www.downloadHandler.text);
            }
        }

    }
    private IEnumerator API_Post<Request,Response>(string path, Request request, Action<Response> callback)
    {
        string json = JsonUtility.ToJson(request);
        using UnityWebRequest www = UnityWebRequest.Post(_baseUrl + path, json);
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (ErrorCheck(www))
            {
                Debug.Log(www.downloadHandler.text);
                Response res = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                callback(res);
                string tempjson = JsonUtility.ToJson(res);
                Debug.Log(tempjson);
            }
        }

    }
    private IEnumerator API_PostWithToken<Request, Response>(string path, Request request, Action<Response> callback)
    {
        string json = JsonUtility.ToJson(request);
        using UnityWebRequest www = UnityWebRequest.Post(_baseUrl + path, json);
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + UserData.token);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (ErrorCheck(www))
            {
                Debug.Log(www.downloadHandler.text);
                Response res = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                callback(res);
                string tempjson = JsonUtility.ToJson(res);
                Debug.Log(tempjson);
            }
        }

    }

    IEnumerator API_Get<Response>(string path, Action<Response> callback)
    {
        using UnityWebRequest www = UnityWebRequest.Get(_baseUrl + path);
        {
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + UserData.token);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (ErrorCheck(www))
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
        using UnityWebRequest www = UnityWebRequest.Delete(_baseUrl + path);
        {
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + UserData.token);

            yield return www.SendWebRequest();

            if (ErrorCheck(www))
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    IEnumerator API_Delete<Response>(string path, Action<Response> callback)
    {
        using UnityWebRequest www = UnityWebRequest.Delete(_baseUrl + path);
        {
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + UserData.token);

            yield return www.SendWebRequest();

            if (ErrorCheck(www))
            {
                Debug.Log(www.downloadHandler.text);
                Response res = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                string resJson = JsonUtility.ToJson(res);
                Debug.Log(resJson);
                callback(res);
            }
        }
    }

    private bool ErrorCheck(UnityWebRequest www)
    {
        switch(www.result)
        {
            case UnityWebRequest.Result.Success:
                return true;
            default:
                Debug.LogError(www.error);
                Debug.LogError(www.downloadHandler.text);
                return false;

        }
    }
    #endregion

    #region POST
    /// <summary>
    /// ??? API
    /// </summary>
    /// <param name="req"></param>
    /// <param name="callback"></param>
    public void API_Login(TemporaryRequest req, Action callback)
    {
        Debug.Log("API_Login");
        string path = "/v1/auth/temporary";
        StartCoroutine(API_Post<TemporaryRequest, TemporaryResponse>(path, req, res=>
        {
            UserData.SetString("Token", res.token);
            callback();
        }));
    }

    /// <summary>
    /// ??? ?? API
    /// </summary>
    /// <param name="req"></param>
    /// <param name="callback"></param>
    public void API_CreateSingleNetworkBrain(CreateSingleNetworkBrainRequest req, Action<CreateSingleNetworkBrainResponse> callback)
    {
        Debug.Log("API_CreateSingleNetworkBrain");
        string path = "/v1/experiment/single/network/brain";
        StartCoroutine(API_PostWithToken<CreateSingleNetworkBrainRequest, CreateSingleNetworkBrainResponse>(path, req, res =>
        {
            callback(res);
        }));
    }

    /// <summary>
    /// ?? ?? API
    /// </summary>
    /// <param name="req"></param>
    /// <param name="callback"></param>
    public void API_CreateSingleNetworkChannel(CreateSingleNetworkChannelRequest req, Action<CreateSingleNetworkChannelResponse> callback)
    {
        Debug.Log("API_CreateSingleNetworkBrain");
        string path = "/v1/experiment/single/network/channel";
        StartCoroutine(API_PostWithToken<CreateSingleNetworkChannelRequest, CreateSingleNetworkChannelResponse>(path, req, res =>
        {
            callback(res);
        }));
    }

    /// <summary>
    /// ??? ????? API
    /// </summary>
    /// <param name="req"></param>
    public void API_CreateSingleNetworkBrainNumber(CreateSingleNetworkBrainNumberRequest req)
    {
        Debug.Log("API_CreateSingleNetworkBrainNumber");
        string path = "/v1/experiment/single/network/brain/intelligence";
        StartCoroutine(API_PostWithToken<CreateSingleNetworkBrainNumberRequest, CreateSingleNetworkBrainNumberResponse>(path, req, res =>
        {

        }));
    }
    #endregion

    #region GET
    /// <summary>
    /// ?? ??? ???? API
    /// </summary>
    public void API_LoadUserData()
    {
        string path = "/v1/experiment/single/network";
        StartCoroutine(API_Get<SingleNetworkResponse>(path, res =>
        {
            //wrapper = new SingleNetworkWrapper(res);
        }));
    }


    /// <summary>
    /// ?? ??? ???? API 
    /// </summary>
    public void API_LoadUserData(Action<SingleNetworkWrapper> callback)
    {
        string path = "/v1/experiment/single/network";
        StartCoroutine(API_Get<SingleNetworkResponse>(path, res =>
        {
            SingleNetworkWrapper wrapper = new SingleNetworkWrapper(res);
            callback(wrapper);
        }));
    }
    #endregion

    #region DELETE
    /// <summary>
    /// ??? ?? API
    /// </summary>
    /// <param name="brainID"></param>
    public void API_DeleteSingleNetworkBrain(int brainID)
    {
        string path = "/v1/experiment/single/network/brain/" + brainID.ToString();
        Debug.LogError(path);
        StartCoroutine(API_Delete<DeleteSingleNetworkBrainResponse>(path, res =>
        {

        }));
    }
    #endregion
}

public enum StatusCode
{
    JWT_REFRESH = 202,
    BAD_REQUEST  = 400,
    Forbidden = 403,

}
