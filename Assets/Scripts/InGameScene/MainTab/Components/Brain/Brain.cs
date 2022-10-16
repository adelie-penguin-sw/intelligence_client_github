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
        [SerializeField] private Dictionary<long, Brain> _brainNetwork;
        [SerializeField] private BrainData _brainData;
        [SerializeField] private UpArrowNotation _currentIntellectLimit;
        [SerializeField] private UpArrowNotation _fullMultiplier;

        [SerializeField] private bool _isCollision = false;
        [SerializeField] private bool _isLocked = false;

        #region property
        public HashSet<long> ReceiverIdList { get { return _brainData.receiverIds; } }
        public HashSet<long> SenderIdList { get { return _brainData.senderIds; } }

        public BrainData BrainData { get { return _brainData; } }
        public Dictionary<long, Brain> BrainNetwork
        {
            get
            {
                return _brainNetwork;
            }
        }

        /// <summary>
        /// 지능 수치 계산하여 반환
        /// </summary>
        public UpArrowNotation Intellect
        {
            get
            {
                double elapsedTime = (double)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - LastCalcTime) / 1000f;
                UpArrowNotation intellect = Equation.GetCurrentIntellect(_brainData.intellectEquation, elapsedTime);

                return intellect;
            }
        }

        /// <summary>
        /// 마지막으로 지능이 계산된 시각 반환
        /// </summary>
        public long LastCalcTime { get { return _brainData.LastCalcTime; } }

        /// <summary>
        /// 지능 증폭계수 반환
        /// </summary>
        public UpArrowNotation Multiplier
        {
            get
            {
                return _fullMultiplier;
            }
        }

        /// <summary>
        /// 현재 지능 한계치 반환
        /// </summary>
        public UpArrowNotation CurrentIntellectLimit
        {
            get
            {
                return _currentIntellectLimit;
            }
        }

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

        public bool IsCollision
        {
            get
            {
                return _isCollision;
            }
        }
        #endregion

        public void Init(BrainData data, Dictionary<long, Brain> brainNetwork = null)
        {
            _brainData = data;
            _brainNetwork = brainNetwork;
            if (_brainData.brainType == EBrainType.GUIDEBRAIN)
                gameObject.SetActive(false);
            Set();
            UpdateFullMultiplier();
            UpdateCurrentIntellectLimit();
            SetNumText(Intellect);
            SetMulText(Multiplier);

            _textMul.gameObject.SetActive(_brainData.brainType != EBrainType.GUIDEBRAIN && UserData.TPUpgrades[0].UpgradeCount > 0);
            _isLocked = false;
        }

        public void Set()
        {
            if (_brainData != null)
            {
                _collisionCount = 0;
                switch (_brainData.brainType)
                {
                    case EBrainType.GUIDEBRAIN:
                        transform.localScale = new Vector2(1, 1);
                        _textNum.gameObject.SetActive(false);
                        _textMul.gameObject.SetActive(false);
                        break;
                    case EBrainType.COREBRAIN:
                        _brainData.distance = 0;
                        transform.localScale = new Vector2(2f, 2f);
                        break;
                    case EBrainType.NORMALBRAIN:
                        transform.localScale = new Vector2(1, 1);
                        break;
                }

                transform.position = new Vector2(_brainData.coordinates.x, _brainData.coordinates.y);
                _brainData.LastCalcTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
        }

        public void AdvanceTime(float dt_sec)
        {
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                SetNumText(Intellect);
                SetMulText(Multiplier);
                if (_brainData.brainType == EBrainType.NORMALBRAIN &&
                    !_isLocked &&
                    Intellect > CurrentIntellectLimit)
                {
                    UpdateLockedStatus();
                }
            }
        }

        private async void UpdateLockedStatus()
        {
            _isLocked = true;
            if(await Managers.Network.API_LoadUserData())
            {
                Managers.Notification.PostNotification(ENotiMessage.UPDATE_BRAIN_NETWORK);
            }
        }

        public void Dispose()
        {
            _brainData = null;
            _isLocked = false;
            _collisionCount = 0;
            Managers.Pool.DespawnObject(EPrefabsType.BRAIN, gameObject);

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

        public void UpdateCurrentIntellectLimit()
        {
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();

                inputMap.Add("baseLimit", Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.baseIntellectLimitList)[(int)UserData.TPUpgrades[2].UpgradeCount]);
                inputMap.Add("upgradeCount", new UpArrowNotation(_brainData.limitUpgradeCount));
                inputMap.Add("tpu002", new UpArrowNotation(UserData.TPUpgrades[2].UpgradeCount));

                _currentIntellectLimit = Managers.Definition.CalcEquation(inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainLimitEquation));
            }
            else
            {
                _currentIntellectLimit = new UpArrowNotation();
            }
        }
        public UpArrowNotation GetNextIntellectLimit()
        {
            Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();

            inputMap.Add("baseLimit", Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.baseIntellectLimitList)[(int)UserData.TPUpgrades[2].UpgradeCount]);
            inputMap.Add("upgradeCount", new UpArrowNotation(_brainData.limitUpgradeCount + 1));
            inputMap.Add("tpu002", new UpArrowNotation(UserData.TPUpgrades[2].UpgradeCount));

            return Managers.Definition.CalcEquation(inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainLimitEquation));
        }

        public void UpdateFullMultiplier()
        {
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                _fullMultiplier = GetBaseMultiplier() * GetPassiveMultiplier() * GetUpgradedMultiplier();
            }
            else
            {
                _fullMultiplier = new UpArrowNotation();
            }
        }

        private UpArrowNotation GetBaseMultiplier()
        {
            Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();

            UpArrowNotation baseMultiplier = UserData.MultiplierRewardForReset.Copy();
            if (UserData.TPUpgrades[10].UpgradeCount > 0)       // TPU-010: 브레인 연합 부스터
            {
                inputMap.Clear();
                inputMap.Add("brainCount", new UpArrowNotation(_brainNetwork.Count));
                baseMultiplier.Mul(Managers.Definition.CalcEquation(inputMap, Managers.Definition.GetData<string>(DefinitionKey.multiplierBoostForTPU010)));
            }
            if (UserData.TPUpgrades[26].UpgradeCount > 0)       // TPU-026: NP 부스터
            {
                inputMap.Clear();
                inputMap.Add("NP", UserData.NP);
                baseMultiplier.Mul(Managers.Definition.CalcEquation(inputMap, Managers.Definition.GetData<string>(DefinitionKey.multiplierBoostForTPU026)));
            }
            if (UserData.TPUpgrades[28].UpgradeCount > 0)       // TPU-028: TP 부스터
            {
                inputMap.Clear();
                inputMap.Add("TP", UserData.TP);
                baseMultiplier.Mul(Managers.Definition.CalcEquation(inputMap, Managers.Definition.GetData<string>(DefinitionKey.multiplierBoostForTPU028)));
            }

            return baseMultiplier;
        }

        private UpArrowNotation GetPassiveMultiplier()
        {
            Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();

            UpArrowNotation passiveMultiplier = new UpArrowNotation(1);
            if (UserData.TPUpgrades[7].UpgradeCount > 0)        // TPU-007: 브레인 길드 부스터
            {
                long brainCount = 0;
                foreach (Brain brain in _brainNetwork.Values)
                {
                    if (brain.Distance == Distance)
                    {
                        brainCount++;
                    }
                }
                inputMap.Clear();
                inputMap.Add("brainCount", new UpArrowNotation(brainCount));
                passiveMultiplier.Mul(Managers.Definition.CalcEquation(inputMap, Managers.Definition.GetData<string>(DefinitionKey.multiplierBoostForTPU007)));
            }
            if (UserData.TPUpgrades[15].UpgradeCount > 0)       // TPU-015: 센더 부스터
            {
                inputMap.Clear();
                inputMap.Add("brainCount", new UpArrowNotation(_brainData.senderIds.Count == 0 ? 1 : _brainData.senderIds.Count));
                passiveMultiplier.Mul(Managers.Definition.CalcEquation(inputMap, Managers.Definition.GetData<string>(DefinitionKey.multiplierBoostForTPU015)));
            }
            if (UserData.TPUpgrades[19].UpgradeCount > 0)       // TPU-019: 리시버 부스터
            {
                inputMap.Clear();
                inputMap.Add("brainCount", new UpArrowNotation(_brainData.receiverIds.Count == 0 ? 1 : _brainData.receiverIds.Count));
                passiveMultiplier.Mul(Managers.Definition.CalcEquation(inputMap, Managers.Definition.GetData<string>(DefinitionKey.multiplierBoostForTPU019)));
            }
            if (UserData.TPUpgrades[20].UpgradeCount > 0)       // TPU-020: 센더 부스터 체이닝
            {
                inputMap.Clear();
                long allSenders = CountAllSenders();
                inputMap.Add("brainCount", new UpArrowNotation(allSenders == 0 ? 1 : allSenders));
                passiveMultiplier.Mul(Managers.Definition.CalcEquation(inputMap, Managers.Definition.GetData<string>(DefinitionKey.multiplierBoostForTPU020)));
            }

            return passiveMultiplier;
        }

        private UpArrowNotation GetUpgradedMultiplier()
        {
            Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();

            inputMap.Add("upgradeCount", new UpArrowNotation(_brainData.multiplierUpgradeCount));
            inputMap.Add("tpu001", new UpArrowNotation(UserData.TPUpgrades[1].UpgradeCount));

            return Managers.Definition.CalcEquation(inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainMultiplierEquation));
        }

        private long CountAllSenders()
        {
            long senderCount = 0;

            List<long> visitedList = new List<long>();
            List<long> toVisitList = new List<long>();

            foreach (long id in _brainData.senderIds)
            {
                toVisitList.Add(id);
                senderCount++;
            }

            while (toVisitList.Count > 0)
            {
                long currentID = toVisitList[0];
                visitedList.Add(currentID);
                toVisitList.RemoveAt(0);
                if (_brainNetwork.ContainsKey(currentID))
                {
                    foreach (long id in _brainNetwork[currentID]._brainData.senderIds)
                    {
                        if (!visitedList.Contains(id))
                        {
                            toVisitList.Add(id);
                            senderCount++;
                        }
                    }
                }
            }

            return senderCount;
        }

        private void SetNumText(UpArrowNotation num)
        {
            _textNum.text = num.ToString(ECurrencyType.INTELLECT);
        }

        private void SetMulText(UpArrowNotation num)
        {
            _textMul.text = "x" + num.ToString(ECurrencyType.MULTIPLIER);
        }

        #region EventData
        private void OnMouseDown()
        {
            if (_brainData == null || Managers.Popup.Count > 0)
                return;
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.CLASS_BRAIN, this);
                Managers.Notification.PostNotification(ENotiMessage.MOUSE_DOWN_BRAIN, _sendData);
            }
        }

        private void OnMouseExit()
        {
            if (_brainData == null || Managers.Popup.Count > 0)
                return;
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                Managers.Notification.PostNotification(ENotiMessage.MOUSE_EXIT_BRAIN);
            }
        }

        private void OnMouseUp()
        {
            if (_brainData == null || Managers.Popup.Count > 0)
                return;
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                Managers.Notification.PostNotification(ENotiMessage.MOUSE_UP_BRAIN);
            }
        }

        private void OnMouseEnter()
        {
            if (_brainData == null || Managers.Popup.Count > 0)
                return;
            if (_brainData.brainType != EBrainType.GUIDEBRAIN)
            {
                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.CLASS_BRAIN, this);
                Managers.Notification.PostNotification(ENotiMessage.MOUSE_ENTER_BRAIN, _sendData);
            }
        }

        private int _collisionCount = 0;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_brainData == null || Managers.Popup.Count > 0)
                return;
            _collisionCount++;
            _isCollision = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_brainData == null || Managers.Popup.Count > 0)
                return;

            _collisionCount--;
            if(_collisionCount == 0)
            {
                _isCollision = false;
            }
        }
        #endregion
    }

}