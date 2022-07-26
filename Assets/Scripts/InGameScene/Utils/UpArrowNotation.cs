using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpArrowNotation
{
    public PowerTowerNotation _top3Layer;
    public int[] _operatorLayerCount = {1, 1, 1, 1, 1, 1, 1, 1, 1};

    private void Convert(float number)
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

    public UpArrowNotation(float number)
    {
        Convert(number);
    }

    public UpArrowNotation(int number)
    {
        Convert(number);
    }

    public UpArrowNotation(float layer1Coeff, float layer2Coeff, float layer3Coeff, int operatorLayerCounts)
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

    public static UpArrowNotation operator +(UpArrowNotation a) => a;

    public static UpArrowNotation operator -(UpArrowNotation a)
    {
        UpArrowNotation result = a.Copy();
        result._top3Layer = -result._top3Layer;
        return result;
    }
}
