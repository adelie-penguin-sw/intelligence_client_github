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
            //여기서 최초 브레인 데이터 받아와서 초기화 해주면 될듯.
            //만약 기존에 생성된 브레인이 존재할 경우 -> ex ) 서버 데이터를 받아온 후

            //Init시킬 MainBrain 데이터 임의 생성
            BrainData data = new BrainData(1, 1, EBrainType.MAINBRAIN);

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
            foreach (var brain in _removeList)
            {
                if (_brainNetWork.ContainsKey(brain.ID))
                    _brainNetWork.Remove(brain.ID);
                else
                    Debug.LogError("존재하지 않는 브레인을 지우려고 시도");
            }
            _removeList.Clear();

            foreach (var brain in _addList)
            {
                if (_brainNetWork.ContainsKey(brain.ID))
                    Debug.LogError("이미 존재하는 ID를 가진 브레인을 추가 시도");
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