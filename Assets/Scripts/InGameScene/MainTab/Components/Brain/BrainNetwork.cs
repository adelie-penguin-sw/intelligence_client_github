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
        private List<Brain> _removeList = new List<Brain>();
        private List<Brain> _addList = new List<Brain>();

        [SerializeField] private List<Channel> _channelList = new List<Channel>();
        public void Init(Transform brainLayer)
        {
            _brainLayer = brainLayer;
            //���⼭ ���� �극�� ������ �޾ƿͼ� �ʱ�ȭ ���ָ� �ɵ�.
            //���� ������ ������ �극���� ������ ��� -> ex ) ���� �����͸� �޾ƿ� ��

            //Init��ų MainBrain ������ ���� ����
            BrainData data = new BrainData(1, 1, EBrainType.MAINBRAIN);

            //�ϴ� �ӽ÷� ������ MainBrain�����ǰ� ����
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
            foreach (var brain in _removeList)
            {
                if (_brainNetWork.ContainsKey(brain.ID))
                    _brainNetWork.Remove(brain.ID);
                else
                    Debug.LogError("�������� �ʴ� �극���� ������� �õ�");
            }
            _removeList.Clear();

            foreach (var brain in _addList)
            {
                if (_brainNetWork.ContainsKey(brain.ID))
                    Debug.LogError("�̹� �����ϴ� ID�� ���� �극���� �߰� �õ�");
                else
                    _brainNetWork.Add(brain.ID, brain);
            }
            _addList.Clear();

            _elapseTime += dt_sec;
            if(_elapseTime>= _countingTime)
            {
                _elapseTime = 0f;

                foreach (var brain in _brainNetWork.Values)
                {
                    foreach (var sender in brain.SenderIdList)
                    {
                        if (_brainNetWork.ContainsKey(sender))
                            brain.StandByIntellect += _brainNetWork[sender].Intellect;
                    }
                }

                foreach (var brain in _brainNetWork.Values)
                {
                    brain.AdvanceTime(dt_sec);
                }
            }
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
        /// �극�� �߰�
        /// </summary>
        /// <param name="brain">�߰��� �극��</param>
        public void AddBrain(Brain brain)
        {
            _addList.Add(brain);
            ClearAndDrawChannel();
        }

        /// <summary>
        /// �극�� ����
        /// </summary>
        /// <param name="brain">������ �극��</param>
        public void RemoveBrain(Brain brain)
        {
            _removeList.Add(brain);
            ClearAndDrawChannel();
        }
     
        /// <summary>
        /// ���� �극�� ��Ʈ��ũ���� �ش� ���踦 ������ ���� ���� ��� �ش� ���踦 �߰�
        /// </summary>
        /// <param name="relation">�߰���</param>
        /// <returns>�߰� ������ true, ���н� false</returns>
        public bool AddBrainRelation(BrainRelation relation)
        {
            if (_brainNetWork[relation.receiverId].Type != EBrainType.MAINBRAIN)
            {
                if (_brainNetWork[relation.receiverId].Distance == -1)
                {
                    return false;
                }
            }

            if( ! _brainNetWork[relation.receiverId].IsContainsSender(relation.senderId) &&
                ! _brainNetWork[relation.senderId].IsContainsReceiver(relation.receiverId))
            {
                _brainNetWork[relation.receiverId].AddSender(relation.senderId);
                _brainNetWork[relation.senderId].AddReceiver(relation.receiverId);
                _brainNetWork[relation.senderId].Distance = _brainNetWork[relation.receiverId].Distance + 1;
                 ClearAndDrawChannel();
                return true;
            }
            return false;
        }

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
                    Debug.LogError("=========================================");
                    Debug.LogError(brain.ID);
                    Debug.LogError(receiver);
                    Debug.LogError("=========================================");
                    channel.Init(EChannelType.NORMAL, _brainNetWork[brain.ID].transform, _brainNetWork[receiver].transform );
                    _channelList.Add(channel);
                }
            }
        }
    }

    /// <summary>
    /// �극�� ���踦 ǥ���ϱ� ���� ����ϴ� ����ü
    /// </summary>
    public struct BrainRelation
    {
        public int senderId;
        public int receiverId;
        public BrainRelation(int sender,int receiver)
        {
            senderId = sender;
            receiverId = receiver;
        }
    }

}