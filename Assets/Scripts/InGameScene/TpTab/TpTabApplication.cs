using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TpTab
{
    public class TpTabApplication : BaseTabApplication
    {
        [SerializeField] private BaseTabController<TpTabApplication>[] _controllers;
        [SerializeField] private TpTabView _tpTabView;
        public TpTabView TpTabView
        {
            get
            {
                return _tpTabView;
            }
        }

        [SerializeField] private TpTabModel _tpTabModel;
        public TpTabModel TpTabModel
        {
            get
            {
                return _tpTabModel;
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

            _tpTabModel.MainCamera.transform.position = new Vector3(0, 0, -10);

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
