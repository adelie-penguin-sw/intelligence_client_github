using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _textEmail;
    [SerializeField] private TMP_InputField _textDomain;
    [SerializeField] private TMP_InputField _textUsername;

    [SerializeField] private TextMeshProUGUI _usernameText;

    [SerializeField] private GameObject _contentNotLoggedIn;
    [SerializeField] private GameObject _contentLoggedIn;

    public void Start()
    {
        _contentNotLoggedIn.SetActive(true);
        _contentLoggedIn.SetActive(false);

        //PlayerPrefs.DeleteAll();
        UserData.LoadAllData();
        CheckChangeScene();
    }

    public async void OnClick_Login()
    {
        TemporaryRequest req = new TemporaryRequest();
        req.email = _textEmail.text;
        req.domain = _textDomain.text;

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

    private async void CheckChangeScene()
    {
        if (!string.IsNullOrEmpty(UserData.token))
        {
            AuthValidationResponse res = await Managers.Network.API_TokenValidation();
            if (res != null)
            {
                switch ((EStatusCode)res.statusCode)
                {
                    case EStatusCode.SUCCESS:
                        Managers.Definition.LoadS3Data();
                        SceneManager.LoadScene("InGameScene");
                        break;
                    case EStatusCode.JWT_REFRESH:
                        UserData.SetString("Token", res.token);
                        Managers.Definition.LoadS3Data();
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
