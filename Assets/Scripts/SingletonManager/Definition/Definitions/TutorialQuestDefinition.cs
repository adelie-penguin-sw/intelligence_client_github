using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialQuestDefinition
{
    public int key;
    public string text;
    public long requirement;
    public string tpReward;

    public TutorialQuestDefinition(object key, object text, object requirement, object tpReward)
    {
        this.key = int.Parse(key.ToString());
        this.text = text.ToString();
        this.requirement = long.Parse(requirement.ToString());
        this.tpReward = tpReward.ToString();
    }
}


public class TutorialQuestDefinitions : ILoader<int, TutorialQuestDefinition>
{
    private List<Dictionary<string, object>> _csvData = new List<Dictionary<string, object>>();

    public TutorialQuestDefinitions(List<Dictionary<string, object>> csvData)
    {
        _csvData = csvData;
    }

    public Dictionary<int, TutorialQuestDefinition> MakeDict()
    {
        Dictionary<int, TutorialQuestDefinition> dict = new Dictionary<int, TutorialQuestDefinition>();
        foreach (var data in _csvData)
        {
            dict.Add(int.Parse(data["key"].ToString()),
                new TutorialQuestDefinition(data["key"], data["text"], data["requirement"], data["tpReward"]));
        }
        return dict;
    }
}