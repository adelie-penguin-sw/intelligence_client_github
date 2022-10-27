using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserTab
{
    public class UserTabApplication : BaseTabApplication
    {
        [SerializeField] private BaseTabController<UserTabApplication>[] _controllers;
        [SerializeField] private UserTabView _userTabView;
        public UserTabView UserTabView
        {
            get
            {
                return _userTabView;
            }
        }

        [SerializeField] private UserTabModel _userTabModel;
        public UserTabModel UserTabModel
        {
            get
            {
                return _userTabModel;
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
        }
    }
}
