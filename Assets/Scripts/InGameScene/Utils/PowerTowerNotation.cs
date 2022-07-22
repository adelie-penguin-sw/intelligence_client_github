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
        if (Mathf.Abs(number) < 10f)
        {
            _coeffArr[0] = number;
            return;
        }

        float power = Mathf.Floor(Mathf.Log10(Mathf.Abs(number)));
        float coeff = number / Mathf.Pow(10, power);

        _coeffArr[0] = coeff;

        if (Mathf.Abs(power) < 10f)
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
        Debug.Log(_coeffArr[0]);
        Debug.Log(_coeffArr[1]);
        Debug.Log(_coeffArr[2]);
    }

    public PowerTowerNotation(float number)
    {
        Convert(number);
        Debug.Log(_coeffArr[0]);
        Debug.Log(_coeffArr[1]);
        Debug.Log(_coeffArr[2]);
    }

    public PowerTowerNotation(int number)
    {
        Convert(number);
        Debug.Log(_coeffArr[0]);
        Debug.Log(_coeffArr[1]);
        Debug.Log(_coeffArr[2]);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
