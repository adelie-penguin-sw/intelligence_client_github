using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TpTab
{
    public class TpUpgradeController : BaseTabController<TpTabApplication>
    {
        private Dictionary<int, TpUpgradeDefinition> _definition;
        public override void Init(TpTabApplication app)
        {
            base.Init(app);
            _definition = _app.TpTabModel.TpUpgradeDefinition;

            foreach(var id in _definition.Keys)
            {
                _app.TpTabView.SetUpgradeItem(
                    id,
                    _definition[id].nameText,
                    string.Format("#{0}", id),
                    _definition[id].effectText,
                    _definition[id].costEquation
                    );
            }
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
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
        }

    }
}
