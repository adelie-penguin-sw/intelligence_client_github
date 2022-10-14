using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    /// <summary>
    /// 메인 탭 어플리케이션
    /// </summary>
    public class MainTabApplication : BaseTabApplication
    {
        [SerializeField] private BaseTabController<MainTabApplication>[] _controllers;
        [SerializeField] private MainTabView _mainTabView;
        public MainTabView MainTabView
        {
            get
            {
                return _mainTabView;
            }
        }
        [SerializeField] private MainTabModel _mainTabModel = new MainTabModel();
        public MainTabModel MainTabModel
        {
            get
            {
                return _mainTabModel;
            }
        }

        public override async void Init()
        {
            base.Init();

            if (await Managers.Network.API_LoadUserData())
            {
                foreach (var controller in _controllers)
                {
                    controller.Init(this);
                }
            }
        }

        public override async void OnEnter()
        {
            base.OnEnter();

            Camera.main.transform.position = new Vector3(0, 0, -10);

            if (await Managers.Network.API_LoadUserData())
            {
                foreach (var controller in _controllers)
                {
                    controller.Set();
                }
            }
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
            foreach (var controller in _controllers)
            {
                controller.AdvanceTime(dt_sec);
            }
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
            foreach (var controller in _controllers)
            {
                controller.LateAdvanceTime(dt_sec);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var controller in _controllers)
            {
                controller.Dispose();
            }
            Managers.Pool.DespawnObject(EPrefabsType.TAP_APPLICATION, this.gameObject);
        }

        public void OnClick_ResetButton()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_RESET_BUTTON);
        }
    }
}