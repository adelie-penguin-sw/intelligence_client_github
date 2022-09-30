using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class DefinitionManager
{
    private List<Dictionary<string, object>> _csvData;
    public List<Dictionary<string, object>> CSVData { get { return _csvData; } }

    private Dictionary<string, object> _definitionDic = new Dictionary<string, object>();

    private async void LoadS3Data()
    {
        string res = await Managers.Network.API_S3Data("base.csv");
        if (res != null)
        {
            _csvData = CSVReader.Read(res);

            foreach (var li in _csvData)
            {
                string value = li["value"].ToString();
                switch (li["type"])
                {
                    case "string":
                    case "[]string":
                        if (!_definitionDic.ContainsKey((string)li["name"]))
                            _definitionDic.Add((string)li["name"], value);
                        else
                            _definitionDic[(string)li["name"]] = value;
                        break;

                    case "int":
                        if (!_definitionDic.ContainsKey((string)li["name"]))
                            _definitionDic.Add((string)li["name"], int.Parse(value));
                        else
                            _definitionDic[(string)li["name"]] = int.Parse(value);
                        break;
                    case "[]UpArrowNotation":
                        if (!_definitionDic.ContainsKey((string)li["name"]))
                            _definitionDic.Add((string)li["name"], ConvertToUANList(value));
                        else
                            _definitionDic[(string)li["name"]] = ConvertToUANList(value);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public T GetData<T>(string dataName)
    {
        if (_definitionDic.ContainsKey(dataName))
        {
            return (T)_definitionDic[dataName];
        }
        else
        {
            Debug.LogError("NULL DATA DICTIONARY");
            return default;
        }
    }

    public void Init()
    {
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

    private List<UpArrowNotation> ConvertToUANList(string data)
    {
        List<UpArrowNotation> result = new List<UpArrowNotation>();
        int initLength = data.Length;

        data = data.Substring(2, initLength - 4);
        data = data.Replace("], [", "/");
        string[] splittedData = data.Split('/');

        foreach (string segment in splittedData)
        {
            string[] coeffs = segment.Replace(", ", "/").Split('/');
            float top1Coeff, top2Coeff, top3Coeff;
            if(!float.TryParse(coeffs[0],out top1Coeff))
            {
                Debug.LogErrorFormat("Float Parse Error : {0}",coeffs[0]);
            }
            if (!float.TryParse(coeffs[1], out top2Coeff))
            {
                Debug.LogErrorFormat("Float Parse Error : {0}", coeffs[1]);
            }
            if (!float.TryParse(coeffs[2], out top3Coeff))
            {
                Debug.LogErrorFormat("Float Parse Error : {0}", coeffs[2]);
            }

            result.Add(new UpArrowNotation(top1Coeff, top2Coeff, top3Coeff));
        }

        return result;
    }

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

    public string CalcEquationToString(Dictionary<string, UpArrowNotation> inputMap, string equationKey)
    {
        return CalcPostfix(inputMap, ConvEquationToPostfix(GetData<string>(equationKey))).ToString();
    }
}

