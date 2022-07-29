using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpArrowNotation
{
    private PowerTowerNotation _top3Layer;
    public int[] _operatorLayerCount = {1, 1, 1, 1, 1, 1, 1, 1, 1};

    public PowerTowerNotation Top3Layer { get { return _top3Layer; } }

    private void Convert(double number)
    {
        _top3Layer = new PowerTowerNotation(number);

        if (_top3Layer._coeffArr[1] != 0f)
        {
            _operatorLayerCount[0] = 2;
        }
        if (_top3Layer._coeffArr[2] != 0f)
        {
            _operatorLayerCount[0] = 3;
        }
    }

    public UpArrowNotation()
    {
        _top3Layer = new PowerTowerNotation();
    }

    public UpArrowNotation(double number)
    {
        Convert(number);
    }

    public UpArrowNotation(int number)
    {
        Convert(number);
    }

    public UpArrowNotation(double layer1Coeff, double layer2Coeff, double layer3Coeff, int operatorLayerCounts)
    {
        _top3Layer = new PowerTowerNotation(layer1Coeff, layer2Coeff, layer3Coeff);

        for (int i=8; i>=0; i--)
        {
            _operatorLayerCount[i] = operatorLayerCounts % 10;
            operatorLayerCounts /= 10;
        }
    }

    public override string ToString()
    {
        string resultString = _top3Layer.ToString();

        int layerCountTest = 1;
        for (int i=1; i<9; i++)
        {
            layerCountTest *= _operatorLayerCount[i];
        }

        if (_top3Layer._coeffArr[2] > 0f && (_operatorLayerCount[0] > 3 || layerCountTest > 1))
        {
            resultString = "(" + resultString + ")";
        }

        string operatorString = "^";
        for (int i=0; i<_operatorLayerCount[0]-3; i++)
        {
            resultString = "10" + operatorString + resultString;
        }

        for (int i=1; i<9; i++)
        {
            operatorString += "^";

            for (int j=0; j<_operatorLayerCount[i]-1; j++)
            {
                resultString = "10" + operatorString + resultString;
            }
        }

        return resultString;
    }

    public UpArrowNotation Copy()
    {
        UpArrowNotation copiedNumber = new UpArrowNotation();

        copiedNumber._top3Layer = _top3Layer.Copy();
        for (int i=0; i<9; i++)
        {
            copiedNumber._operatorLayerCount[i] = _operatorLayerCount[i];
        }

        return copiedNumber;
    }

    private void CheckLayer()
    {
        if (_operatorLayerCount[0] < 4)
        {
            if (_top3Layer._coeffArr[1] != 0f)
            {
                _operatorLayerCount[0] = 2;
            }
            if (_top3Layer._coeffArr[2] != 0f)
            {
                _operatorLayerCount[0] = 3;
            }
        }

        if (_top3Layer._coeffArr[2] >= 10f)
        {
            _top3Layer.AscendLayer();
            _operatorLayerCount[0] += 1;
        }

        if (_operatorLayerCount[0] > 9)
        {
            _operatorLayerCount[1] += 1;
            _operatorLayerCount[0] = 2;
            _top3Layer = new PowerTowerNotation(1f, 1f, 0f);
        }

        for (int i=1; i<8; i++)
        {
            if (_operatorLayerCount[i] > 9)
            {
                _operatorLayerCount[i] = 1;
                _operatorLayerCount[i + 1] += 1;
            }
            else
            {
                break;
            }
        }

        if (_operatorLayerCount[8] > 9)
        {
            // If number gets SOOOOOOOOOOOOO large..??
        }

        if (_top3Layer._coeffArr[2] < 1f && _operatorLayerCount[0] > 3)
        {
            _top3Layer.DescendLayer();
            _operatorLayerCount[0] -= 1;
        }

        // Decrease Layer ...
    }

    public static UpArrowNotation operator +(UpArrowNotation a) => a;

    public static UpArrowNotation operator -(UpArrowNotation a)
    {
        UpArrowNotation result = a.Copy();
        result._top3Layer = -result._top3Layer;
        return result;
    }

    public void Add(UpArrowNotation other)
    {
        _top3Layer += other._top3Layer;
        CheckLayer();
    }

    public void Sub(UpArrowNotation other)
    {

    }

    public void Mul(UpArrowNotation other)
    {
        _top3Layer *= other._top3Layer;
        CheckLayer();
    }

    public void Div(UpArrowNotation other)
    {

    }

    public void Pow(UpArrowNotation other)
    {

    }

    public double CalculatePower()
    {
        double coeff = _top3Layer._coeffArr[1];
        double power = _top3Layer._coeffArr[2];

        return coeff * Math.Pow(10, power);
    }

    public double CalculateFull()
    {
        double coeff = _top3Layer._coeffArr[0];
        double power = this.CalculatePower();

        try
        {
            return coeff * Math.Pow(10, power);
        }
        catch (OverflowException e)
        {
            return double.PositiveInfinity;
        }
    }

    public static bool operator ==(UpArrowNotation a, UpArrowNotation b)
    {
        for (int i=0; i<9; i++)
        {
            if (a._operatorLayerCount[i] != b._operatorLayerCount[i])
            {
                return false;
            }
        }

        return a._top3Layer == b._top3Layer;
    }

    public static bool operator ==(UpArrowNotation a, double b) => a == (new UpArrowNotation(b));

    public static bool operator ==(UpArrowNotation a, int b) => a == (new UpArrowNotation(b));

    public static bool operator ==(double a, UpArrowNotation b) => (new UpArrowNotation(a)) == b;

    public static bool operator ==(int a, UpArrowNotation b) => (new UpArrowNotation(a)) == b;

    public static bool operator !=(UpArrowNotation a, UpArrowNotation b) => !(a == b);

    public static bool operator !=(UpArrowNotation a, double b) => a != (new UpArrowNotation(b));

    public static bool operator !=(UpArrowNotation a, int b) => a != (new UpArrowNotation(b));

    public static bool operator !=(double a, UpArrowNotation b) => (new UpArrowNotation(a)) != b;

    public static bool operator !=(int a, UpArrowNotation b) => (new UpArrowNotation(a)) != b;

    public static bool operator >(UpArrowNotation a, UpArrowNotation b)
    {
        for (int i = 8; i > -1; i--)
        {
            if (a._operatorLayerCount[i] > b._operatorLayerCount[i])
            {
                return true;
            }
        }

        return a._top3Layer > b._top3Layer;
    }

    public static bool operator >(UpArrowNotation a, double b) => a > (new UpArrowNotation(b));

    public static bool operator >(UpArrowNotation a, int b) => a > (new UpArrowNotation(b));

    public static bool operator >(double a, UpArrowNotation b) => (new UpArrowNotation(a)) > b;

    public static bool operator >(int a, UpArrowNotation b) => (new UpArrowNotation(a)) > b;

    public static bool operator <=(UpArrowNotation a, UpArrowNotation b) => !(a > b);

    public static bool operator <=(UpArrowNotation a, double b) => a <= (new UpArrowNotation(b));

    public static bool operator <=(UpArrowNotation a, int b) => a <= (new UpArrowNotation(b));

    public static bool operator <=(double a, UpArrowNotation b) => (new UpArrowNotation(a)) <= b;

    public static bool operator <=(int a, UpArrowNotation b) => (new UpArrowNotation(a)) <= b;

    public static bool operator >=(UpArrowNotation a, UpArrowNotation b) => (a > b) || (a == b);

    public static bool operator >=(UpArrowNotation a, double b) => a >= (new UpArrowNotation(b));

    public static bool operator >=(UpArrowNotation a, int b) => a >= (new UpArrowNotation(b));

    public static bool operator >=(double a, UpArrowNotation b) => (new UpArrowNotation(a)) >= b;

    public static bool operator >=(int a, UpArrowNotation b) => (new UpArrowNotation(a)) >= b;

    public static bool operator <(UpArrowNotation a, UpArrowNotation b) => !(a >= b);

    public static bool operator <(UpArrowNotation a, double b) => a < (new UpArrowNotation(b));

    public static bool operator <(UpArrowNotation a, int b) => a < (new UpArrowNotation(b));

    public static bool operator <(double a, UpArrowNotation b) => (new UpArrowNotation(a)) < b;

    public static bool operator <(int a, UpArrowNotation b) => (new UpArrowNotation(a)) < b;
}
