using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
namespace MainTab
{
    [Serializable]
    public class Brain : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textNum;
        [SerializeField] private TextMeshPro _textMul;
        [SerializeField] private BrainData _brainData;

        [SerializeField] private bool _isCollisionGuide = false;
        [SerializeField] private long _lastCalcTime;

        #region property
        public HashSet<long> ReceiverIdList { get { return _brainData.receiverIds; } }
        public HashSet<long> SenderIdList { get { return _brainData.senderIds; } }

        public BrainData BrainData { get { return _brainData; } }

        /// <summary>
        /// 지능 수치 계산하여 반환
        /// </summary>
        public UpArrowNotation Intellect
        {
            get
            {
                double elapsedTime = (double)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - _lastCalcTime) / 1000f;
                UpArrowNotation intellect = Equation.GetCurrentIntellect(_brainData.intellectEquation, elapsedTime);

                if (_brainData.id == 0)
                {
                    UserData.CoreIntellect = intellect;
                }
                return intellect;
            }
        }

        /// <summary>
        /// 마지막으로 지능이 계산된 시각 반환
        /// </summary>
        public long LastCalcTime { get { return _lastCalcTime; } }

        /// <summary>
        /// 지능 증폭계수 반환
        /// </summary>
        public UpArrowNotation Multiplier { get { return _brainData.multiplier; } }

        /// <summary>
        /// 해당 브레인의 ID
        /// </summary>
        public long ID { get { return _brainData.id; } }

        /// <summary>
        /// 브레인 타입
        /// </summary>
        public EBrainType Type { get { return _brainData.brainType; } }

        /// <summary>
        /// 브레인 거리
        /// </summary>
        public long Distance { get { return _brainData.distance; } set { _brainData.distance = value; } }

        public bool IsCollisionGuide
        {
            get
            {
                return _isCollisionGuide;
            }
        }
        #endregion

        public void Init(BrainData data)
        {
            _brainData = data;
            SetNumText(Intellect);
            SetMulText(Multiplier);
            if (_brainData.brainType == EBrainType.GUIDEBRAIN)
                gameObject.SetActive(false);
            Set();
        }

        public void Set()
        {
            if (_brainData != null)
            {
                switch (_brainData.brainType)
                {
                    case EBrainType.GUIDEBRAIN:
                        _textNum.gameObject.SetActive(false);
                        _textMul.gameObject.SetActive(false);
                        break;
                    case EBrainType.MAINBRAIN:
                        _brainData.distance = 0;
                        transform.localScale = new Vector2(2f, 2f);
                        break;
                    case EBrainType.NORMALBRAIN:
                        transform.localScale = new Vector2(1, 1);
                        break;
                }

                transform.position = new Vector2(_brainData.coordinates.x, _brainData.coordinates.y);
                _brainData.lastCalcTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                _lastCalcTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
        }

        public void AdvanceTime(float dt_sec)
        {
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                SetNumText(Intellect);
                SetMulText(Multiplier);
            }
        }

        public void Dispose()
        {
            _brainData = null;

            PoolManager.Instance.DespawnObject(EPrefabsType.BRAIN, gameObject);
        }

        /// <summary>
        /// senderIDList에 해당 id를 추가
        /// </summary>
        /// <param name="id">추가할 sender id</param>
        /// <returns> 추가 성공시 true, 이미 존재하는 id일 경우 false 반환</returns>
        public bool AddSender(int id)
        {
            return _brainData.senderIds.Add(id);
        }

        /// <summary>
        /// receiverIDList에 해당 id를 추가
        /// </summary>
        /// <param name="id">추가할 receiver id</param>
        /// <returns> 추가 성공시 true, 이미 존재하는 id일 경우 false 반환</returns>
        public bool AddReceiver(int id)
        {
            return _brainData.receiverIds.Add(id);
        }

        /// <summary>
        /// 현재 senderidList에 포함되어있는 id인지 판별
        /// </summary>
        /// <param name="id">sender id</param>
        /// <returns>존재하면 true 없으면 false</returns>
        public bool IsContainsSender(int id)
        {
            return _brainData.senderIds.Contains(id);
        }

        /// <summary>
        /// 현재 ReceiveridList에 포함되어있는 id인지 판별
        /// </summary>
        /// <param name="id">Receiver id</param>
        /// <returns>존재하면 true 없으면 false</returns>
        public bool IsContainsReceiver(int id)
        {
            return _brainData.receiverIds.Contains(id);
        }

        private void SetNumText(UpArrowNotation num)
        {
            _textNum.text = num.ToString();
        }

        private void SetMulText(UpArrowNotation num)
        {
            _textMul.text = "x" + num.ToString();
        }

        #region EventData
        private void OnMouseDown()
        {
            if (_brainData == null)
                return;
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.CLASS_BRAIN, this);
                NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_DOWN_BRAIN, _sendData);
            }
        }

        private void OnMouseExit()
        {
            if (_brainData == null)
                return;
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_EXIT_BRAIN);
            }
        }

        private void OnMouseUp()
        {
            if (_brainData == null)
                return;
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_UP_BRAIN);
            }
        }

        private void OnMouseEnter()
        {
            if (_brainData == null)
                return;
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.CLASS_BRAIN, this);
                NotificationManager.Instance.PostNotification(ENotiMessage.MOUSE_ENTER_BRAIN, _sendData);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_brainData == null)
                return;

            if (_brainData.brainType == EBrainType.GUIDEBRAIN)
            {
                _isCollisionGuide = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_brainData == null)
                return;

            if (_brainData.brainType == EBrainType.GUIDEBRAIN)
            {
                _isCollisionGuide = false;
            }
        }
        #endregion
    }

}