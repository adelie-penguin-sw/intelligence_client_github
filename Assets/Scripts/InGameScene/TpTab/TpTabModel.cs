using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace TpTab
{
    /// <summary>
    /// TP 업그레이드 탭의 데이터를 가지고 있는 Model 클래스
    /// </summary>
    [Serializable]
    public class TpTabModel
    {
        public Dictionary<int,TpUpgradeDefinition> TpUpgradeDefinition
        {
            get
            {
                return Managers.Definition.GetDatas<Dictionary<int, TpUpgradeDefinition>>(EDefType.TP_UPGRADE);
            }
        }
    }
}
