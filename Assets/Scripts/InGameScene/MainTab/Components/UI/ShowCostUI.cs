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
            this.gameObject.SetActive(true);
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
                            _inputMap.Add("tpu021", new UpArrowNotation(UserData.TPUpgrades[21].UpgradeCount));

                            UpArrowNotation brainGenCost = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainGeneratingCostEquation));

                            _costText.text = "Cost: " + brainGenCost.ToString() + " NP";
                            if (brainGenCost <= UserData.NP)
                                _costText.color = new Color32(255, 255, 255, 255);
                            else
                                _costText.color = new Color32(255, 0, 0, 255);
                        }
                        break;

                    case ENPCostType.CHNNL_GEN:
                        long strctureLevel = UserData.TPUpgrades[18].UpgradeCount + UserData.TPUpgrades[24].UpgradeCount;

                        if (_senderBrain.ID == 0)                                                       // ?????????? ?????? ???? ?? ????
                        {
                            _costText.text = "Cannot generate a channel from Core Brain";
                            _costText.color = new Color32(255, 0, 0, 255);
                        }
                        else if (strctureLevel == 0 && _senderBrain.ReceiverIdList.Count > 0)           // ?????? ?? ?????? ???????? ?? ???? (???????? ???? ???? ?????? ????)
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
                        else if (_receiverBrain.Distance >= UserData.MaxDepth)    // ?????? ?????? ?????? ???????? ?????? ?? ????
                        {
                            _costText.text = $"Cannot make a chain longer than {UserData.MaxDepth}";
                            _costText.color = new Color32(255, 0, 0, 255);
                        }
                        else if (strctureLevel == 1 && !IsReceiverDistanceMatch())
                        {
                            _costText.text = "All receivers' distance must be identical";
                            _costText.color = new Color32(255, 0, 0, 255);
                        }
                        else if (strctureLevel == 2 && ContainsLoop(_senderBrain.BrainNetwork, _senderBrain.ID, _receiverBrain.ID))
                        {
                            _costText.text = "Cannot generate a loop";
                            _costText.color = new Color32(255, 0, 0, 255);
                        }
                        else if (strctureLevel > 0 && _senderBrain.ReceiverIdList.Contains(_receiverBrain.ID))
                        {
                            _costText.text = "Connection already exists";
                            _costText.color = new Color32(255, 0, 0, 255);
                        }
                        else                                                                            // ?????? ???? ????. NP???? ????
                        {
                            _inputMap.Add("receiverDistance", new UpArrowNotation(_receiverBrain.Distance));
                            _inputMap.Add("senderIntellect", _senderBrain.Intellect);
                            _inputMap.Add("tpu013", new UpArrowNotation(UserData.TPUpgrades[13].UpgradeCount));
                            _inputMap.Add("tpu023", new UpArrowNotation(UserData.TPUpgrades[23].UpgradeCount));

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

        private bool IsReceiverDistanceMatch()
        {
            if (_senderBrain.ReceiverIdList.Count == 0)
            {
                return true;
            }

            foreach (long id in _senderBrain.ReceiverIdList)
            {
                if (_receiverBrain.Distance != _senderBrain.BrainNetwork[id].Distance)
                {
                    return false;
                }
            }

            return true;
        }

        private class BrainConnection
        {
            public HashSet<long> senderBrains;
            public HashSet<long> receiverBrains;

            public BrainConnection(HashSet<long> senderBrains, HashSet<long> receiverBrains)
            {
                this.senderBrains = new HashSet<long>(senderBrains);
                this.receiverBrains = new HashSet<long>(receiverBrains);
            }
        }

        private Dictionary<long, BrainConnection> ConvertToBrainConnection(Dictionary<long, Brain> brainNetwork)
        {
            Dictionary<long, BrainConnection> result = new Dictionary<long, BrainConnection>();
            foreach (long brain in brainNetwork.Keys)
            {
                result.Add(brain, new BrainConnection(brainNetwork[brain].SenderIdList, brainNetwork[brain].ReceiverIdList));
            }
            return result;
        }

        private bool ContainsLoop(Dictionary<long, Brain> brainNetwork, long senderID, long receiverID)
        {
            // 새 연결관계가 추가됐다고 가정
            Dictionary<long, BrainConnection> allConnections = ConvertToBrainConnection(brainNetwork);
            allConnections[senderID].receiverBrains.Add(receiverID);
            allConnections[receiverID].senderBrains.Add(senderID);

            List<long> visited = new List<long>();
            Stack<long> toVisit = new Stack<long>();
            visited.Add(receiverID);
            foreach (long brain in allConnections[receiverID].receiverBrains)
            {
                toVisit.Push(brain);
            }

            // DFS
            long currentBrain;
            while (toVisit.Count > 0)
            {
                currentBrain = toVisit.Pop();
                visited.Add(currentBrain);
                foreach (long brain in allConnections[currentBrain].receiverBrains)
                {
                    if (brain == senderID)
                    {
                        return true;
                    }
                    if (!visited.Contains(brain) && !toVisit.Contains(brain))
                    {
                        toVisit.Push(brain);
                    }
                }
            }

            return false;
        }
    }

    public enum ENPCostType
    {
        BRAIN_GEN,
        CHNNL_GEN,
    }
}