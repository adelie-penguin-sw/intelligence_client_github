using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _textEmail;
    [SerializeField] private TMP_InputField _textDomain;

    public void Start()
    {
        UserData.LoadAllData();
        CheckChangeScene();
    }

    public void OnClick_Login()
    {
        TemporaryRequest req = new TemporaryRequest();
        req.email = _textEmail.text;
        req.domain = _textDomain.text;
        Debug.Log("Onclick_Login");
        NetworkManager.Instance.API_Login(req);

        CheckChangeScene();
    }

    private void CheckChangeScene()
    {
        if (!string.IsNullOrEmpty(UserData.token))
        {
            NetworkManager.Instance.API_LoadUserData();
            SceneManager.LoadScene("InGameScene");
        }
    }
}
