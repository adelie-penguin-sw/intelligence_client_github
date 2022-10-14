using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TpTab
{
    public class TpTabView : MonoBehaviour
    {
        [SerializeField] private TpUpgradeItem[] _tpUpgradeItems;

        public void SetUpgradeItem(int index, string name,string num, string effect, string costEquation)
        {
            if(index >= 0 && index < _tpUpgradeItems.Length && _tpUpgradeItems[index] != null)
            {
                _tpUpgradeItems[index].SetNameText(name);
                _tpUpgradeItems[index].SetNumText(num);
                _tpUpgradeItems[index].SetEffectText(effect);
                _tpUpgradeItems[index].SetCostText(index, costEquation);
            }
        }
    }
}
