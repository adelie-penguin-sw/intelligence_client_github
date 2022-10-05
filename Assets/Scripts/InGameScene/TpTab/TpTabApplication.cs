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
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _tpTabModel.TPU01NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu011NameText);
            _tpTabModel.TPU02NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu013NameText);
            _tpTabModel.TPU03NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu012NameText);
            _tpTabModel.TPU04NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu008NameText);
            _tpTabModel.TPU05NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu001NameText);
            _tpTabModel.TPU06NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu016NameText);
            _tpTabModel.TPU07NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu005NameText);
            _tpTabModel.TPU08NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu006NameText);

            _tpTabModel.TPU01EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu011EffectText);
            _tpTabModel.TPU02EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu013EffectText);
            _tpTabModel.TPU03EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu012EffectText);
            _tpTabModel.TPU04EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu008EffectText);
            _tpTabModel.TPU05EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu001EffectText);
            _tpTabModel.TPU06EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu016EffectText);
            _tpTabModel.TPU07EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu005EffectText);
            _tpTabModel.TPU08EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu006EffectText);
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();
            
            if (!UserData.TPUpgrades.ContainsKey(11)) { UserData.TPUpgrades.Add(11, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[11].UpgradeCount));
            _tpTabModel.TPU01CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu011CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(13)) { UserData.TPUpgrades.Add(13, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[13].UpgradeCount));
            _tpTabModel.TPU02CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu013CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(12)) { UserData.TPUpgrades.Add(12, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[12].UpgradeCount));
            _tpTabModel.TPU03CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu012CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(8)) { UserData.TPUpgrades.Add(8, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[8].UpgradeCount));
            _tpTabModel.TPU04CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu008CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(1)) { UserData.TPUpgrades.Add(1, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[1].UpgradeCount));
            _tpTabModel.TPU05CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu001CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(16)) { UserData.TPUpgrades.Add(16, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[16].UpgradeCount));
            _tpTabModel.TPU06CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu016CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(5)) { UserData.TPUpgrades.Add(5, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[5].UpgradeCount));
            _tpTabModel.TPU07CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu005CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(6)) { UserData.TPUpgrades.Add(6, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[6].UpgradeCount));
            _tpTabModel.TPU08CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu006CostEquation) + " TP";
            inputMap.Clear();
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
