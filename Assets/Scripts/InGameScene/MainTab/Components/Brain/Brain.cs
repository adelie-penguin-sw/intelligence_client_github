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
        public double Intellect
        {
            get
            {
                return _brainData.intellect;
            }
        }
        public double StandByIntellect
        {
            get
            {
                return _brainData.standByIntellect;
            }
            set
            {
                _brainData.standByIntellect  = value;
            }
        }

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
                _brainData.intellect += _brainData.standByIntellect;
                _brainData.standByIntellect = 0;
                SetNumText(_brainData.intellect);
            }
        }

        public void Dispose()
        {
        }
        #region EventData
        private void OnMouseDown()
        {
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.CLASS_BRAIN, this);
                NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_DOWN_BRAIN, _sendData);
            }
        }
        private void OnMouseExit()
        {
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_EXIT_BRAIN);
            }
        }
   
        private void OnMouseUp()
        {
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_UP_BRAIN);
            }
        }

        private void OnMouseEnter()
        {
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.CLASS_BRAIN, this);
                NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_ENTER_BRAIN, _sendData);
            }
        }
        #endregion
        private void SetNumText(double num)
        {
            _textNum.text = num.ToString();
        }
    }
}
