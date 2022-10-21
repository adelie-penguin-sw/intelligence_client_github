using System.Collections;
using System.Collections.Generic;
using TpTab;
using UnityEngine;

namespace ShopTab
{
    public class ShopTabApplication : BaseTabApplication
    {
        [SerializeField] private BaseTabController<ShopTabApplication>[] _controllers;

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
