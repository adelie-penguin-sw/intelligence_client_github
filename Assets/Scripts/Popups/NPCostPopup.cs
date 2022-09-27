using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InGame
{
    /// <summary>
    /// 브레인 및 채널 생성할 때 NP비용 표시해주는 역할 <br />
    /// NP가 모자랄 때 또는 다른 이유로 생성이 불가할 때에는 그것까지 표시해주면 좋을듯 <br />
    /// </summary>
    public class NPCostPopup : PopupBase
    {
        [SerializeField] private TextMeshProUGUI _costText;

        private ENPCostType _costType;
        private Dictionary<string, UpArrowNotation> _inputMap = new Dictionary<string, UpArrowNotation>();

        private bool _isCollision;

        private MainTab.Brain _senderBrain = null;
        private MainTab.Brain _receiverBrain = null;

        public void Init(ENPCostType type)
        {
            base.Init();
            _costType = type;
        }

        public void Set()
        {
            base.Set();
        }
        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            _inputMap.Clear();
            switch (_costType)
            {
                case ENPCostType.BRAIN_GEN:
                    if (_isCollision)
                    {
                        _costText.text = "Too close to other brain";
                        _costText.color = new Color32(255, 0, 0, 255);
                    }
                    else
                    {
                        Vector2 curpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        float physicalDistance = curpos.magnitude;

                        _inputMap.Add("physicalDistance", new UpArrowNotation(physicalDistance));
                        _inputMap.Add("pastBrainGenCount", new UpArrowNotation(UserData.PastBrainGenCount));
                        _inputMap.Add("tpu01", new UpArrowNotation(UserData.TPUpgradeCounts[1]));

                        if (Managers.Definition["brainGeneratingCostEquation"] != null)
                        {
                            UpArrowNotation brainGenCost = Managers.Definition.CalcEquation(_inputMap, (string)Managers.Definition["brainGeneratingCostEquation"]);

                            _costText.text = "Cost: " + brainGenCost.ToString() + " NP";
                            if (brainGenCost <= UserData.NP)
                                _costText.color = new Color32(255, 255, 255, 255);
                            else
                                _costText.color = new Color32(255, 0, 0, 255);
                        }
                            
                    }
                    break;

                case ENPCostType.CHNNL_GEN:
                    if (_senderBrain.ID == 0)                                                       // 코어에서는 채널이 나올 수 없다
                    {
                        _costText.text = "Cannot generate a channel from Core Brain";
                        _costText.color = new Color32(255, 0, 0, 255);
                    }
                    else if (_senderBrain.ReceiverIdList.Count > 0)                                 // 채널을 두 개이상 출발시킬 수 없다 (네트워크 레벨 조건 추가로 필요)
                    {
                        _costText.text = "This brain already has a channel";
                        _costText.color = new Color32(255, 0, 0, 255);
                    }
                    else if (_receiverBrain == null)                                                // 아직 브레인을 선택하지 않음
                    {
                        _costText.text = "Select a brain to connect";
                        _costText.color = new Color32(255, 255, 255, 255);
                    }
                    else if (_receiverBrain.ID != 0 && _receiverBrain.ReceiverIdList.Count == 0)    // 코어를 제외한 고립된 브레인에 연결할 수 없다
                    {
                        _costText.text = "Cannot connect to an isolated brain";
                        _costText.color = new Color32(255, 0, 0, 255);
                    }
                    else                                                                            // 구조적 문제 없음. NP비용 표시
                    {
                        _inputMap.Add("receiverDistance", new UpArrowNotation(_receiverBrain.Distance));
                        _inputMap.Add("senderIntellect", _senderBrain.Intellect);
                        _inputMap.Add("tpu02", new UpArrowNotation(UserData.TPUpgradeCounts[2]));

                        if (Managers.Definition["channelGeneratingCostEquation"] != null)
                        {
                            UpArrowNotation channelGenCost = Managers.Definition.CalcEquation(_inputMap, (string)Managers.Definition["channelGeneratingCostEquation"]);

                            _costText.text = "Cost: " + channelGenCost.ToString() + " NP";
                            if (channelGenCost <= UserData.NP)
                                _costText.color = new Color32(255, 255, 255, 255);
                            else
                                _costText.color = new Color32(255, 0, 0, 255);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public void SetBrain(MainTab.Brain sender, MainTab.Brain receiver)
        {
            _senderBrain = sender;
            _receiverBrain = receiver;
        }

        public void SetCollisoinState(bool state)
        {
            _isCollision = state;
        }
    }
}

public enum ENPCostType
{
    BRAIN_GEN,
    CHNNL_GEN,
}
