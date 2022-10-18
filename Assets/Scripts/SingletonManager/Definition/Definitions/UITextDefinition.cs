using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextDefinition
{
    public string txtName;
    public Dictionary<ELanguage, string> text = new Dictionary<ELanguage, string>();

    public UITextDefinition(string txtName, string txtKor, string txtEng)
    {
        this.txtName = txtName;
        this.text.Add(ELanguage.KOR, txtKor);
        this.text.Add(ELanguage.ENG, txtEng);
    }

    public string GetString(params string[] formatParams)
    {
        return string.Format(text[UserData.Lang], formatParams);
    }
}

public class UITextDefinitions : ILoader<string, UITextDefinition>
{
    private List<Dictionary<string, object>> _csvData = new List<Dictionary<string, object>>();

    public UITextDefinitions(List<Dictionary<string, object>> csvData)
    {
        _csvData = csvData;
    }

    public Dictionary<string, UITextDefinition> MakeDict()
    {
        Dictionary<string, UITextDefinition> dict = new Dictionary<string, UITextDefinition>();
        foreach (var data in _csvData)
        {
            dict.Add(data["name"].ToString(), new UITextDefinition(data["name"].ToString(), data["value-kor"].ToString(), data["value-eng"].ToString()));
        }
        return dict;
    }
}

public enum ELanguage
{
    KOR,
    ENG, 
}
