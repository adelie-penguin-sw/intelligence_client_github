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

            _tpTabModel.TPU001NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu001NameText);
            _tpTabModel.TPU002NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu002NameText);
            _tpTabModel.TPU003NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu003NameText);
            _tpTabModel.TPU004NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu004NameText);
            _tpTabModel.TPU005NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu005NameText);
            _tpTabModel.TPU006NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu006NameText);
            _tpTabModel.TPU007NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu007NameText);
            _tpTabModel.TPU008NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu008NameText);
            _tpTabModel.TPU009NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu009NameText);
            _tpTabModel.TPU010NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu010NameText);
            _tpTabModel.TPU011NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu011NameText);
            _tpTabModel.TPU012NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu012NameText);
            _tpTabModel.TPU013NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu013NameText);
            _tpTabModel.TPU014NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu014NameText);
            _tpTabModel.TPU015NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu015NameText);
            _tpTabModel.TPU016NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu016NameText);
            _tpTabModel.TPU017NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu017NameText);
            _tpTabModel.TPU018NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu018NameText);
            _tpTabModel.TPU019NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu019NameText);
            _tpTabModel.TPU020NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu020NameText);
            _tpTabModel.TPU021NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu021NameText);
            _tpTabModel.TPU022NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu022NameText);
            _tpTabModel.TPU023NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu023NameText);
            _tpTabModel.TPU024NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu024NameText);
            _tpTabModel.TPU025NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu025NameText);
            _tpTabModel.TPU026NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu026NameText);
            _tpTabModel.TPU027NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu027NameText);
            _tpTabModel.TPU028NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu028NameText);
            _tpTabModel.TPU029NameText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu029NameText);

            _tpTabModel.TPU001EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu001EffectText);
            _tpTabModel.TPU002EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu002EffectText);
            _tpTabModel.TPU003EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu003EffectText);
            _tpTabModel.TPU004EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu004EffectText);
            _tpTabModel.TPU005EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu005EffectText);
            _tpTabModel.TPU006EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu006EffectText);
            _tpTabModel.TPU007EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu007EffectText);
            _tpTabModel.TPU008EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu008EffectText);
            _tpTabModel.TPU009EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu009EffectText);
            _tpTabModel.TPU010EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu010EffectText);
            _tpTabModel.TPU011EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu011EffectText);
            _tpTabModel.TPU012EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu012EffectText);
            _tpTabModel.TPU013EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu013EffectText);
            _tpTabModel.TPU014EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu014EffectText);
            _tpTabModel.TPU015EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu015EffectText);
            _tpTabModel.TPU016EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu016EffectText);
            _tpTabModel.TPU017EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu017EffectText);
            _tpTabModel.TPU018EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu018EffectText);
            _tpTabModel.TPU019EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu019EffectText);
            _tpTabModel.TPU020EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu020EffectText);
            _tpTabModel.TPU021EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu021EffectText);
            _tpTabModel.TPU022EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu022EffectText);
            _tpTabModel.TPU023EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu023EffectText);
            _tpTabModel.TPU024EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu024EffectText);
            _tpTabModel.TPU025EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu025EffectText);
            _tpTabModel.TPU026EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu026EffectText);
            _tpTabModel.TPU027EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu027EffectText);
            _tpTabModel.TPU028EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu028EffectText);
            _tpTabModel.TPU029EffectText.text = Managers.Definition.GetData<string>(DefinitionKey.tpu029EffectText);
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();

            if (!UserData.TPUpgrades.ContainsKey(1)) { UserData.TPUpgrades.Add(1, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[1].UpgradeCount));
            _tpTabModel.TPU001CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu001CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(2)) { UserData.TPUpgrades.Add(2, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[2].UpgradeCount));
            _tpTabModel.TPU002CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu002CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(3)) { UserData.TPUpgrades.Add(3, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[3].UpgradeCount));
            _tpTabModel.TPU003CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu003CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(4)) { UserData.TPUpgrades.Add(4, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[4].UpgradeCount));
            _tpTabModel.TPU004CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu004CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(5)) { UserData.TPUpgrades.Add(5, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[5].UpgradeCount));
            _tpTabModel.TPU005CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu005CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(6)) { UserData.TPUpgrades.Add(6, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[6].UpgradeCount));
            _tpTabModel.TPU006CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu006CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(7)) { UserData.TPUpgrades.Add(7, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[7].UpgradeCount));
            _tpTabModel.TPU007CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu007CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(8)) { UserData.TPUpgrades.Add(8, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[8].UpgradeCount));
            _tpTabModel.TPU008CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu008CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(9)) { UserData.TPUpgrades.Add(9, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[9].UpgradeCount));
            _tpTabModel.TPU009CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu009CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(10)) { UserData.TPUpgrades.Add(10, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[10].UpgradeCount));
            _tpTabModel.TPU010CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu010CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(11)) { UserData.TPUpgrades.Add(11, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[11].UpgradeCount));
            _tpTabModel.TPU011CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu011CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(12)) { UserData.TPUpgrades.Add(12, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[12].UpgradeCount));
            _tpTabModel.TPU012CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu012CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(13)) { UserData.TPUpgrades.Add(13, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[13].UpgradeCount));
            _tpTabModel.TPU013CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu013CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(14)) { UserData.TPUpgrades.Add(14, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[14].UpgradeCount));
            _tpTabModel.TPU014CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu014CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(15)) { UserData.TPUpgrades.Add(15, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[15].UpgradeCount));
            _tpTabModel.TPU015CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu015CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(16)) { UserData.TPUpgrades.Add(16, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[16].UpgradeCount));
            _tpTabModel.TPU016CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu016CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(17)) { UserData.TPUpgrades.Add(17, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[17].UpgradeCount));
            _tpTabModel.TPU017CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu017CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(18)) { UserData.TPUpgrades.Add(18, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[18].UpgradeCount));
            _tpTabModel.TPU018CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu018CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(19)) { UserData.TPUpgrades.Add(19, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[19].UpgradeCount));
            _tpTabModel.TPU019CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu019CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(20)) { UserData.TPUpgrades.Add(20, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[20].UpgradeCount));
            _tpTabModel.TPU020CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu020CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(21)) { UserData.TPUpgrades.Add(21, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[21].UpgradeCount));
            _tpTabModel.TPU021CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu021CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(22)) { UserData.TPUpgrades.Add(22, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[22].UpgradeCount));
            _tpTabModel.TPU022CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu022CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(23)) { UserData.TPUpgrades.Add(23, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[23].UpgradeCount));
            _tpTabModel.TPU023CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu023CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(24)) { UserData.TPUpgrades.Add(24, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[24].UpgradeCount));
            _tpTabModel.TPU024CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu024CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(25)) { UserData.TPUpgrades.Add(25, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[25].UpgradeCount));
            _tpTabModel.TPU025CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu025CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(26)) { UserData.TPUpgrades.Add(26, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[26].UpgradeCount));
            _tpTabModel.TPU026CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu026CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(27)) { UserData.TPUpgrades.Add(27, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[27].UpgradeCount));
            _tpTabModel.TPU027CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu027CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(28)) { UserData.TPUpgrades.Add(28, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[28].UpgradeCount));
            _tpTabModel.TPU028CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu028CostEquation) + " TP";
            inputMap.Clear();

            if (!UserData.TPUpgrades.ContainsKey(29)) { UserData.TPUpgrades.Add(29, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[29].UpgradeCount));
            _tpTabModel.TPU029CostText.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpu029CostEquation) + " TP";
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
