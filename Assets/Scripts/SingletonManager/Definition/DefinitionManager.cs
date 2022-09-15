using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class DefinitionManager : MonoBehaviour
{
    #region Singelton
    private static DefinitionManager _instance;
    public static DefinitionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DefinitionManager>();
                if (FindObjectsOfType<DefinitionManager>().Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong " +
                        " - there should never be more than 1 singleton!" +
                        " Reopening the scene might fix it.");
                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject go = new GameObject("DefinitionManager");
                    _instance = go.AddComponent<DefinitionManager>();
                }
            }

            return _instance;
        }
    }
    #endregion
    private List<Dictionary<string, object>> _csvData;
    public List<Dictionary<string, object>> CSVData { get { return _csvData; } }

    private async void LoadS3Data()
    {
        string res = await NetworkManager.Instance.API_S3Data("base.csv");
        if (res != null)
        {
            _csvData = CSVReader.Read(res);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadS3Data();
    }

    private Dictionary<string, int> PRIORITY = new Dictionary<string, int>()
    {
        {"^", 3},
        {"log", 3},
        {"*", 2},
        {"/", 2},
        {"+", 1},
        {"-", 1},
        {"(", 0},
    };

    public List<string> ConvEquationToPostfix(string s)
    {
        List<string> postfix = new List<string>();

        Regex checkNum = new Regex(@"^[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)$");
        Regex checkUnaryOp = new Regex(@"^(log10 | slog10)$");
        Regex checkBinaryOp = new Regex(@"^(\^| log |\*|\/|\+|\-)$");

        List<string> infix = s.Split(' ').ToList();
        Stack<string> stack = new Stack<string>();

        foreach(var v in infix) {
            switch (v)
            {
                case "(":
                    stack.Push(v);
                    break;
                case ")":
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        postfix.Add(stack.Pop());
                    }
                    stack.Pop();
                    break;
                case string name when checkNum.IsMatch(name):
                    postfix.Add(name);
                    break;
                case string name when checkUnaryOp.IsMatch(name):
                    stack.Push(name);
                    break;
                case string name when checkBinaryOp.IsMatch(name):
                    while (stack.Count > 0 && PRIORITY[name] <= PRIORITY[stack.Peek()])
                    {
                        postfix.Add(stack.Pop());
                    }
                    stack.Push(name);
                    break;
                default:
                    postfix.Add(stack.Pop());
                    break;
            }
        }

        while (stack.Count > 0)
        {
            postfix.Add(stack.Pop());
        }

        return postfix;
    }
}

