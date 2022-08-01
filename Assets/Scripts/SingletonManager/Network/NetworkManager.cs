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

    private string _accessToken;
    public string AccessToken
    {
        get
        {
            return _accessToken;
        }
    }


    void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }

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

    IEnumerator API_Get<Response>(string path)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(_baseUrl+path))
        {
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + _accessToken);
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
            }
        }
    }

    IEnumerator API_Delete(string path)
    {
        using (UnityWebRequest www = UnityWebRequest.Delete(_baseUrl + path))
        {
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", "Bearer " + _accessToken);

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
}
