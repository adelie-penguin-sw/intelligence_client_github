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
    public string costEquation;

    public TpUpgradeDefinition(object key, object nameText, object effectText, object costEquation)
    {
        this.key = int.Parse(key.ToString());
        this.nameText = nameText.ToString();
        this.effectText = effectText.ToString();
        this.costEquation = costEquation.ToString();
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
                new TpUpgradeDefinition(data["key"], data["nameText"], data["effectText"], data["costEquation"]));
        }
        return dict;
    }
}