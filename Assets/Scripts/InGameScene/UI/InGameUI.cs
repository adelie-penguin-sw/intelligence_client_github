using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using InGame;
public class InGameUI : MonoBehaviour, IGameBasicModule
{
    [SerializeField] private TextMeshProUGUI _txtCoreIntellect;
    [SerializeField] private TextMeshProUGUI _txtNP;
    [SerializeField] private TextMeshProUGUI _txtTP;

    public void Init()
    {
        UpdateCoreIntellectText();
        UpdateNPText();
        UpdateTPText();
    }

    public void Set()
    {
        UpdateCoreIntellectText();
        UpdateNPText();
        UpdateTPText();
    }


    public void AdvanceTime(float dt_sec)
    {
        UpdateCoreIntellectText();
        UpdateNPText();
        UpdateTPText();
    }

    public void LateAdvanceTime(float dt_sec)
    {
    }

    /// <summary>
    /// ???? Intellect Text?? ???? ?????? ????????.
    /// </summary>
    /// <param name="intellect">?????? intellect</param>
    public void UpdateCoreIntellectText()
    {
        _txtCoreIntellect.text = UserData.CoreIntellect.ToString();
    }

    /// <summary>
    /// ???? NP Text?? ???? ?????? ????????.
    /// </summary>
    /// <param name="np">?????? np</param>
    public void UpdateNPText()
    {
        _txtNP.text = string.Format("NP: {0}", UserData.NP.ToString());
    }

    /// <summary>
    /// ???? TP Text?? ???? ?????? ????????.
    /// </summary>
    /// <param name="tp">?????? tp</param>
    public void UpdateTPText()
    {
        _txtTP.text = string.Format("TP: {0}", UserData.TP.ToString());
    }

    public void OnClick_LeaderBoard()
    {
        LeaderboardPopup leaderboardPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "LeaderboardPopup", PopupType.NORMAL)
                            .GetComponent<LeaderboardPopup>();
        if (leaderboardPopup != null)
        {
            leaderboardPopup.Init();
        }
    }

    public void OnClick_Logout()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LoginScene");
    }

    public void Dispose()
    {
    }

}
