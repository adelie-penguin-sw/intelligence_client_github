using System.Collections;
using System.Collections.Generic;
using MainTab;
using UnityEngine;
using UnityEngine.UI;
namespace TpTab
{
    public class TpTabView : MonoBehaviour
    {
        [SerializeField] public Dictionary<int, TpUpgradeItem> _tpUpgradeItems = new Dictionary<int, TpUpgradeItem>();
        [SerializeField] private Transform _tpuTreeLayer;

        public void SetUpgradeItem(int index, string name, string num, string effect, string effectValue, string effectValueEquation, string costEquation, int currentLevel, int maxLevel, List<List<int>> unlockRequirement)
        {
            if(index >= 0)
            {
                TpUpgradeItem item;
                if (index >= _tpUpgradeItems.Count)
                {
                    item = Managers.Pool.GrabPrefabs(EPrefabsType.TP_UPGRADE_ITEM, "TpUpgradeItem", _tpuTreeLayer).GetComponent<TpUpgradeItem>();
                    _tpUpgradeItems.Add(index, item);
                }
                else
                {
                    item = _tpUpgradeItems[index];
                }

                if (index == 17)    // ASN 제외
                {
                    item.gameObject.SetActive(false);
                }
                if (index == 14)    // NP 탭 획득 제외
                {
                    item.gameObject.SetActive(false);
                }

                item.UpdateAllData(index, costEquation, currentLevel, maxLevel, unlockRequirement);
                item.SetImage(index);
                item.SetCostText();
                item.SetUpgradeLevelText();
                item.SetRequirementText();
                item.SetNameText(name);
                item.SetNumText(num);
                item.SetEffectText(effect);
                item.SetEffectValueText(effectValue);
                item.SetEffectValueEquation(effectValueEquation);
            }
        }
    }
}
