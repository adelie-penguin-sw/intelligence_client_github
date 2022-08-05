using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpArrowNotation
{
    private PowerTowerNotation _top3Layer;
    private int _operatorLayerCount = 0;

    /// <summary>
    /// UpArrowNotation의 최상위 세 계층에 관한 정보를 PowerTowerNotation의 형태로 반환합니다.
    /// </summary>
    public PowerTowerNotation Top3Layer { get { return _top3Layer; } }

    /// <summary>
    /// UpArrowNotation의 나머지 레이어에 관한 정보를 하나의 9자리 정수값으로 반환합니다.
    /// </summary>
    public int OperatorLayerCount { get { return _operatorLayerCount; } }

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

    /// <summary>
    /// 파라미터 없이 객체를 생성하면 모든 레이어의 계수 및 레이어값이 모두 0으로 초기화됩니다.
    /// </summary>
    public UpArrowNotation()
    {
        _top3Layer = new PowerTowerNotation();
    }

    /// <summary>
    /// 일반 숫자 타입을 파라미터로 받아 내부적으로 UpArrowNotation 타입으로 변환합니다.
    /// </summary>
    /// <param name="number"></param>
    public UpArrowNotation(double number)
    {
        Convert(number);
    }

    /// <summary>
    /// 일반 숫자 타입을 파라미터로 받아 내부적으로 UpArrowNotation 타입으로 변환합니다.
    /// </summary>
    /// <param name="number"></param>
    public UpArrowNotation(int number)
    {
        Convert(number);
    }

    /// <summary>
    /// 세 개의 실수를 입력으로 받아 이들을 최상위 세 레이어의 계수로 갖는 객체를 생성합니다. 세 입력의 절댓값은 10 이상이 될 수 없습니다.
    /// 특정 파라미터가 0이면 이후의 모든 파라미터 또한 0이 됩니다.
    /// 추가적으로 최대 9자리의 정수 하나를 더 받아 나머지 계층의 정보를 생성합니다.
    /// </summary>
    /// <param name="layer1Coeff"></param>
    /// <param name="layer2Coeff"></param>
    /// <param name="layer3Coeff"></param>
    /// <param name="operatorLayerCounts"></param>
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
    }

    /// <summary>
    /// 화면에 출력하는 형식을 결정하여 문자열화합니다.
    /// </summary>
    /// <returns>문자열화된 숫자표현식</returns>
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

    /// <summary>
    /// 완전히 똑같은 값을 갖는 새로운 객체를 복사하여 반환합니다.
    /// </summary>
    /// <returns>동일한 값을 갖는 새 객체</returns>
    public UpArrowNotation Copy()
    {
        UpArrowNotation copiedNumber = new UpArrowNotation();

        copiedNumber._top3Layer = _top3Layer.Copy();
        copiedNumber._operatorLayerCount = _operatorLayerCount;

        return copiedNumber;
    }

    /// <summary>
    /// 최상위 두 계층까지의 실제 값을 계산하여 반환합니다.
    /// </summary>
    /// <returns></returns>
    public double CalculatePower()
    {
        return _top3Layer._top2Coeff * Math.Pow(10.0, _top3Layer._top3Coeff);
    }

    /// <summary>
    /// 최상위 세 계층까지의 실제 값을 계산하여 반환합니다. 계산값이 double의 표현가능범위를 초과할 경우 PositiveInfinity를 대신 반환합니다.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 양의 부호에 해당하는 단항 연산으로, 자기 자신을 반환합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <returns>자기 자신을 그대로 반환</returns>
    public static UpArrowNotation operator +(UpArrowNotation a) => a;

    /// <summary>
    /// 음의 부호에 해당하는 단항 연산으로, 최상위 세 계층 중 가장 아래층 계수의 부호를 뒤집은 새 객체를 반환합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <returns>부호가 바뀐 새 객처</returns>
    public static UpArrowNotation operator -(UpArrowNotation a)
    {
        UpArrowNotation result = a.Copy();
        result._top3Layer = -result._top3Layer;
        return result;
    }

    /// <summary>
    /// 자기 자신을 other만큼 증가시킵니다. += 연산 대신 사용합니다.
    /// </summary>
    /// <param name="other"></param>
    public void Add(UpArrowNotation other)
    {
        _top3Layer += other._top3Layer;
        CheckLayer();
    }

    /// <summary>
    /// 자기 자신을 other만큼 감소시킵니다. -= 연산 대신 사용합니다.
    /// </summary>
    /// <param name="other"></param>
    public void Sub(UpArrowNotation other)
    {
        _top3Layer -= other._top3Layer;
        CheckLayer();
    }

    /// <summary>
    /// 자기 자신을 other배만큼 증가시킵니다. *= 연산 대신 사용합니다.
    /// </summary>
    /// <param name="other"></param>
    public void Mul(UpArrowNotation other)
    {
        _top3Layer *= other._top3Layer;
        CheckLayer();
    }

    /// <summary>
    /// 자기 자신을 other로 나눕니다. /= 연산 대신 사용합니다.
    /// </summary>
    /// <param name="other"></param>
    public void Div(UpArrowNotation other)
    {
        _top3Layer /= other._top3Layer;
        CheckLayer();
    }

    /// <summary>
    /// 자기 자신에 other를 지수로 올립니다. ^= 연산 대신 사용합니다.
    /// </summary>
    /// <param name="other"></param>
    public void Pow(UpArrowNotation other)
    {
        _top3Layer ^= other._top3Layer;
        CheckLayer();
    }

    /// <summary>
    /// 두 수의 합에 해당하는 새 객체를 반환합니다. UpArrowNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>합</returns>
    public static UpArrowNotation operator +(UpArrowNotation a, UpArrowNotation b)
    {
        UpArrowNotation result = a.Copy();
        result.Add(b);
        return result;
    }

    public static UpArrowNotation operator +(UpArrowNotation a, double b) => a + new UpArrowNotation(b);

    public static UpArrowNotation operator +(UpArrowNotation a, int b) => a + new UpArrowNotation(b);

    public static UpArrowNotation operator +(double a, UpArrowNotation b) => new UpArrowNotation(a) + b;

    public static UpArrowNotation operator +(int a, UpArrowNotation b) => new UpArrowNotation(a) + b;

    /// <summary>
    /// 두 수의 차에 해당하는 새 객체를 반환합니다. UpArrowNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>차</returns>
    public static UpArrowNotation operator -(UpArrowNotation a, UpArrowNotation b)
    {
        UpArrowNotation result = a.Copy();
        result.Sub(b);
        return result;
    }

    public static UpArrowNotation operator -(UpArrowNotation a, double b) => a - new UpArrowNotation(b);

    public static UpArrowNotation operator -(UpArrowNotation a, int b) => a - new UpArrowNotation(b);

    public static UpArrowNotation operator -(double a, UpArrowNotation b) => new UpArrowNotation(a) - b;

    public static UpArrowNotation operator -(int a, UpArrowNotation b) => new UpArrowNotation(a) - b;

    /// <summary>
    /// 두 수의 곱에 해당하는 새 객체를 반환합니다. UpArrowNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>곱</returns>
    public static UpArrowNotation operator *(UpArrowNotation a, UpArrowNotation b)
    {
        UpArrowNotation result = a.Copy();
        result.Mul(b);
        return result;
    }

    public static UpArrowNotation operator *(UpArrowNotation a, double b) => a * new UpArrowNotation(b);

    public static UpArrowNotation operator *(UpArrowNotation a, int b) => a * new UpArrowNotation(b);

    public static UpArrowNotation operator *(double a, UpArrowNotation b) => new UpArrowNotation(a) * b;

    public static UpArrowNotation operator *(int a, UpArrowNotation b) => new UpArrowNotation(a) * b;

    /// <summary>
    /// 첫째 파라미터를 둘째 파라미터로 나눈 값에 해당하는 새 객체를 반환합니다. UpArrowNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>몫</returns>
    public static UpArrowNotation operator /(UpArrowNotation a, UpArrowNotation b)
    {
        UpArrowNotation result = a.Copy();
        result.Div(b);
        return result;
    }

    public static UpArrowNotation operator /(UpArrowNotation a, double b) => a / new UpArrowNotation(b);

    public static UpArrowNotation operator /(UpArrowNotation a, int b) => a / new UpArrowNotation(b);

    public static UpArrowNotation operator /(double a, UpArrowNotation b) => new UpArrowNotation(a) / b;

    public static UpArrowNotation operator /(int a, UpArrowNotation b) => new UpArrowNotation(a) / b;

    /// <summary>
    /// 첫째 파라미터를 밑수, 둘째 파라미터를 지수로 두는 지수 연산식의 계산값에 해당하는 새 객체를 반환합니다. UpArrowNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>지수 연산값</returns>
    public static UpArrowNotation operator ^(UpArrowNotation a, UpArrowNotation b)
    {
        UpArrowNotation result = a.Copy();
        result.Pow(b);
        return result;
    }

    public static UpArrowNotation operator ^(UpArrowNotation a, double b) => a ^ new UpArrowNotation(b);

    public static UpArrowNotation operator ^(UpArrowNotation a, int b) => a ^ new UpArrowNotation(b);

    public static UpArrowNotation operator ^(double a, UpArrowNotation b) => new UpArrowNotation(a) ^ b;

    public static UpArrowNotation operator ^(int a, UpArrowNotation b) => new UpArrowNotation(a) ^ b;

    /// <summary>
    /// 두 파라미터의 모든 속성이 같은지에 대한 진리값을 반환합니다. 타입이 다른 경우 UpArrowNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>두 파라미터가 같은지에 대한 진리값</returns>
    public static bool operator ==(UpArrowNotation a, UpArrowNotation b)
    {
        return a._top3Layer == b._top3Layer && a._operatorLayerCount == b._operatorLayerCount;
    }

    public static bool operator ==(UpArrowNotation a, double b) => a == (new UpArrowNotation(b));

    public static bool operator ==(UpArrowNotation a, int b) => a == (new UpArrowNotation(b));

    public static bool operator ==(double a, UpArrowNotation b) => (new UpArrowNotation(a)) == b;

    public static bool operator ==(int a, UpArrowNotation b) => (new UpArrowNotation(a)) == b;

    /// <summary>
    /// 두 파라미터의 속성 중 다른 것이 있는지에 대한 진리값을 반환합니다. 타입이 다른 경우 UpArrowNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>두 파라미터가 같은지에 대한 진리값</returns>
    public static bool operator !=(UpArrowNotation a, UpArrowNotation b) => !(a == b);

    public static bool operator !=(UpArrowNotation a, double b) => a != (new UpArrowNotation(b));

    public static bool operator !=(UpArrowNotation a, int b) => a != (new UpArrowNotation(b));

    public static bool operator !=(double a, UpArrowNotation b) => (new UpArrowNotation(a)) != b;

    public static bool operator !=(int a, UpArrowNotation b) => (new UpArrowNotation(a)) != b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 큰지에 대한 진리값을 반환합니다. 타입이 다른 경우 UpArrowNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 큰지에 대한 진리값</returns>
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

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 작거나 같은지에 대한 진리값을 반환합니다. 타입이 다른 경우 UpArrowNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 큰지에 대한 진리값</returns>
    public static bool operator <=(UpArrowNotation a, UpArrowNotation b) => !(a > b);

    public static bool operator <=(UpArrowNotation a, double b) => a <= (new UpArrowNotation(b));

    public static bool operator <=(UpArrowNotation a, int b) => a <= (new UpArrowNotation(b));

    public static bool operator <=(double a, UpArrowNotation b) => (new UpArrowNotation(a)) <= b;

    public static bool operator <=(int a, UpArrowNotation b) => (new UpArrowNotation(a)) <= b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 크거나 같은지에 대한 진리값을 반환합니다. 타입이 다른 경우 UpArrowNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 큰지에 대한 진리값</returns>
    public static bool operator >=(UpArrowNotation a, UpArrowNotation b) => (a > b) || (a == b);

    public static bool operator >=(UpArrowNotation a, double b) => a >= (new UpArrowNotation(b));

    public static bool operator >=(UpArrowNotation a, int b) => a >= (new UpArrowNotation(b));

    public static bool operator >=(double a, UpArrowNotation b) => (new UpArrowNotation(a)) >= b;

    public static bool operator >=(int a, UpArrowNotation b) => (new UpArrowNotation(a)) >= b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 작은지에 대한 진리값을 반환합니다. 타입이 다른 경우 UpArrowNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 큰지에 대한 진리값</returns>
    public static bool operator <(UpArrowNotation a, UpArrowNotation b) => !(a >= b);

    public static bool operator <(UpArrowNotation a, double b) => a < (new UpArrowNotation(b));

    public static bool operator <(UpArrowNotation a, int b) => a < (new UpArrowNotation(b));

    public static bool operator <(double a, UpArrowNotation b) => (new UpArrowNotation(a)) < b;

    public static bool operator <(int a, UpArrowNotation b) => (new UpArrowNotation(a)) < b;
}
