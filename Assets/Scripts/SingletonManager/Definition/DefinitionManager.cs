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

    private Dictionary<string, object> _definitionDic = new Dictionary<string, object>();
    public object this[string name] { get { return _definitionDic[name]; } }

    private async void LoadS3Data()
    {
        string res = await NetworkManager.Instance.API_S3Data("base.csv");
        if (res != null)
        {
            _csvData = CSVReader.Read(res);

            foreach (var li in _csvData)
            {
                string value = li["value"].ToString();
                switch (li["type"])
                {
                    case "string":
                        _definitionDic.Add((string)li["name"], value);
                        break;
                    case "int":
                        _definitionDic.Add((string)li["name"], int.Parse(value));
                        break;
                    case "[]string":
                        _definitionDic.Add((string)li["name"], value);  // 일단 그대로 추가, 나중에 형식에 따라 변환할 예정
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadS3Data();
    }

    private Dictionary<string, int> PRIORITY = new Dictionary<string, int>()
    {
        {"log10", 4},
        {"slog10", 4},
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
        Regex checkUnaryOp = new Regex(@"^(log10|slog10)$");
        Regex checkBinaryOp = new Regex(@"^(\^|log|\*|\/|\+|\-)$");

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
                    postfix.Add("0");
                    while (stack.Count > 0 && PRIORITY[name] <= PRIORITY[stack.Peek()])
                    {
                        postfix.Add(stack.Pop());
                    }
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
                    postfix.Add(v);
                    break;
            }
        }

        while (stack.Count > 0)
        {
            postfix.Add(stack.Pop());
        }

        return postfix;
    }

    public UpArrowNotation CalcPostfix(Dictionary<string, UpArrowNotation> inputMap, List<string> postfix)
    {
        Regex checkNum = new Regex(@"^[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)$");

        Stack<UpArrowNotation> stack = new Stack<UpArrowNotation>();

        foreach (var v in postfix)
        {
            UpArrowNotation b = new UpArrowNotation(0);
            UpArrowNotation a = new UpArrowNotation(0);
            switch (v)
            {
                case string name when checkNum.IsMatch(name):
                    UpArrowNotation f = new UpArrowNotation(float.Parse(name));
                    stack.Push(f);
                    break;
                case "slog10":
                    a = stack.Pop();
                    stack.Pop();
                    stack.Push(UpArrowNotation.Slog10(a));
                    break;
                case "log10":
                    a = stack.Pop();
                    stack.Pop();
                    stack.Push(new UpArrowNotation(UpArrowNotation.Log10Top3Layer(a)));
                    break;
                case "^":
                    b = stack.Pop();
                    a = stack.Pop();
                    stack.Push(a ^ b);
                    break;
                case "log":
                    b = stack.Pop();
                    a = stack.Pop();
                    stack.Push(UpArrowNotation.Log(a, b));
                    break;
                case "*":
                    b = stack.Pop();
                    a = stack.Pop();
                    stack.Push(a * b);
                    break;
                case "/":
                    b = stack.Pop();
                    a = stack.Pop();
                    stack.Push(a / b);
                    break;
                case "+":
                    b = stack.Pop();
                    a = stack.Pop();
                    stack.Push(a + b);
                    break;
                case "-":
                    b = stack.Pop();
                    a = stack.Pop();
                    stack.Push(a - b);
                    break;
                default:
                    stack.Push(inputMap[v]);
                    break;
            }
        }
        return stack.Pop();
    }

    public UpArrowNotation CalcEquation(Dictionary<string, UpArrowNotation> inputMap, string equation)
    {
        return CalcPostfix(inputMap, ConvEquationToPostfix(equation));
    }

}

