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

    struct CoeffAndPower
    {
        public float coeff;
        public float power;
    }

    private CoeffAndPower Decompose(float number)
    {
        CoeffAndPower cap = new CoeffAndPower();

        if (number == 0f)
        {
            cap.coeff = 0f;
            cap.power = 0f;
            return cap;
        }

        cap.power = Mathf.Floor(Mathf.Log10(Mathf.Abs(number)));
        cap.coeff = number / Mathf.Pow(10, cap.power);

        return cap;
    }

    private void Convert(float number)
    {
        CoeffAndPower cap = Decompose(number);
        _coeffArr[0] = cap.coeff;

        float tmpPower = cap.power;
        if (tmpPower == 0f)
        {
            _layer = 1;
            return;
        }
        else if (Mathf.Abs(tmpPower) < _coeffMax)
        {
            _layer = 2;
            _coeffArr[1] = tmpPower;
            return;
        }
        else
        {
            _layer = 3;
            cap = Decompose(tmpPower);
            _coeffArr[1] = cap.coeff;
            _coeffArr[2] = cap.power;
            return;
        }
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
            if (_coeffArr[1] >= 0f)
            {
                return (_coeffArr[0] * Mathf.Pow(10, _coeffArr[1])).ToString("N0");
            }
            else
            {
                return (_coeffArr[0] * Mathf.Pow(10, _coeffArr[1])).ToString("N" + (-_coeffArr[1]).ToString("N0"));
            }
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

        if (result._coeffArr[0] == 0f)
        {
            return result;
        }

        if (Mathf.Abs(result._coeffArr[0]) >= _coeffMax)
        {
            result._coeffArr[0] /= 10f;
            resultPower += 1f;
        }

        while (Mathf.Abs(result._coeffArr[0]) < 1f)
        {
            result._coeffArr[0] *= 10f;
            resultPower -= 1f;
        }

        if (Mathf.Abs(resultPower) >= _coeffMax)
        {
            result._coeffArr[2] = Mathf.Floor(Mathf.Log10(Mathf.Abs(resultPower)));
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

    public void Reciprocate()                   // Exception NOT Handled Yet
    {
        _coeffArr[1] *= -1f;

        if (_coeffArr[0] == 1f)
        {
            return;
        }

        _coeffArr[0] = 10f / _coeffArr[0];      // NOT ACCURATE !!!!!
        _coeffArr[1] -= 1f;
    }

    private PowerTowerNotation Multiply(PowerTowerNotation other)
    {
        PowerTowerNotation result = new PowerTowerNotation();

        float coeff = _coeffArr[0];
        float power = Mathf.Round(_coeffArr[1] * Mathf.Pow(10, _coeffArr[2]));
        float otherCoeff = other._coeffArr[0];
        float otherPower = Mathf.Round(other._coeffArr[1] * Mathf.Pow(10, other._coeffArr[2]));

        float resultPower = power + otherPower;

        result._coeffArr[0] = coeff * otherCoeff;

        if (result._coeffArr[0] == 0f)
        {
            return result;
        }

        if (Mathf.Abs(result._coeffArr[0]) >= _coeffMax)
        {
            result._coeffArr[0] /= 10f;
            resultPower += 1f;
        }

        while (Mathf.Abs(result._coeffArr[0]) < 1f)
        {
            result._coeffArr[0] *= 10f;
            resultPower -= 1f;
        }

        if (Mathf.Abs(resultPower) >= _coeffMax)
        {
            result._coeffArr[2] = Mathf.Floor(Mathf.Log10(Mathf.Abs(resultPower)));
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

    public static PowerTowerNotation operator *(PowerTowerNotation a, PowerTowerNotation b) => a.Multiply(b);

    public static PowerTowerNotation operator *(PowerTowerNotation a, float b) => a.Multiply(new PowerTowerNotation(b));

    public static PowerTowerNotation operator *(PowerTowerNotation a, int b) => a.Multiply(new PowerTowerNotation(b));

    public static PowerTowerNotation operator *(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Multiply(b);

    public static PowerTowerNotation operator *(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Multiply(b);

    public static PowerTowerNotation operator /(PowerTowerNotation a, PowerTowerNotation b)
    {
        b.Reciprocate();
        return a.Multiply(b);
    }

    public static PowerTowerNotation operator /(PowerTowerNotation a, float b)
    {
        PowerTowerNotation temp = new PowerTowerNotation(b);
        temp.Reciprocate();
        return a.Multiply(temp);
    }

    public static PowerTowerNotation operator /(PowerTowerNotation a, int b)
    {
        PowerTowerNotation temp = new PowerTowerNotation(b);
        temp.Reciprocate();
        return a.Multiply(temp);
    }

    public static PowerTowerNotation operator /(float a, PowerTowerNotation b)
    {
        PowerTowerNotation temp = new PowerTowerNotation(a);
        b.Reciprocate();
        return temp.Multiply(b);
    }

    public static PowerTowerNotation operator /(int a, PowerTowerNotation b)
    {
        PowerTowerNotation temp = new PowerTowerNotation(a);
        b.Reciprocate();
        return temp.Multiply(b);
    }
}
