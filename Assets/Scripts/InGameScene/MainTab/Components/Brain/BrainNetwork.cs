using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainTab
{
    public class BrainNetwork
    {
        private Transform _brainLayer;

        private Dictionary<int, Brain> _brainNetWork = new Dictionary<int, Brain>();
        private List<Brain> _removeList = new List<Brain>();
        private List<Brain> _addList = new List<Brain>();
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
            foreach (var brain in _brainNetWork.Values)
            {
                brain.Dispose();
            }

            _brainNetWork.Clear();
        }

        public void AddBrain(Brain brain)
        {
            _addList.Add(brain);
        }

        public void RemoveBrain(Brain brain)
        {
            _removeList.Add(brain);
        }
    }

}