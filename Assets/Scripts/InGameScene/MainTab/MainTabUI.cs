using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainTab
{
    public class MainTabUI : MonoBehaviour
    {
        public void OnClick_CreateBrainBtn()
        {
            NotificationManager.Instance.PostNotification(ENotiMessage.ON_CLICK_CREATE_BRAIN_BTN);
        }
    }
}