using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    public class QuestController : BaseTabController<MainTabApplication>
    {
        private MainTabView _view;

        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            _view = app.MainTabView;
        }

        public override void Set()
        {
            base.Set();
        }
        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }
}