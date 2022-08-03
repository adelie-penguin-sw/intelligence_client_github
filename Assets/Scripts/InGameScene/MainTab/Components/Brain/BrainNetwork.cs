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
        [SerializeField] private List<Channel> _channelList = new List<Channel>();
        //[SerializeField] private List<Brain> _removeList = new List<Brain>();
        //[SerializeField] private List<Brain> _addList = new List<Brain>();


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
        }

        public void Set(SingleNetworkWrapper wrapper)
        {
            UpdateBrainNetwork(wrapper);
        }

        private float _elapseTime = 0f;
        private const float _countingTime = 1f;
        public void AdvanceTime(float dt_sec)
        {
            //if (_removeList.Count > 0)
            //{
            //    foreach (var brain in _removeList)
            //    {
            //        if (_brainNetWork.ContainsKey(brain.ID))
            //        {
            //            _brainNetWork.Remove(brain.ID);
            //            brain.Dispose();
            //        }
            //        else
            //        {
            //            Debug.LogError("존재하지 않는 브레인을 지우려고 시도");
            //        }
            //    }
            //    ClearAndDrawChannel();
            //    _removeList.Clear();
            //}

            //foreach (var brain in _addList)
            //{
            //    if (_brainNetWork.ContainsKey(brain.ID))
            //        Debug.LogError("이미 존재하는 ID를 가진 브레인을 추가 시도");
            //    else
            //        _brainNetWork.Add(brain.ID, brain);
            //}
            //_addList.Clear();

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

        private void ClearNetwork()
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
       
        public void Dispose()
        {
            ClearNetwork();
        }

        private void UpdateBrainNetwork(SingleNetworkWrapper wrapper)
        {
            ClearNetwork();

            foreach (var id in wrapper.ansEquationsDic.Keys)
            {
                BrainData data;
                if (id == 1)
                {
                    data = new BrainData(id, new UpArrowNotation(wrapper.ansEquationsDic[id].ansEquation), EBrainType.MAINBRAIN);
                }
                else
                {
                    data = new BrainData(id, new UpArrowNotation(wrapper.ansEquationsDic[id].ansEquation), EBrainType.NORMALBRAIN);
                }

                if (data != null)
                {
                    Brain brain = PoolManager.Instance.GrabPrefabs(EPrefabsType.BRAIN, "Brain", _brainLayer).GetComponent<Brain>();
                    brain.transform.position = new Vector2(wrapper.coordinatesDic[id].x, wrapper.coordinatesDic[id].y);
                    brain.Init(data);
                    _brainNetWork.Add(id, brain);
                    brain.Set();

                    foreach (var receiverId in wrapper.structuresDic[id].structure)
                    {
                        brain.AddReceiver(receiverId);
                    }
                }
            }

            ClearAndDrawChannel();
        }


        /// <summary>
        /// 브레인 추가
        /// </summary>
        /// <param name="brain">추가할 브레인</param>
        public void AddBrain(Brain brain)
        {
            //_addList.Add(brain);
            ClearAndDrawChannel();
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
        private void ClearAndDrawChannel()
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