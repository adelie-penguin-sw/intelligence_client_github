using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    public class MainTabApplication : BaseTabApplication
    {
        [SerializeField]
        private BaseTabController<MainTabApplication>[] _controllers;
        [SerializeField]
        private MainTabView _mainTabView;
        public MainTabView MainTabView
        {
            get
            {
                return _mainTabView;
            }
        }
        [SerializeField]
        private MainTabModel _mainTabModel = new MainTabModel();
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
            foreach (var controller in _controllers)
            {
                controller.Init(this);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            foreach (var controller in _controllers)
            {
                controller.Set();
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
            PoolManager.Instance.DespawnObject(EPrefabsType.TabApplication, this.gameObject);
        }
    }
}