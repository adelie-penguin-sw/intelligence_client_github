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
        [SerializeField] private BrainData _brainData;

        #region property
        public HashSet<long> ReceiverIdList { get { return _brainData._receiverIdList; } }
        public HashSet<long> SenderIdList { get { return _brainData._senderIdList; } }

        /// <summary>
        /// 지능 수치
        /// </summary>
        public UpArrowNotation Intellect { get { return _brainData.intellect; } }
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
        #endregion

        public void Init(BrainData data)
        {
            _brainData = data;
            SetNumText(data.intellect);
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
                        break;
                    case EBrainType.MAINBRAIN:
                        _brainData.distance = 0;
                        transform.localScale = new Vector2(1.2f, 1.2f);
                        break;
                    case EBrainType.NORMALBRAIN:
                        transform.localScale = new Vector2(1, 1);
                        break;
                }

                transform.position = new Vector2(_brainData.coordinates.x, _brainData.coordinates.y);
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
            return _brainData._senderIdList.Add(id);
        }

        /// <summary>
        /// receiverIDList에 해당 id를 추가
        /// </summary>
        /// <param name="id">추가할 receiver id</param>
        /// <returns> 추가 성공시 true, 이미 존재하는 id일 경우 false 반환</returns>
        public bool AddReceiver(int id)
        {
            return _brainData._receiverIdList.Add(id);
        }

        /// <summary>
        /// 현재 senderidList에 포함되어있는 id인지 판별
        /// </summary>
        /// <param name="id">sender id</param>
        /// <returns>존재하면 true 없으면 false</returns>
        public bool IsContainsSender(int id)
        {
            return _brainData._senderIdList.Contains(id);
        }

        /// <summary>
        /// 현재 ReceiveridList에 포함되어있는 id인지 판별
        /// </summary>
        /// <param name="id">Receiver id</param>
        /// <returns>존재하면 true 없으면 false</returns>
        public bool IsContainsReceiver(int id)
        {
            return _brainData._receiverIdList.Contains(id);
        }

        private void SetNumText(UpArrowNotation num)
        {
            _textNum.text = num.ToString();
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
    }

}