using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
namespace MainTab
{
    [Serializable]
    public class BrainNetwork
    {
        private Transform _brainLayer;

        [ShowInInspector] private Dictionary<int, Brain> _brainNetWork = new Dictionary<int, Brain>();
        [SerializeField] private List<Brain> _removeList = new List<Brain>();
        [SerializeField] private List<Brain> _addList = new List<Brain>();

        [SerializeField] private List<Channel> _channelList = new List<Channel>();

        public Brain MainBrain
        {
            get
            {
                return _brainNetWork[1];
            }
        }

        public void Init(Transform brainLayer)
        {
            _brainLayer = brainLayer;
            //여기서 최초 브레인 데이터 받아와서 초기화 해주면 될듯.
            //만약 기존에 생성된 브레인이 존재할 경우 -> ex ) 서버 데이터를 받아온 후

            //Init시킬 MainBrain 데이터 임의 생성
            BrainData data = new BrainData(1, new UpArrowNotation(1), EBrainType.MAINBRAIN);

            //일단 임시로 무조건 MainBrain생성되게 구현
            Brain brain = PoolManager.Instance.GrabPrefabs(EPrefabsType.BRAIN, "Brain", _brainLayer)
                .GetComponent<Brain>();
            brain.Init(data);
            _brainNetWork.Add(1, brain);
        }

        public void Set()
        {
            foreach (var brain in _brainNetWork.Values)
            {
                brain.Set();
            }
        }
         private float _elapseTime = 0f;
        private const float _countingTime = 1f;
        public void AdvanceTime(float dt_sec)
        {
            if (_removeList.Count > 0)
            {
                foreach (var brain in _removeList)
                {
                    if (_brainNetWork.ContainsKey(brain.ID))
                    {
                        _brainNetWork.Remove(brain.ID);
                        brain.Dispose();
                    }
                    else
                    {
                        Debug.LogError("존재하지 않는 브레인을 지우려고 시도");
                    }
                }
                ClearAndDrawChannel();
                _removeList.Clear();
            }

            foreach (var brain in _addList)
            {
                if (_brainNetWork.ContainsKey(brain.ID))
                    Debug.LogError("이미 존재하는 ID를 가진 브레인을 추가 시도");
                else
                    _brainNetWork.Add(brain.ID, brain);
            }
            _addList.Clear();

            _elapseTime += dt_sec;
            if (_elapseTime >= _countingTime)
            {
                _elapseTime = 0f;

                foreach (var brain in _brainNetWork.Values)
                {
                    foreach (var sender in brain.SenderIdList)
                    {
                        if (_brainNetWork.ContainsKey(sender))
                            brain.StandByIntellect.Add(_brainNetWork[sender].Intellect);
                    }
                }

                foreach (var brain in _brainNetWork.Values)
                {
                    brain.AdvanceTime(dt_sec);
                }
            }

            CheckCompleteExperiment();
        }

       
        public void Dispose()
        {
            foreach (var channel in _channelList)
            {
                channel.Dispose();
            }
            foreach (var brain in _brainNetWork.Values)
            {
                brain.Dispose();
            }

            _channelList.Clear();
            _brainNetWork.Clear();
        }

        /// <summary>
        /// 브레인 추가
        /// </summary>
        /// <param name="brain">추가할 브레인</param>
        public void AddBrain(Brain brain)
        {
            _addList.Add(brain);
            ClearAndDrawChannel();
        }

        /// <summary>
        /// 브레인 삭제
        /// </summary>
        /// <param name="brain">지워질 브레인</param>
        /// <returns>얻을 NP반환</returns>
        public int RemoveBrain(Brain brain)
        {
            //_removeList.Add(brain);
            // ClearAndDrawChannel();
            Queue<Brain> brainQ = new Queue<Brain>();
            HashSet<int> visitID = new HashSet<int>();
            brainQ.Enqueue(brain);
            while (brainQ.Count > 0)
            {
                Brain curBrain = brainQ.Dequeue();
                visitID.Add(curBrain.ID);
                _removeList.Add(curBrain);

                foreach (var receiver in curBrain.ReceiverIdList) //현재 브레인에게 지능을 받는 브레인에게 내 브레인 정보를 지워준다
                {
                    if (_brainNetWork[receiver].SenderIdList.Contains(curBrain.ID))
                        _brainNetWork[receiver].SenderIdList.Remove(curBrain.ID);
                }

                foreach (var sender in curBrain.SenderIdList) // 현재 브레인에게 지능을 보내는 브레인 아이디 모음을 불러옴
                {
                    if (!visitID.Contains(_brainNetWork[sender].ID)) // 보내는 브레인을 아직 방문하지 않은 브레인인 경우
                    {
                        //보내는 브레인이 현재 브레인 아이디를 리시브로 가지고있는지 더블체크
                        if (_brainNetWork[sender].ReceiverIdList.Contains(curBrain.ID))
                        {
                            //존재하면 receiverIdList에서 현재 브레인 id를 지워준다.
                            _brainNetWork[sender].ReceiverIdList.Remove(curBrain.ID);
                        }

                        //지우고 나서 보내는 브레인이 전송역할을 하지않는 경우
                        if (_brainNetWork[sender].ReceiverIdList.Count <= 0)
                        {
                            brainQ.Enqueue(_brainNetWork[sender]);
                        }
                    }
                }
            }

            int returnNP = 0;
            //지워지는 브레인의 결과에 따라 얻는 NP 업데이트
            foreach (var remove in _removeList)
            {
                returnNP += (int)remove.Intellect.CalculateFull(); //임시로 1:1로 반환해준다.
            }
            return returnNP;
        }

        /// <summary>
        /// 현재 브레인 네트워크에서 해당 관계를 가지고 있지 않을 경우 해당 관계를 추가
        /// </summary>
        /// <param name="relation">추가할</param>
        /// <returns>추가 성공시 true, 실패시 false</returns>
        public bool AddBrainRelation(BrainRelation relation)
        {
            if (_brainNetWork[relation.receiverId].Type != EBrainType.MAINBRAIN)
            {
                if (_brainNetWork[relation.receiverId].Distance == -1)
                {
                    return false;
                }
            }

            if (!_brainNetWork[relation.receiverId].IsContainsSender(relation.senderId) &&
                !_brainNetWork[relation.senderId].IsContainsReceiver(relation.receiverId))
            {
                _brainNetWork[relation.receiverId].AddSender(relation.senderId);
                _brainNetWork[relation.senderId].AddReceiver(relation.receiverId);
                _brainNetWork[relation.senderId].Distance = _brainNetWork[relation.receiverId].Distance + 1;
                ClearAndDrawChannel();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 현재 메인 브레인의 연구달성 여부를 update 시켜줌
        /// </summary>
        public void CheckCompleteExperiment()
        {
            if (_brainNetWork.ContainsKey(1) &&
                _brainNetWork[1].Type == EBrainType.MAINBRAIN &&
                _brainNetWork[1].Intellect >= InGame.InGameManager.experimentGoal)
            {
                InGame.InGameManager.IsCompleteExp = true;
                NotificationManager.Instance.PostNotification(ENotiMessage.EXPERIMENT_COMPLETE);
            }
            else
            {
                InGame.InGameManager.IsCompleteExp = false;
            }
        }

        /// <summary>
        /// 현재 채널 리스트에 있는 모든 채널을 지우고 다시 재생성
        /// </summary>
        public void ClearAndDrawChannel()
        {
            foreach (var channel in _channelList)
            {
                channel.Dispose();
            }
            _channelList.Clear();

            foreach (var brain in _brainNetWork.Values)
            {
                foreach (var receiver in brain.ReceiverIdList)
                {
                    var channel =
                        PoolManager.Instance.GrabPrefabs(EPrefabsType.CHANNEL, "Channel", _brainLayer).GetComponent<Channel>();
                    channel.Init(EChannelType.NORMAL, brain.transform, _brainNetWork[receiver].transform);
                    _channelList.Add(channel);
                }
            }
        }
    }

    /// <summary>
    /// 브레인 관계를 표시하기 위해 사용하는 구조체
    /// </summary>
    public struct BrainRelation
    {
        public int senderId;
        public int receiverId;
        public BrainRelation(int sender, int receiver)
        {
            senderId = sender;
            receiverId = receiver;
        }
    }

}