using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDefinition
{
    public int key;
    public Dictionary<ETextLanguage, string> text = new Dictionary<ETextLanguage, string>();

    public TextDefinition(int key, string txtKor, string txtEng)
    {
        this.key = key;
        this.text.Add(ETextLanguage.KOR, txtKor);
        this.text.Add(ETextLanguage.ENG, txtEng);
    }

    public string GetText(params string[] formatParams)
    {
        return string.Format(text[UserData.Lang], formatParams);
    }
}

public class TextDefinitions : ILoader<int, TextDefinition>
{
    private List<Dictionary<string, object>> _csvData = new List<Dictionary<string, object>>();

    public TextDefinitions(List<Dictionary<string, object>> csvData)
    {
        _csvData = csvData;
    }

    public Dictionary<int, TextDefinition> MakeDict()
    {
        Dictionary<int, TextDefinition> dict = new Dictionary<int, TextDefinition>();
        foreach (var data in _csvData)
        {
            dict.Add((int)data["key"], new TextDefinition((int)data["key"], data["kor"].ToString(), data["eng"].ToString()));
        }
        return dict;
    }
}
