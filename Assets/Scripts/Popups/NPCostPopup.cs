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

        public override void Set()
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
                    }
                    else
                    {
                        Vector2 curpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        float physicalDistance = curpos.magnitude;

                        _inputMap.Add("physicalDistance", new UpArrowNotation(physicalDistance));
                        _inputMap.Add("pastBrainGenCount", new UpArrowNotation(UserData.PastBrainGenCount));
                        _inputMap.Add("tpu01", new UpArrowNotation(UserData.TPUpgradeCounts[1]));

                        if(Managers.Definition["brainGeneratingCostEquation"]!=null)
                            _costText.text = "Cost: " + Managers.Definition.CalcEquation(_inputMap, (string)Managers.Definition["brainGeneratingCostEquation"]).ToString() + " NP";
                    }
                    break;

                case ENPCostType.CHNNL_GEN:
                    if (_receiverBrain == null)
                    {
                        _costText.text = "Select a brain to connect";
                    }
                    else
                    {
                        _inputMap.Add("receiverDistance", new UpArrowNotation(_receiverBrain.Distance));
                        _inputMap.Add("senderIntellect", _senderBrain.Intellect);
                        _inputMap.Add("tpu02", new UpArrowNotation(UserData.TPUpgradeCounts[2]));

                        _costText.text = "Cost: " + Managers.Definition.CalcEquation(_inputMap, (string)Managers.Definition["channelGeneratingCostEquation"]).ToString() + " NP";
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
