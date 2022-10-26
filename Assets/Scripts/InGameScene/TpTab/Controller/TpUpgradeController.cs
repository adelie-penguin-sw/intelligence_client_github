using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainTab;
using UnityEngine;
namespace TpTab
{
    public class TpUpgradeController : BaseTabController<TpTabApplication>
    {
        private Dictionary<int, TpUpgradeDefinition> _definition;
        private InGame.TpUpgradePopup _popup;
        private TpUpgradeItem _currentItem;

        public override void Init(TpTabApplication app)
        {
            base.Init(app);
            _definition = _app.TpTabModel.TpUpgradeDefinition;
            SetAllUpgradeItems();

            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_TPUPGRADE_ICON);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_TPUPGRADE);
        }

        public override void Set()
        {
            base.Set();
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
        }

        public override void Dispose()
        {
            base.Dispose();
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_TPUPGRADE_ICON);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_TPUPGRADE);
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
        }

        private void OnNotification(Notification noti)
        {
            switch (noti.msg)
            {
                case ENotiMessage.ONCLICK_TPUPGRADE_ICON:
                    _currentItem = (TpUpgradeItem)noti.data[EDataParamKey.CLASS_TPU_ITEM];
                    _popup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "TpUpgradePopup", PopupType.NORMAL)
                                        .GetComponent<InGame.TpUpgradePopup>();
                    _popup.Init(_currentItem);
                    break;
                case ENotiMessage.ONCLICK_TPUPGRADE:
                    _currentItem = (TpUpgradeItem)noti.data[EDataParamKey.CLASS_TPU_ITEM];
                    SetAllUpgradeItems();
                    UpdatePopup();
                    break;
            }
        }

        private void SetAllUpgradeItems()
        {
            foreach (var id in _definition.Keys)
            {
                _app.TpTabView.SetUpgradeItem(
                    id,
                    _definition[id].nameText,
                    string.Format("#{0}", id),
                    _definition[id].effectText,
                    _definition[id].costEquation,
                    (int)UserData.TPUpgrades[id].UpgradeCount,
                    _definition[id].maxLevel,
                    _definition[id].unlockRequirement
                    );
            }
        }

        private void UpdatePopup()
        {
            if (_popup != null)
            {
                _popup.Dispose();
            }
            _popup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "TpUpgradePopup", PopupType.NORMAL)
                                .GetComponent<InGame.TpUpgradePopup>();
            _popup.Init(_currentItem);
        }
    }
}
