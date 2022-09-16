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

    // Exchanging Equation
    private string _brainGeneratingCostEquation;
    private string _channelGeneratingCostEquation;
    private string _brainUpgradeCostEquation;
    private string _brainDecomposingGainEquation;
    private string _tpRewardForResetEquation;

    // Init Condition
    private int _initNP;
    private int _initTP;

    // 모든 필드의 getter
    public string BRAIN_GEN_COST_EQ { get { return _brainGeneratingCostEquation; } }
    public string CHNNL_GEN_COST_EQ { get { return _channelGeneratingCostEquation; } }
    public string BRAIN_UPG_COST_EQ { get { return _brainUpgradeCostEquation; } }
    public string BRAIN_DCP_GAIN_EQ { get { return _brainDecomposingGainEquation; } }
    public string TPRWD_FOR_RSET_EQ { get { return _tpRewardForResetEquation; } }
    public int INIT_NP { get { return _initNP; } }
    public int INIT_TP { get { return _initTP; } }

    private async void LoadS3Data()
    {
        string res = await NetworkManager.Instance.API_S3Data("base.csv");
        if (res != null)
        {
            _csvData = CSVReader.Read(res);

            // 모든 데이터 일일이 할당, 일단은
            _brainGeneratingCostEquation = (string)_csvData[0]["value"];
            _channelGeneratingCostEquation = (string)_csvData[1]["value"];
            _brainUpgradeCostEquation = (string)_csvData[2]["value"];
            _brainDecomposingGainEquation = (string)_csvData[3]["value"];
            _tpRewardForResetEquation = (string)_csvData[4]["value"];

            _initNP = (int)_csvData[9]["value"];
            _initTP = (int)_csvData[10]["value"];
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
                    stack.Push(UpArrowNotation.Slog10(a));
                    break;
                case "log10":
                    a = stack.Pop();
                    stack.Push(new UpArrowNotation(UpArrowNotation.Log10Top3Layer(a)));
                    break;
                case "^":
                    b = stack.Pop();
                    a = stack.Pop();
                    stack.Push(a ^ b);
                    break;
                case "log":
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

