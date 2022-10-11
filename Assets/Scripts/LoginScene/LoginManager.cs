using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.SocialPlatforms;
using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using System.Text;
using AppleAuth.Extensions;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _textEmail;
    [SerializeField] private TMP_InputField _textDomain;
    [SerializeField] private TMP_InputField _textUsername;

    [SerializeField] private TextMeshProUGUI _usernameText;

    [SerializeField] private GameObject _contentNotLoggedIn;
    [SerializeField] private GameObject _contentLoggedIn;

    [SerializeField] private Button _btnAppleLogin;
    private IAppleAuthManager appleAuthManager;
    public const string AppleUserIdKey = "AppleUserIdKey";

    public void Start()
    {
        _contentNotLoggedIn.SetActive(true);
        _contentLoggedIn.SetActive(false);

        _btnAppleLogin.gameObject.SetActive(false);

        UserData.LoadAllData();
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
                    UserData.Username = res.username;
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
        if (!string.IsNullOrEmpty(UserData.token))
        {
            AuthValidationResponse res = await Managers.Network.API_TokenValidation();
            if (res != null)
            {
                // 유저네임 못받을때가 있어서 일단 이렇게 덕지덕지 발라놨어요
                GetUsernameResponse usernameRes;
                switch ((EStatusCode)res.statusCode)
                {
                    case EStatusCode.SUCCESS:
                        Managers.Definition.LoadS3Data();
                        usernameRes = await Managers.Network.API_GetUsername();
                        UserData.Username = usernameRes.username;
                        SceneManager.LoadScene("InGameScene");
                        break;
                    case EStatusCode.JWT_REFRESH:
                        UserData.SetString("Token", res.token);
                        Managers.Definition.LoadS3Data();
                        usernameRes = await Managers.Network.API_GetUsername();
                        UserData.Username = usernameRes.username;
                        SceneManager.LoadScene("InGameScene");
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
