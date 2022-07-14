using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace MainTab
{
    public class Brain : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro _textNum;
        [SerializeField]
        private BrainData _brainData;

        public void Init(EBrainType type)
        {
            _brainData.brainType = type;
            Set();
        }

        public void Set()
        {
            switch(_brainData.brainType)
            {
                case EBrainType.GUIDEBRAIN:
                    _textNum.gameObject.SetActive(false);
                    break;
                case EBrainType.MAINBRAIN:
                    break;
                case EBrainType.NORMALBRAIN:
                    break;
            }
        }
        public void AdvanceTime(float dt_sec)
        {
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                SetNumText(_brainData.intellect);
            }
        }

        public void Dispose()
        {
        }
        #region EventData
        private Hashtable _sendData = new Hashtable();
        private void OnMouseDown()
        {
            _sendData.Clear();
            _sendData.Add(EDataParamKey.CLASS_BRAIN, this);
            NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_DOWN_BRAIN, _sendData);
        }
        private void OnMouseExit()
        {
            _sendData.Clear();
            _sendData.Add(EDataParamKey.CLASS_BRAIN, this);
            NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_EXIT_BRAIN, _sendData);
        }
   
        private void OnMouseUp()
        {
            _sendData.Clear();
            _sendData.Add(EDataParamKey.CLASS_BRAIN, this);
            NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_UP_BRAIN, _sendData);
        }
        #endregion
        private void SetNumText(double num)
        {
            _textNum.text = num.ToString();
        }
    }
}
