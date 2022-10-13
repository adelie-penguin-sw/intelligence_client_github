using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TpUpgradeItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _numText;
    [SerializeField] private TextMeshProUGUI _effectText;
    [SerializeField] private TextMeshProUGUI _costText;
    private int _index;
    private string _costEquation;
    public void SetNameText(string str)
    {
        _nameText.text = str;
    }
    public void SetNumText(string str)
    {
        _numText.text = str;
    }
    public void SetEffectText(string str)
    {
        _effectText.text = str;
    }
    private Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();
    public void SetCostText(int index, string costEquation)
    {
        _index = index;
        _costEquation = costEquation;
        UpdateCostText();
    }

    public void UpdateCostText()
    {
        inputMap.Clear();
        if (!UserData.TPUpgrades.ContainsKey(_index + 1)) { UserData.TPUpgrades.Add(_index + 1, new TPUpgrade(false, 0, false)); }
        inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[_index + 1].UpgradeCount));
        _costText.text = Managers.Definition.CalcEquationToStringForStr(inputMap, _costEquation) + " TP";
    }
}
