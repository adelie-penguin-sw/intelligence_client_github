using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace MainTab
{
    /// <summary>
    /// Brain Component Class<br />
    /// 모든 브레인 오브젝트는 이 클래스를 보유<br />
    /// </summary>
    public class Brain : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textNum;
        [SerializeField] private BrainData _brainData;
        private static int tempBrainID = 0;
        /// <summary>
        /// 지능 수치
        /// </summary>
        public double Intellect
        {
            get
            {
                return _brainData.intellect;
            }
        }

        /// <summary>
        /// 다음 Tick에 증가 될 예정인 지능수치
        /// </summary>
        public double StandByIntellect
        {
            get
            {
                return _brainData.standByIntellect;
            }
            set
            {
                _brainData.standByIntellect = value;
            }
        }

        public int ID
        {
            get
            {
                return _brainData.id;
            }
        }

        /// <summary>
        /// 해당 브레인 오브젝트 생성시 최초 1회 실행 되어야 한다.
        /// </summary>
        /// <param name="type">브레인 타입</param>
        public void Init(EBrainType type)
        {
            _brainData = new BrainData();
            _brainData.brainType = type;
            _brainData.id = tempBrainID++;
            if (type == EBrainType.GUIDEBRAIN)
                gameObject.SetActive(false);
            Set();
        }

        /// <summary>
        /// 브레인 타입에 따라 기능을 셋팅해주는 초기화 함수.
        /// </summary>
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

        /// <summary>
        /// Unity 기본 생명주기 Update를 대체해주는 함수 / 지속 실행 시켜주어야한다.
        /// </summary>
        /// <param name="dt_sec">deltaTime</param>
        public void AdvanceTime(float dt_sec)
        {
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                _brainData.intellect += _brainData.standByIntellect;
                _brainData.standByIntellect = 0;
                SetNumText(_brainData.intellect);
            }
        }

        /// <summary>
        /// 해당 오브젝트 삭제시 실행시켜주어야 한다.
        /// </summary>
        public void Dispose()
        {
            PoolManager.Instance.DespawnObject(EPrefabsType.BRAIN, gameObject);
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
