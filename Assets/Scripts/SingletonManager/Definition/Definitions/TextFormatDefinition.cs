using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFormatDefinition
{
    public int key;
    public Dictionary<ETextLanguage, string> text = new Dictionary<ETextLanguage, string>();

    public TextFormatDefinition(int key, string txtKor, string txtEng)
    {
        this.key = key;
        this.text.Add(ETextLanguage.KOR, txtKor);
        this.text.Add(ETextLanguage.ENG, txtEng);
    }
}

public class TextFormatDefinitions : ILoader<int, TextFormatDefinition>
{
    private List<Dictionary<string, object>> _csvData = new List<Dictionary<string, object>>();

    public TextFormatDefinitions(List<Dictionary<string, object>> csvData)
    {
        _csvData = csvData;
    }

    public Dictionary<int, TextFormatDefinition> MakeDict()
    {
        Dictionary<int, TextFormatDefinition> dict = new Dictionary<int, TextFormatDefinition>();
        foreach (var data in _csvData)
        {
            dict.Add((int)data["key"], new TextFormatDefinition((int)data["key"], data["kor"].ToString(), data["eng"].ToString()));
        }
        return dict;
    }
}