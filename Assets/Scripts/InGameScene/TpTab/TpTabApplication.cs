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

            if (!UserData.TPUpgradeCounts.ContainsKey(1)) { UserData.TPUpgradeCounts.Add(1, 0); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgradeCounts[1]));
            _tpTabModel.TPU01CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu011CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgradeCounts.ContainsKey(2)) { UserData.TPUpgradeCounts.Add(2, 0); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgradeCounts[2]));
            _tpTabModel.TPU02CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu013CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgradeCounts.ContainsKey(3)) { UserData.TPUpgradeCounts.Add(3, 0); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgradeCounts[3]));
            _tpTabModel.TPU03CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu012CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgradeCounts.ContainsKey(4)) { UserData.TPUpgradeCounts.Add(4, 0); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgradeCounts[4]));
            _tpTabModel.TPU04CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu008CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgradeCounts.ContainsKey(5)) { UserData.TPUpgradeCounts.Add(5, 0); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgradeCounts[5]));
            _tpTabModel.TPU05CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu001CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgradeCounts.ContainsKey(6)) { UserData.TPUpgradeCounts.Add(6, 0); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgradeCounts[6]));
            _tpTabModel.TPU06CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu016CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgradeCounts.ContainsKey(7)) { UserData.TPUpgradeCounts.Add(7, 0); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgradeCounts[7]));
            _tpTabModel.TPU07CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu005CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgradeCounts.ContainsKey(8)) { UserData.TPUpgradeCounts.Add(8, 0); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgradeCounts[8]));
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
