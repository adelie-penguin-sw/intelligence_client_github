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

        public override void Init()
        {
            base.Init();
            NetworkManager.Instance.API_LoadUserData(wrapper =>
            {
                _mainTabModel.SingleNetworkWrapper = wrapper;

                foreach (var controller in _controllers)
                {
                    controller.Init(this);
                }
                CreateSingleNetworkBrainNumberRequest req = new CreateSingleNetworkBrainNumberRequest();
                req.brain = 6;
                NetworkManager.Instance.API_CreateSingleNetworkBrainNumber(req);
            });
        }

        public override void OnEnter()
        {
            base.OnEnter();

            NetworkManager.Instance.API_LoadUserData(wrapper =>
            {
                _mainTabModel.SingleNetworkWrapper = wrapper;
                foreach (var controller in _controllers)
                {
                    controller.Set();
                }
            });
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
            foreach (var controller in _controllers)
            {
                controller.AdvanceTime(dt_sec);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            foreach (var controller in _controllers)
            {
                controller.Dispose();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            PoolManager.Instance.DespawnObject(EPrefabsType.TAP_APPLICATION, this.gameObject);
        }
    }
}