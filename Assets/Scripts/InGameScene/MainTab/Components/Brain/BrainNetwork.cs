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

        [ShowInInspector] private Dictionary<long, Brain> _brainNetWork = new Dictionary<long, Brain>();
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
        }

        public void Set(SingleNetworkWrapper wrapper)
        {
            UpdateBrainNetwork(wrapper);
        }

        private float _elapseTime = 0f;
        private const float _countingTime = 1f;
        public void AdvanceTime(float dt_sec)
        {

            _elapseTime += dt_sec;
            if (_elapseTime >= _countingTime)
            {
                _elapseTime = 0f;

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

        public void UpdateBrainNetwork(SingleNetworkWrapper wrapper)
        {
            ClearNetwork();

            foreach (var id in wrapper.ansEquationsDic.Keys)
            {
                BrainData data = wrapper.GetBrainDataForID(id);
                if (data != null)
                {
                    Brain brain = PoolManager.Instance.GrabPrefabs(EPrefabsType.BRAIN, "Brain", _brainLayer).GetComponent<Brain>();
                    brain.Init(data);
                    _brainNetWork.Add(id, brain);
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