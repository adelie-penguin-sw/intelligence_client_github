using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTowerNotation
{
    private static float _coeffMax = 10f;

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
        _layer += 1;

        float power = Mathf.Floor(Mathf.Log10(Mathf.Abs(number)));
        float coeff = number / Mathf.Pow(10, power);

        _coeffArr[0] = coeff;

        if (Mathf.Abs(power) < _coeffMax)
        {
            _coeffArr[1] = power;
            return;
        }
        _layer += 1;

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

    public PowerTowerNotation Copy()
    {
        PowerTowerNotation copiedNumber = new PowerTowerNotation();
        copiedNumber._coeffArr[0] = _coeffArr[0];
        copiedNumber._coeffArr[1] = _coeffArr[1];
        copiedNumber._coeffArr[2] = _coeffArr[2];
        copiedNumber._layer = _layer;

        return copiedNumber;
    }

    public static PowerTowerNotation operator +(PowerTowerNotation a) => a;

    public static PowerTowerNotation operator -(PowerTowerNotation a)
    {
        PowerTowerNotation result = a.Copy();
        result._coeffArr[0] *= -1;
        return result;
    }

    private PowerTowerNotation Add(PowerTowerNotation other)
    {
        PowerTowerNotation result = new PowerTowerNotation();

        float coeff = _coeffArr[0];
        float power = Mathf.Round(_coeffArr[1] * Mathf.Pow(10, _coeffArr[2]));
        float otherCoeff = other._coeffArr[0];
        float otherPower = Mathf.Round(other._coeffArr[1] * Mathf.Pow(10, other._coeffArr[2]));

        float powerDiff = Mathf.Abs(power - otherPower);
        float resultPower;

        if (power >= otherPower)
        {
            result._coeffArr[0] = coeff + otherCoeff / Mathf.Pow(10, powerDiff);
            resultPower = power;
        }
        else
        {
            result._coeffArr[0] = otherCoeff + coeff / Mathf.Pow(10, powerDiff);
            resultPower = otherPower;
        }

        if (Mathf.Abs(result._coeffArr[0]) >= _coeffMax)
        {
            result._coeffArr[0] /= 10f;
            resultPower += 1f;
        }

        if (resultPower >= _coeffMax)
        {
            result._coeffArr[2] = Mathf.Floor(Mathf.Log10(resultPower));
            result._coeffArr[1] = resultPower / Mathf.Pow(10, result._coeffArr[2]);
        }
        else
        {
            result._coeffArr[1] = resultPower;
        }

        if (result._coeffArr[1] >= 1f)
        {
            result._layer = 2;
        }
        if (result._coeffArr[2] >= 1f)
        {
            result._layer = 3;
        }

        return result;
    }

    public static PowerTowerNotation operator +(PowerTowerNotation a, PowerTowerNotation b) => a.Add(b);

    public static PowerTowerNotation operator +(PowerTowerNotation a, float b) => a.Add(new PowerTowerNotation(b));

    public static PowerTowerNotation operator +(PowerTowerNotation a, int b) => a.Add(new PowerTowerNotation(b));

    public static PowerTowerNotation operator +(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(b);

    public static PowerTowerNotation operator +(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(b);

    public static PowerTowerNotation operator -(PowerTowerNotation a, PowerTowerNotation b) => a.Add(-b);

    public static PowerTowerNotation operator -(PowerTowerNotation a, float b) => a.Add(-(new PowerTowerNotation(b)));

    public static PowerTowerNotation operator -(PowerTowerNotation a, int b) => a.Add(-(new PowerTowerNotation(b)));

    public static PowerTowerNotation operator -(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(-b);

    public static PowerTowerNotation operator -(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(-b);
}
