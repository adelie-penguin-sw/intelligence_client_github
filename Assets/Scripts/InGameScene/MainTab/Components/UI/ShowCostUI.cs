using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MainTab
{
    public class ShowCostUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _costText;

        private ENPCostType _costType;
        private Dictionary<string, UpArrowNotation> _inputMap = new Dictionary<string, UpArrowNotation>();

        private bool _isCollision;

        private Brain _senderBrain = null;
        private Brain _receiverBrain = null;

        //public void Init(ENPCostType type)
        //{
        //    Set(type);
        //}

        public void Set(ENPCostType type)
        {
            _costType = type;
            gameObject.SetActive(true);
        }
        public void AdvanceTime(float dt_sec)
        {
            if (gameObject.activeSelf)
            {
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
                            _inputMap.Add("pastBrainGenCount", new UpArrowNotation(UserData.TotalBrainGenCount));
                            _inputMap.Add("tpu011", new UpArrowNotation(UserData.TPUpgrades[11].UpgradeCount));

                            UpArrowNotation brainGenCost = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainGeneratingCostEquation));

                            _costText.text = "Cost: " + brainGenCost.ToString() + " NP";
                            if (brainGenCost <= UserData.NP)
                                _costText.color = new Color32(255, 255, 255, 255);
                            else
                                _costText.color = new Color32(255, 0, 0, 255);
                        }
                        break;

                    case ENPCostType.CHNNL_GEN:
                        if (_senderBrain.ID == 0)                                                       // ?????????? ?????? ???? ?? ????
                        {
                            _costText.text = "Cannot generate a channel from Core Brain";
                            _costText.color = new Color32(255, 0, 0, 255);
                        }
                        else if (_senderBrain.ReceiverIdList.Count > 0)                                 // ?????? ?? ?????? ???????? ?? ???? (???????? ???? ???? ?????? ????)
                        {
                            _costText.text = "This brain already has a channel";
                            _costText.color = new Color32(255, 0, 0, 255);
                        }
                        else if (_receiverBrain == null)                                                // ???? ???????? ???????? ????
                        {
                            _costText.text = "Select a brain to connect";
                            _costText.color = new Color32(255, 255, 255, 255);
                        }
                        else if (_receiverBrain.ID != 0 && _receiverBrain.ReceiverIdList.Count == 0)    // ?????? ?????? ?????? ???????? ?????? ?? ????
                        {
                            _costText.text = "Cannot connect to an isolated brain";
                            _costText.color = new Color32(255, 0, 0, 255);
                        }
                        else                                                                            // ?????? ???? ????. NP???? ????
                        {
                            _inputMap.Add("receiverDistance", new UpArrowNotation(_receiverBrain.Distance));
                            _inputMap.Add("senderIntellect", _senderBrain.Intellect);
                            _inputMap.Add("tpu013", new UpArrowNotation(UserData.TPUpgrades[13].UpgradeCount));

                            UpArrowNotation channelGenCost = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.channelGeneratingCostEquation));

                            _costText.text = "Cost: " + channelGenCost.ToString() + " NP";
                            if (channelGenCost <= UserData.NP)
                                _costText.color = new Color32(255, 255, 255, 255);
                            else
                                _costText.color = new Color32(255, 0, 0, 255);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public void Dispose()
        {
            _costText.text = "";
            gameObject.SetActive(false);
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

    public enum ENPCostType
    {
        BRAIN_GEN,
        CHNNL_GEN,
    }
}