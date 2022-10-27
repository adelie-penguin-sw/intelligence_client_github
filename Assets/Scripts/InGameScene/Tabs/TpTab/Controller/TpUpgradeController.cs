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


        #region CameraSmoothMoving
        private bool _smoothMoving = false;

        private Transform _target;

        private float _smoothTime = 0.07f;

        private Vector3 _lastMovingVelocity;
        private Vector3 _targetPosition;

        private float _targetZoomSize = 350f;
        private float _lastZoomSpeed;
        #endregion


        public override void Init(TpTabApplication app)
        {
            base.Init(app);
            _definition = _app.TpTabModel.TpUpgradeDefinition;
            SetAllUpgradeItems();

            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_TPUPGRADE_ICON);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_TPUPGRADE);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.EXIT_TPUPGRADE_POPUP);
        }

        public override void Set()
        {
            base.Set();
            _app.TpTabModel.MainCamera.orthographicSize = 500f;
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            if (_smoothMoving)
            {
                MoveCamSmooth();
                ZoomCamSmooth();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_TPUPGRADE_ICON);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_TPUPGRADE);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.EXIT_TPUPGRADE_POPUP);
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
                    _targetPosition = _currentItem.transform.position - new Vector3(0f, 170f, 10f);
                    _smoothMoving = true;

                    _popup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "TpUpgradePopup", PopupType.NORMAL)
                                        .GetComponent<InGame.TpUpgradePopup>();
                    _popup.Init(_currentItem);
                    break;
                case ENotiMessage.ONCLICK_TPUPGRADE:
                    _currentItem = (TpUpgradeItem)noti.data[EDataParamKey.CLASS_TPU_ITEM];
                    SetAllUpgradeItems();
                    UpdatePopup();
                    break;
                case ENotiMessage.EXIT_TPUPGRADE_POPUP:
                    _smoothMoving = false;
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
                    _definition[id].effectValueText,
                    _definition[id].effectValueEquation,
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

        private void MoveCamSmooth()
        {
            if (_targetPosition != null)
            {
                Vector3 smoothPosition = Vector3.SmoothDamp(_app.TpTabModel.MainCamera.transform.position, _targetPosition, ref _lastMovingVelocity, _smoothTime);
                _app.TpTabModel.MainCamera.transform.position = smoothPosition;
            }
        }

        private void ZoomCamSmooth()
        {
            float smoothZoomSize = Mathf.SmoothDamp(_app.TpTabModel.MainCamera.orthographicSize, _targetZoomSize, ref _lastZoomSpeed, _smoothTime);
            _app.TpTabModel.MainCamera.orthographicSize = smoothZoomSize;
        }
    }
}
