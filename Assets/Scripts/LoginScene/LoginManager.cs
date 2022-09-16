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

    public void Start()
    {
        //PlayerPrefs.DeleteAll();
        UserData.LoadAllData();
        CheckChangeScene();
    }

    public async void OnClick_Login()
    {
        TemporaryRequest req = new TemporaryRequest();
        req.email = _textEmail.text;
        req.domain = _textDomain.text;
        await NetworkManager.Instance.API_Login(req);
        CheckChangeScene();
    }

    private async void CheckChangeScene()
    {
        if (!string.IsNullOrEmpty(UserData.token))
        {
            AuthValidationResponse res = await NetworkManager.Instance.API_TokenValidation();
            if (res != null)
            {
                switch ((EStatusCode)res.statusCode)
                {
                    case EStatusCode.SUCCESS:
                        SceneManager.LoadScene("InGameScene");
                        break;
                    case EStatusCode.JWT_REFRESH:
                        UserData.SetString("Token", res.token);
                        SceneManager.LoadScene("InGameScene");
                        break;
                    default:
                        Debug.LogError(res.statusCode);
                        break;
                }
            }
        }
    }
}
