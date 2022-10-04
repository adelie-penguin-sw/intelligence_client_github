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

public class NetworkManager
{
    #region REST API FUNCTION
    protected static double timeout = 5;
    //private const string _baseUrl = "http://ec2-43-200-22-171.ap-northeast-2.compute.amazonaws.com:8080"; //테스트 서버 url
    public string editorBaseUrl;
    private const string _baseUrl = "http://ec2-52-79-187-33.ap-northeast-2.compute.amazonaws.com:8080";

    private async UniTask<T> SendToServer<T>(string url, ENetworkSendType sendType, string jsonBody = null, ENetworkRecvType resType = ENetworkRecvType.JSON)
    {
        LoadingPopup loadingPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "LoadingPopup", PopupType.NORMAL)
            .GetComponent<LoadingPopup>();
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
            await request.SendWebRequest().WithCancellation(cts.Token);
            Debug.Log(request.downloadHandler.text);
            if(request.responseCode != (int)EStatusCode.SUCCESS)
            {
                ErrorResponse errorResult = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);

                GameObject go = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "ErrorPopup", PopupType.IMPORTANT);
                go.GetComponent<ErrorPopup>().Init(errorResult);

                loadingPopup.Dispose();
                request.Dispose();
                return default;
            }

            T result;
            switch (resType)
            {
                case ENetworkRecvType.JSON:
                    result = JsonUtility.FromJson<T>(request.downloadHandler.text);
                    break;
                case ENetworkRecvType.FILE:
                    //result = JsonUtility.FromJson<T>("{\"csvDataString\":\"" +  res.downloadHandler.text + "\"}");
                    result = (T)(object)request.downloadHandler.text;
                    break;
                default:
                    result = default(T);
                    break;
            }
            
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
                return await SendToServer<T>(url, sendType, jsonBody, resType);
            }
        }
        catch(Exception e)
        {
            ErrorResponse errorResult = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);

            GameObject go = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "ErrorPopup", PopupType.IMPORTANT);
            go.GetComponent<ErrorPopup>().Init(errorResult);

            loadingPopup.Dispose();
            request.Dispose();
            return default;
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
    public const string PATH_TP_UPGRADE = "/v1/experiment/single/network/reinforcement";

    /// 
    /// GET PATH
    /// 
    public const string PATH_SINGLE_NETWORK = "/v1/experiment/single/network";
    public const string PATH_TOKEN_VALIDATION = "/v1/auth/validation";
    public const string PATH_LEADERBOARD = "/v1/leaderboard/single";
    public const string PATH_S3DATA = "/v1/assets/";

    ///
    /// GET & POST PATH
    ///
    public const string PATH_USERNAME = "/v1/user/username";

    /// 
    /// DELETE PATH
    /// 
    public const string PATH_DELETE_SINGLE_NETWORK_BRAIN = "/v1/experiment/single/network/brain/";
   
    #region POST

    public async UniTask<bool> API_Login(TemporaryRequest req)
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

        return (res != null);
    }

    public async UniTask<bool> API_PostUsername(PostUsernameRequest req,string userName)
    {
        string json = JsonUtility.ToJson(req);
        var res =
            await SendToServer<PostUsernameResponse>(
                PATH_USERNAME,
                ENetworkSendType.POST,
                json);

        UserData.Username = userName;
        return (res != null);
    }

    public async UniTask<bool> API_CreateBrain(CreateSingleNetworkBrainRequest req)
    {
        string json = JsonUtility.ToJson(req);
        var res =
            await SendToServer<CreateSingleNetworkBrainResponse>(
                    PATH_CREATE_SINGLE_NETWORK_BRAIN,
                    ENetworkSendType.POST,
                    json);
        if (res != null)
        {
            UserData.SingleNetworkWrapper.UpdateSingleNetworkData(req, res);
        }
        return (res != null);
    }

    public async UniTask<bool> API_CreateChannel(CreateSingleNetworkChannelRequest req)
    {
        string json = JsonUtility.ToJson(req);
        var res =
            await SendToServer<CreateSingleNetworkChannelResponse>(
                    PATH_CREATE_SINGLE_NETWORK_CHANNEL,
                    ENetworkSendType.POST,
                    json);
        if (res != null && res.statusCode == (int)EStatusCode.SUCCESS)
        {
            UserData.SingleNetworkWrapper.UpdateSingleNetworkData(req, res);
        }
        return (res != null);
    }

    public async UniTask<bool> API_UpgradeBrain(CreateSingleNetworkBrainNumberRequest req)
    {
        string json = JsonUtility.ToJson(req);
        var res =
            await SendToServer<CreateSingleNetworkBrainNumberResponse>(
                    PATH_SINGLE_NETWORK_BRAIN_NUMBER,
                    ENetworkSendType.POST,
                    json);
        if (res != null && res.statusCode == (int)EStatusCode.SUCCESS)
        {
            UserData.SingleNetworkWrapper.UpdateSingleNetworkData(res);
        }
        return (res != null);
    }

    public async UniTask<bool> API_NetworkReset()
    {
        string json = "";
        var res =
            await SendToServer<SingleNetworkResponse>(
                    PATH_SINGLE_NETWORK_RESET,
                    ENetworkSendType.POST,
                    json);
        if (res != null && res.statusCode == (int)EStatusCode.SUCCESS)
        {
            UserData.SingleNetworkWrapper = new SingleNetworkWrapper(res);
        }
        return (res != null);
    }

    public async UniTask<bool> API_TpUpgrade(TpUpgradeSingleNetworkRequest req)
    {
        string json = JsonUtility.ToJson(req);
        var res =
            await SendToServer<TpUpgradeSingleNetworkResponse>(
                    PATH_TP_UPGRADE,
                    ENetworkSendType.POST,
                    json);
        if (res != null && res.statusCode == (int)EStatusCode.SUCCESS)
        {
            UserData.UpdateTPUpgradeCounts(res.upgradeCondition);
            UserData.TP = new UpArrowNotation(
            res.TP.top3Coeffs[0],
            res.TP.top3Coeffs[1],
            res.TP.top3Coeffs[2],
            res.TP.operatorLayerCount);
        }
        return (res != null);
    }
    #endregion

    #region GET

    public async UniTask<bool> API_LoadUserData()
    {
        var res =
            await SendToServer<SingleNetworkResponse>(
                    PATH_SINGLE_NETWORK,
                    ENetworkSendType.GET);
        if (res != null)
        {
            UserData.SingleNetworkWrapper = new SingleNetworkWrapper(res);
            UserData.UpdateTPUpgradeCounts(res.upgradeCondition);
        }
        return (res != null);
    }

    public async UniTask<AuthValidationResponse> API_TokenValidation()
    {
        var res =
            await SendToServer<AuthValidationResponse>(
                    PATH_TOKEN_VALIDATION,
                    ENetworkSendType.GET);
        return res;
    }

    public async UniTask<GetUsernameResponse> API_GetUsername()
    {
        var res =
            await SendToServer<GetUsernameResponse>(
                PATH_USERNAME,
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

    public async UniTask<string> API_S3Data(string fileName)
    {
        var res =
            await SendToServer<string>(
                    PATH_S3DATA + fileName,
                    ENetworkSendType.GET,
                    resType:ENetworkRecvType.FILE);
        return res;
    }
    #endregion

    #region DELETE

    public async UniTask<bool> API_DeleteBrain(long brainID)
    {
        var res =
            await SendToServer<DeleteSingleNetworkBrainResponse>(
                    PATH_DELETE_SINGLE_NETWORK_BRAIN + brainID.ToString(),
                    ENetworkSendType.DELETE);
        if (res != null)
        {
            UserData.SingleNetworkWrapper.UpdateSingleNetworkData(res);
        }
        return (res != null);
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

public enum ENetworkRecvType
{
    JSON,
    FILE, 
}

public enum UrlType { TEST = 1, DEPLOY = 2, LOCAL = 3, }