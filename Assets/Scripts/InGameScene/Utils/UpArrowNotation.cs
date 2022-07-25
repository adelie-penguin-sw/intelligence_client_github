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
}
