using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Extensions;
using System.Text;
using UnityEngine.UI;
using AppleAuth.Native;
using GooglePlayGames.BasicApi;

#if UNITY_ANDROID
using GooglePlayGames;
#endif
public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _textEmail;
    [SerializeField] private TMP_InputField _textDomain;
    [SerializeField] private TMP_InputField _textUsername;

    [SerializeField] private TextMeshProUGUI _usernameText;

    [SerializeField] private GameObject _contentNotLoggedIn;
    [SerializeField] private GameObject _contentLoggedIn;

    [SerializeField] private Button _btnAppleLogin;
    [SerializeField] private Button _btnGoogleLogin;
    [SerializeField] private TextMeshProUGUI _textLog;

    private IAppleAuthManager appleAuthManager;
    public const string AppleUserIdKey = "AppleUserIdKey";
    public const string GoogleUserIdKey = "GoogleUserIdKey";

    public void Start()
    {
        _contentNotLoggedIn.SetActive(true);
        _contentLoggedIn.SetActive(false);

        _btnAppleLogin.gameObject.SetActive(false);
        _btnGoogleLogin.gameObject.SetActive(false);

        SaveUserData.LoadAllData();
        CheckChangeScene();

#if UNITY_IOS
        _btnAppleLogin.gameObject.SetActive(true);
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            this.appleAuthManager = new AppleAuthManager(deserializer);
        }
#endif
#if UNITY_ANDROID
        _btnGoogleLogin.gameObject.SetActive(true);
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
.RequestServerAuthCode(false)
.EnableSavedGames()
.RequestIdToken()
.RequestEmail()
.Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#endif
    }

    public void Update()
    {
#if UNITY_IOS
        if (this.appleAuthManager != null)
        {
            this.appleAuthManager.Update();
        }
#endif
#if UNITY_ANDROID
#endif
    }

    public void OnClickGoogleLogin()
    {
#if UNITY_ANDROID
        if(!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                Debug.Log("success: " + success);
                if(success)
                {
                    PlayGamesPlatform.Instance.GetAnotherServerAuthCode(true, code =>
                    {
                        _textLog.text = code;
                        Debug.LogFormat("code : {0}",code);
                        Debug.LogErrorFormat("code : {0}", code);

                        string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                        string idToken = PlayGamesPlatform.Instance.GetIdToken();
                        string userId = PlayGamesPlatform.Instance.GetUserId();
                        Debug.LogErrorFormat("{0} {1} {2}", authCode, idToken, userId);
                        Debug.Log("GetServerAuthCode: " + PlayGamesPlatform.Instance.GetServerAuthCode());
                        this.Login(Social.localUser.id, GoogleUserIdKey);
                    });
                }
            });
        }
        
#endif
    }

    /// <summary>
    /// 애플로그인 클릭
    /// </summary>
    public void OnClickAppleLogin()
    {
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

        this.appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                // Obtained credential, cast it to IAppleIDCredential
                var appleIdCredential = credential as IAppleIDCredential;
                        if (appleIdCredential != null)
                        {
                    // Apple User ID
                    // PlayerPrefs로 저장하고 있
                    var userId = appleIdCredential.User;
                            PlayerPrefs.SetString(AppleUserIdKey, userId);
                    // Email 첫 로그인시 가져옴.
                    var email = appleIdCredential.Email;

                    // Full name 첫 로그인시 가져옴. 
                    var fullName = appleIdCredential.FullName;

                    // Identity token 로그인 할 때마다 바뀜
                    var identityToken = Encoding.UTF8.GetString(
                                appleIdCredential.IdentityToken,
                                0,
                                appleIdCredential.IdentityToken.Length);

                    // Authorization code 로그인 할 때마다 바뀜
                    var authorizationCode = Encoding.UTF8.GetString(
                                appleIdCredential.AuthorizationCode,
                                0,
                                appleIdCredential.AuthorizationCode.Length);

                    this.Login(userId, AppleUserIdKey);
                }
            },
            error =>
            {
                // Something went wrong
                var authorizationErrorCode = error.GetAuthorizationErrorCode();
            });
    }

    public async void Login(string email, string domain)
    {
        TemporaryRequest req = new TemporaryRequest();
        req.email = email;
        req.domain = domain;

        if (await Managers.Network.API_Login(req))
        {
            GetUsernameResponse res = await Managers.Network.API_GetUsername();
            if (res != null)
            {
                if (string.IsNullOrEmpty(res.username))   // 유저네임 아직 설정 안했을때
                {
                    _contentNotLoggedIn.SetActive(false);
                    _contentLoggedIn.SetActive(true);
                }
                else                        // 이미 유저네임을 가진 상태에서 로그인할때
                {
                    UserData.username = res.username;
                    CheckChangeScene();
                }
            }
        }
    }
    public void OnClick_Login()
    {
        this.Login(_textEmail.text, _textDomain.text);
    }

    private async void CheckChangeScene()
    {
        if (!string.IsNullOrEmpty(SaveUserData.Token))
        {
            AuthValidationResponse res = await Managers.Network.API_TokenValidation();
            if (res != null)
            {
                // 유저네임 못받을때가 있어서 일단 이렇게 덕지덕지 발라놨어요
                GetUsernameResponse usernameRes;
                switch ((EStatusCode)res.statusCode)
                {
                    case EStatusCode.SUCCESS:
                        if (await Managers.Definition.LoadS3Data())
                        {
                            usernameRes = await Managers.Network.API_GetUsername();
                            UserData.username = usernameRes.username;

                            if (await Managers.Network.API_LoadUserData())
                            {
                                SceneManager.LoadScene("InGameScene");
                            }
                        }
                        break;
                    case EStatusCode.JWT_REFRESH:
                        SaveUserData.Token = res.token;
                        if (await Managers.Definition.LoadS3Data())
                        {
                            usernameRes = await Managers.Network.API_GetUsername();
                            UserData.username = usernameRes.username;

                            if (await Managers.Network.API_LoadUserData())
                            {
                                SceneManager.LoadScene("InGameScene");
                            }
                        }
                        break;
                    default:
                        Debug.LogError(res.statusCode);
                        break;
                }
            }
        }
    }

    public async void OnClick_Start()
    {
        PostUsernameRequest req = new PostUsernameRequest();
        req.username = _textUsername.text;
        if (await Managers.Network.API_PostUsername(req, _textUsername.text))
        {
            CheckChangeScene();
        }
    }
}
