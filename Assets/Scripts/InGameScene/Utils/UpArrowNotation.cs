using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpArrowNotation
{
    private PowerTowerNotation _top3Layer;
    public int _operatorLayerCount = 0;

    public PowerTowerNotation Top3Layer { get { return _top3Layer; } }

    private void Convert(double number)
    {
        _top3Layer = new PowerTowerNotation(number);

        if (_top3Layer._top2Coeff != 0f)
        {
            _operatorLayerCount = 1;
        }
        if (_top3Layer._top3Coeff != 0f)
        {
            _operatorLayerCount = 2;
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

    public UpArrowNotation(double layer1Coeff, double layer2Coeff, double layer3Coeff, int operatorLayerCounts = 0)
    {
        _top3Layer = new PowerTowerNotation(layer1Coeff, layer2Coeff, layer3Coeff);

        int expoLayerCount = operatorLayerCounts % 10;

        if (expoLayerCount < 3)
        {
            operatorLayerCounts /= 10;
            operatorLayerCounts *= 10;

            if (layer2Coeff >= 1f)
            {
                operatorLayerCounts += 1;
            }
            if (layer3Coeff >= 1f) {
                operatorLayerCounts += 1;
            }
        }

        _operatorLayerCount = operatorLayerCounts;
    }

    public override string ToString()
    {
        string resultString = _top3Layer.ToString();

        int layerCountTest = 1;
        int layerCount = _operatorLayerCount / 10;

        for (int i = 0; i < 8; i++) {
            layerCountTest *= layerCount % 10;
            layerCount /= 10;
        }

        if (_top3Layer._top3Coeff > 0.0 && (_operatorLayerCount % 10 > 3 || layerCountTest > 1)) {
            resultString = "(" + resultString + ")";
        }

        string operatorString = "^";
        for (int i = 0; i < _operatorLayerCount % 10 - 3; i++) {
            resultString = "10" + operatorString + resultString;
        }

        int higherLevelCounts = _operatorLayerCount / 10;
        for (int i = 0; i < 8; i++) {
            operatorString += "^";

            for (int j = 0; j < higherLevelCounts % 10; j++) {
                resultString = "10" + operatorString + resultString;
            }
            higherLevelCounts /= 10;
        }

        return resultString;
    }

    public UpArrowNotation Copy()
    {
        UpArrowNotation copiedNumber = new UpArrowNotation();

        copiedNumber._top3Layer = _top3Layer.Copy();
        copiedNumber._operatorLayerCount = _operatorLayerCount;

        return copiedNumber;
    }

    public double CalculatePower()
    {
        return _top3Layer._top2Coeff * Math.Pow(10.0, _top3Layer._top3Coeff);
    }

    public double CalculateFull()
    {
        try
        {
            double power = _top3Layer._top2Coeff * Math.Pow(10.0, _top3Layer._top3Coeff);
            return _top3Layer._top1Coeff * Math.Pow(10.0, power);
        }
        catch (OverflowException e)
        {
            return double.PositiveInfinity;
        }
    }

    private void CheckLayer()
    {
        int expoLayerCount = _operatorLayerCount % 10;


        if (expoLayerCount < 3) {
            _operatorLayerCount /= 10;
            _operatorLayerCount *= 10;

            if (_top3Layer._top2Coeff != 0.0) {
                _operatorLayerCount += 1;
            }
            if (_top3Layer._top3Coeff != 0.0) {
                _operatorLayerCount += 1;
            }
        }

        if (_top3Layer._top3Coeff >= 10.0) {
            _top3Layer.AscendLayer();
            _operatorLayerCount += 1;
        }

        if (_operatorLayerCount % 10 == 0) {
            _operatorLayerCount += 1;
            _top3Layer = new PowerTowerNotation(1.1f, 1.0f, 0f);
        }

        // if uan.operatorLayerCount >= 1000000000 {
        // 	// If number gets SOOOOOOOOOOOOO large..??
        // }

        if (_top3Layer._top3Coeff < 1.0 && _operatorLayerCount % 10 > 2) {
            _top3Layer.DescendLayer();
            _operatorLayerCount -= 1;
        }

        int totalLayerCounts = 0;
        int higherLevelCounts = _operatorLayerCount / 10;

        for (int i = 0; i < 8; i++) {
            totalLayerCounts += higherLevelCounts % 10;
            higherLevelCounts /= 10;
        }

        if (totalLayerCounts > 0 && _operatorLayerCount % 10 == 0) {
            _operatorLayerCount -= 1;
            _top3Layer = new PowerTowerNotation(9.999f, 9.999f, 9f);
            _operatorLayerCount -= 1;
        }
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
        _top3Layer -= other._top3Layer;
        CheckLayer();
    }

    public void Mul(UpArrowNotation other)
    {
        _top3Layer *= other._top3Layer;
        CheckLayer();
    }

    public void Div(UpArrowNotation other)
    {
        _top3Layer /= other._top3Layer;
        CheckLayer();
    }

    public void Pow(UpArrowNotation other)
    {
        _top3Layer ^= other._top3Layer;
        CheckLayer();
    }

    public static bool operator ==(UpArrowNotation a, UpArrowNotation b)
    {
        return a._top3Layer == b._top3Layer && a._operatorLayerCount == b._operatorLayerCount;
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
        if (a._operatorLayerCount > b._operatorLayerCount)
        {
            return true;
        }
        else if (a._operatorLayerCount < b._operatorLayerCount)
        {
            return false;
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
