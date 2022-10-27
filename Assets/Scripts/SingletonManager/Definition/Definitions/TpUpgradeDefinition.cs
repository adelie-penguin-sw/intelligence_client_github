using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class TpUpgradeDefinition
{
    public int key;
    public string nameText;
    public string effectText;
    public string effectValueText;
    public string effectValueEquation;
    public string costEquation;
    public int maxLevel;
    public List<List<int>> unlockRequirement;

    public TpUpgradeDefinition(object key, object nameText, object effectText, object effectValueText, object effectValueEquation, object costEquation, object maxLevel, object unlockRequirement)
    {
        this.key = int.Parse(key.ToString());
        this.nameText = nameText.ToString();
        this.effectText = effectText.ToString();
        this.effectValueText = effectValueText.ToString();
        this.effectValueEquation = effectValueEquation.ToString();
        this.costEquation = costEquation.ToString();
        this.maxLevel = int.Parse(maxLevel.ToString());
        this.unlockRequirement = ConvertIntPairList(unlockRequirement.ToString());
    }

    private List<List<int>> ConvertIntPairList(string data)
    {
        List<List<int>> result = new List<List<int>>();
        int initLength = data.Length;
        if (initLength == 2)
        {
            return result;
        }

        data = data.Substring(2, initLength - 4);
        data = data.Replace("], [", "/");
        string[] splittedData = data.Split('/');

        foreach (string segment in splittedData)
        {
            string[] intPair = segment.Replace(", ", "/").Split('/');
            int upgrade, requirement;
            if (!int.TryParse(intPair[0], out upgrade))
            {
                Debug.LogErrorFormat("Float Parse Error : {0}", intPair[0]);
            }
            if (!int.TryParse(intPair[1], out requirement))
            {
                Debug.LogErrorFormat("Float Parse Error : {0}", intPair[1]);
            }

            result.Add(new List<int> { upgrade, requirement });
        }

        return result;
    }
}

public class TpUpgradeDefinitions : ILoader<int, TpUpgradeDefinition>
{
    private List<Dictionary<string, object>> _csvData = new List<Dictionary<string, object>>();

    public TpUpgradeDefinitions(List<Dictionary<string, object>> csvData)
    {
        _csvData = csvData;
    }

    public Dictionary<int, TpUpgradeDefinition> MakeDict()
    {
        Dictionary<int, TpUpgradeDefinition> dict = new Dictionary<int, TpUpgradeDefinition>();
        foreach(var data in _csvData)
        {
            dict.Add(int.Parse(data["key"].ToString()),
                new TpUpgradeDefinition(data["key"], data["nameText"], data["effectText"], data["effectValueText"], data["effectValueEquation"], data["costEquation"], data["maxLevel"], data["unlockRequirement"]));
        }
        return dict;
    }
}