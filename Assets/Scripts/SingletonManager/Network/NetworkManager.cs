using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEditor;

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

    #region REST API FUNCTION
    protected static double timeout = 5;
    private const string _baseUrl = "http://ec2-43-200-22-171.ap-northeast-2.compute.amazonaws.com:8080"; //테스트 서버 url
    public string editorBaseUrl;
    //private const string _baseUrl = "http://ec2-3-38-74-157.ap-northeast-2.compute.amazonaws.com:8080"; //배포 서버 url

    private async UniTask<T> SendToServer<T>(string url, ENetworkSendType sendType, string jsonBody = null)
    {
        LoadingPopup loadingPopup = PopupManager.Instance.CreatePopup(EPrefabsType.POPUP, "LoadingPopup").GetComponent<LoadingPopup>();
        //await Task.Delay(1000);
        //1. 네트워크 체크.
        await CheckNetwork();

        //2. API URL 생성.
        string requestURL = _baseUrl + url;
#if UNITY_EDITOR
        switch ((UrlType)EditorPrefs.GetInt("urlType"))
        {
            case UrlType.TEST:
                editorBaseUrl = "http://ec2-52-79-187-33.ap-northeast-2.compute.amazonaws.com:8080"; //테스트 서버 url
                break;
            case UrlType.DEPLOY:
                editorBaseUrl = "http://ec2-43-200-22-171.ap-northeast-2.compute.amazonaws.com:8080"; //배포 서버 url
                break;
            case UrlType.LOCAL:
                editorBaseUrl = "http://localhost:8080";
                break;
        }
        requestURL = editorBaseUrl + url;
#endif
        //3. Timeout 설정.
        var cts = new CancellationTokenSource();
        cts.CancelAfterSlim(TimeSpan.FromSeconds(timeout));

        //4. 웹 요청 생성(Get, Post, Delete, Update)
        UnityWebRequest request = new UnityWebRequest(requestURL, sendType.ToString());

        //5. Body 정보 입력
        request.downloadHandler = new DownloadHandlerBuffer();
        if (!string.IsNullOrEmpty(jsonBody))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        //6. Header 정보 입력
        SetHeaders(request);
        try
        {
            var res = await request.SendWebRequest().WithCancellation(cts.Token);
            Debug.Log(res.downloadHandler.text);
            T result = JsonUtility.FromJson<T>(res.downloadHandler.text);
            request.Dispose();
            loadingPopup.Dispose();
            return result;
        }
        catch(OperationCanceledException ex)
        {
            if(ex.CancellationToken == cts.Token)
            {
                Debug.Log("TimeOut");
                //TODO: 네트워크 재시도 팝업 호출

                //재시도
                return await SendToServer<T>(url, sendType, jsonBody);
            }
        }
        catch(Exception e)
        {
            ErrorResponse errorResult = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);

            GameObject go = PopupManager.Instance.CreatePopup(EPrefabsType.POPUP, "ErrorPopup");
            go.GetComponent<ErrorPopup>().Init(errorResult);

            loadingPopup.Dispose();
            request.Dispose();
            return default;

            //Debug.LogError(e.Message);
            //return default;
        }
        loadingPopup.Dispose();
        request.Dispose();
        return default;
    }

    private static async UniTask CheckNetwork()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            //TODO: 네트워크 오류 팝업 호출
            Debug.LogError("네트워크가 연결되지 않음");
            await UniTask.WaitUntil(() => Application.internetReachability != NetworkReachability.NotReachable);
            Debug.LogError("네트워크가 다시 연결 됌");
        }
    }

    private static void SetHeaders(UnityWebRequest request)
    {
        //필요한 Header 추가.
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + UserData.token);
    }

    #endregion

    /// 
    /// POST PATH
    /// 
    public const string PATH_TEMPORARY = "/v1/auth/temporary";
    public const string PATH_CREATE_SINGLE_NETWORK_BRAIN = "/v1/experiment/single/network/brain";
    public const string PATH_CREATE_SINGLE_NETWORK_CHANNEL = "/v1/experiment/single/network/channel";
    public const string PATH_SINGLE_NETWORK_BRAIN_NUMBER = "/v1/experiment/single/network/brain/intelligence";
    public const string PATH_SINGLE_NETWORK_RESET = "/v1/experiment/single/network/reset";

    /// 
    /// GET PATH
    /// 
    public const string PATH_SINGLE_NETWORK = "/v1/experiment/single/network";
    public const string PATH_TOKEN_VALIDATION = "/v1/auth/validation";
    public const string PATH_LEADERBOARD = "/v1/leaderboard/single";

    /// 
    /// DELETE PATH
    /// 
    public const string PATH_DELETE_SINGLE_NETWORK_BRAIN = "/v1/experiment/single/network/brain/";
   
    #region POST

    public async UniTask API_Login(TemporaryRequest req)
    {
        Debug.Log("API_Login");
        string json = JsonUtility.ToJson(req);
        var res =
            await SendToServer<TemporaryResponse>(
            PATH_TEMPORARY,
            ENetworkSendType.POST,
            json);
        //if (res != null)
        {
            Debug.Log(res.token);
            UserData.SetString("Token", res.token);
        }
    }

    public async UniTask<CreateSingleNetworkBrainResponse> API_CreateBrain(CreateSingleNetworkBrainRequest req)
    {
        string json = JsonUtility.ToJson(req);
        var res =
            await SendToServer<CreateSingleNetworkBrainResponse>(
                    PATH_CREATE_SINGLE_NETWORK_BRAIN,
                    ENetworkSendType.POST,
                    json);
        return res;
    }

    public async UniTask<CreateSingleNetworkChannelResponse> API_CreateChannel(CreateSingleNetworkChannelRequest req)
    {
        string json = JsonUtility.ToJson(req);
        var res =
            await SendToServer<CreateSingleNetworkChannelResponse>(
                    PATH_CREATE_SINGLE_NETWORK_CHANNEL,
                    ENetworkSendType.POST,
                    json);
        return res;
    }

    public async UniTask<CreateSingleNetworkBrainNumberResponse> API_UpgradeBrain(CreateSingleNetworkBrainNumberRequest req)
    {
        string json = JsonUtility.ToJson(req);
        var res =
            await SendToServer<CreateSingleNetworkBrainNumberResponse>(
                    PATH_SINGLE_NETWORK_BRAIN_NUMBER,
                    ENetworkSendType.POST,
                    json);
        return res;
    }

    public async UniTask<SingleNetworkResponse> API_NetworkReset()
    {
        string json = "";
        var res =
            await SendToServer<SingleNetworkResponse>(
                    PATH_SINGLE_NETWORK_RESET,
                    ENetworkSendType.POST,
                    json);
        return res;
    }
    #endregion

    #region GET

    public async UniTask<SingleNetworkResponse> API_LoadUserData()
    {
        var res =
            await SendToServer<SingleNetworkResponse>(
                    PATH_SINGLE_NETWORK,
                    ENetworkSendType.GET);
        return res;
    }

    public async UniTask<AuthValidationResponse> API_TokenValidation()
    {
        var res =
            await SendToServer<AuthValidationResponse>(
                    PATH_TOKEN_VALIDATION,
                    ENetworkSendType.GET);
        return res;
    }

    public async UniTask<LeaderboardResponse> API_Leaderboard()
    {
        var res =
            await SendToServer<LeaderboardResponse>(
                    PATH_LEADERBOARD,
                    ENetworkSendType.GET);
        return res;
    }
    #endregion

    #region DELETE

    public async UniTask<DeleteSingleNetworkBrainResponse> API_DeleteBrain(long brainID)
    {
        var res =
            await SendToServer<DeleteSingleNetworkBrainResponse>(
                    PATH_DELETE_SINGLE_NETWORK_BRAIN + brainID.ToString(),
                    ENetworkSendType.DELETE);
        return res;
    }
    #endregion
}

public enum EStatusCode
{
    SUCCESS = 200,
    JWT_REFRESH = 202,
    BAD_REQUEST  = 400,
    FORBIDDEN = 403,
}

public enum ENetworkSendType
{
    GET,
    POST,
    PUT,
    DELETE,
}

public enum UrlType { TEST = 1, DEPLOY = 2, LOCAL = 3, }