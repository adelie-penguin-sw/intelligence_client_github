using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTowerNotation
{
    private static float _coeffMax = 10f;
    private static float _coeffMin = -10f;

    private float[] _coeffArr = {0f, 0f, 0f};
    private int _layer = 1;

    public int Layer
    {
        get
        {
            return _layer;
        }
    }

    private void Convert(float number)
    {
        if (Mathf.Abs(number) < _coeffMax)
        {
            _coeffArr[0] = number;
            return;
        }

        float power = Mathf.Floor(Mathf.Log10(Mathf.Abs(number)));
        float coeff = number / Mathf.Pow(10, power);

        _coeffArr[0] = coeff;

        if (Mathf.Abs(power) < _coeffMax)
        {
            _coeffArr[1] = power;
            return;
        }

        number = power;
        power = Mathf.Floor(Mathf.Log10(number));
        coeff = number / Mathf.Pow(10, power);

        _coeffArr[1] = coeff;
        _coeffArr[2] = power;
    }

    public PowerTowerNotation()
    {
        
    }

    public PowerTowerNotation(float number)
    {
        Convert(number);
    }

    public PowerTowerNotation(int number)
    {
        Convert(number);
    }

    public override string ToString()
    {
        if (_coeffArr[2] == 0f)
        {
            return (_coeffArr[0] * Mathf.Pow(10, _coeffArr[1])).ToString("N0");
        }

        string powerString = (_coeffArr[1] * Mathf.Pow(10, _coeffArr[2])).ToString("N0");
        string coeffString = _coeffArr[0].ToString("N2");

        return coeffString + "x10^" + powerString;
    }
}
