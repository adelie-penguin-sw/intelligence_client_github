using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;

public class UpArrowNotation
{
    [ShowInInspector] private List<double> _top3Coeffs;
    [ShowInInspector] private int _operatorLayerCount = 0;

    /// <summary>
    /// UpArrowNotation의 최상위 세 계층에 관한 정보를 list 형태로 반환합니다.
    /// </summary>
    public List<double> Top3Layer { get { return _top3Coeffs; } }

    /// <summary>
    /// UpArrowNotation의 나머지 레이어에 관한 정보를 하나의 9자리 정수값으로 반환합니다.
    /// </summary>
    public int OperatorLayerCount { get { return _operatorLayerCount; } }

    private static (double, double) _getCoeffAndPower(double number)
    {
        if (number == 0f)
        {
            return (0f, 0f);
        }

        double power = Math.Floor(Math.Log10(Math.Abs(number)));
        double coeff = number / Math.Pow(10f, power);

        return (coeff, power);
    }

    private void _convert(double number)
    {
        (double top1Coeff, double power) = _getCoeffAndPower(number);
        (double top2Coeff, double top3Coeff) = _getCoeffAndPower(power);

        if (top2Coeff != 0f)
        {
            _operatorLayerCount = 1;
        }
        if (top3Coeff != 0f)
        {
            _operatorLayerCount = 2;
        }

        _top3Coeffs = new List<double> { top1Coeff, top2Coeff, top3Coeff };
    }

    /// <summary>
    /// 파라미터 없이 객체를 생성하면 모든 레이어의 계수 및 레이어값이 모두 0으로 초기화됩니다.
    /// </summary>
    public UpArrowNotation()
    {
        _top3Coeffs = new List<double> { 0f, 0f, 0f };
    }

    /// <summary>
    /// 일반 숫자 타입을 파라미터로 받아 내부적으로 UpArrowNotation 타입으로 변환합니다.
    /// </summary>
    /// <param name="number"></param>
    public UpArrowNotation(double number)
    {
        _convert(number);
    }

    /// <summary>
    /// 일반 숫자 타입을 파라미터로 받아 내부적으로 UpArrowNotation 타입으로 변환합니다.
    /// </summary>
    /// <param name="number"></param>
    public UpArrowNotation(int number)
    {
        _convert(number);
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
    public UpArrowNotation(double layer1Coeff, double layer2Coeff, double layer3Coeff = 0f, int operatorLayerCounts = 0)
    {
        // 레이어 카운팅 넘버는 최대 아홉 자리 정수까지만 허용된다
        if (operatorLayerCounts > 999999999)
        {
            throw new ArgumentOutOfRangeException("operatorlayercount can have maximum 9 digits");
        }

        // 레이어 카운팅 넘버는 0과 자연수만 허용된다
        if (operatorLayerCounts < 0)
        {
            throw new ArgumentOutOfRangeException("operatorlayercount cannot be negative");
        }

        // 모든 coeff 파라미터의 절댓값은 10 이상일 수 없다
        if (Math.Abs(layer1Coeff) >= 10f || Math.Abs(layer2Coeff) >= 10f || Math.Abs(layer3Coeff) >= 10f)
        {
            throw new ArgumentOutOfRangeException("abs value of each coefficients cannot exceed 10");
        }

        // 모든 coeff 파라미터의 절댓값은 1보다 작을 수 없다
        if (Math.Abs(layer1Coeff) < 1f && layer1Coeff != 0f || Math.Abs(layer2Coeff) < 1f && layer2Coeff != 0f || Math.Abs(layer3Coeff) < 1f && layer3Coeff != 0f)
        {
            throw new ArgumentOutOfRangeException("abs value of each coefficients cannot be lower than 1");
        }

        // 세 번째 coeff 파라미터는 음수가 될 수 없다
        if (layer3Coeff < 0f)
        {
            throw new ArgumentOutOfRangeException("the third coefficient must be nonnegative");
        }

        _top3Coeffs = new List<double> { layer1Coeff, layer2Coeff, layer3Coeff };
        if (_top3Coeffs[0] == 0f)
        {
            _top3Coeffs[1] = 0f;
            _top3Coeffs[2] = 0f;
        }
        else if (_top3Coeffs[1] == 0f)
        {
            _top3Coeffs[2] = 0f;
        }

        int expoLayerCount = operatorLayerCounts % 10;

        if (expoLayerCount < 3)
        {
            operatorLayerCounts /= 10;
            operatorLayerCounts *= 10;

            if (layer2Coeff >= 1f)
            {
                operatorLayerCounts++;
            }
            if (layer3Coeff >= 1f)
            {
                operatorLayerCounts++;
            }
        }

        _operatorLayerCount = operatorLayerCounts;
    }

    /// <summary>
    /// 모든 필드를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public (double, double, double, int) GetFields()
    {
        return (_top3Coeffs[0], _top3Coeffs[1], _top3Coeffs[2], _operatorLayerCount);
    }

    /// <summary>
    /// 화면에 출력하는 형식을 결정하여 문자열화합니다.
    /// </summary>
    /// <returns>문자열화된 숫자표현식</returns>
    public string ToString(ECurrencyType displayType = ECurrencyType.NORMAL)
    {
        if (_top3Coeffs[2] == 0f)
        {
            switch (displayType)
            {
                case ECurrencyType.NORMAL:
                    if (_top3Coeffs[1] >= 0f)
                    {
                        return (_top3Coeffs[0] * Math.Pow(10, _top3Coeffs[1])).ToString("N0");
                    }
                    else
                    {
                        return (_top3Coeffs[0] * Math.Pow(10, _top3Coeffs[1])).ToString("N" + (-_top3Coeffs[1]).ToString("N0"));
                    }
                case ECurrencyType.INTELLECT:
                    return (_top3Coeffs[0] * Math.Pow(10, _top3Coeffs[1])).ToString("N0");
                case ECurrencyType.MULTIPLIER:
                case ECurrencyType.NP:
                    return (_top3Coeffs[0] * Math.Pow(10, _top3Coeffs[1])).ToString("N" + (_top3Coeffs[1] < 4 ? "2" : "0"));
                case ECurrencyType.TP:
                    return Math.Floor(_top3Coeffs[0] * Math.Pow(10, _top3Coeffs[1])).ToString("N0");
            }
        }

        string powerString = (_top3Coeffs[1] * Math.Pow(10, _top3Coeffs[2])).ToString("N0");
        string coeffString = _top3Coeffs[0].ToString("N2");
        string resultString = coeffString + "x10^" + powerString;

        int layerCountTest = 1;
        int layerCount = _operatorLayerCount / 10;

        for (int i = 0; i < 8; i++)
        {
            layerCountTest *= layerCount % 10;
            layerCount /= 10;
        }

        if (_top3Coeffs[2] > 0.0 && (_operatorLayerCount % 10 > 3 || layerCountTest > 1))
        {
            resultString = "(" + resultString + ")";
        }

        string operatorString = "^";
        for (int i = 0; i < _operatorLayerCount % 10 - 3; i++)
        {
            resultString = "10" + operatorString + resultString;
        }

        int higherLevelCounts = _operatorLayerCount / 10;
        for (int i = 0; i < 8; i++)
        {
            operatorString += "^";

            for (int j = 0; j < higherLevelCounts % 10; j++)
            {
                resultString = "10" + operatorString + resultString;
            }
            higherLevelCounts /= 10;
        }

        return resultString;
    }

    /// <summary>
    /// 최상위 2계층까지의 실제 값을 계산합니다.
    /// </summary>
    /// <returns></returns>
    public double CalculateTop2Layer()
    {
        return _top3Coeffs[1] * Math.Pow(10f, _top3Coeffs[2]);
    }

    /// <summary>
    /// 최상위 3계층까지의 실제 값을 계산합니다. double타입 한계를 넘어서면 무한대를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public double CalculateTop3Layer()
    {
        try
        {
            double power = _top3Coeffs[1] * Math.Pow(10f, _top3Coeffs[2]);
            return _top3Coeffs[0] * Math.Pow(10f, power);
        }
        catch (OverflowException e)
        {
            return double.PositiveInfinity;
        }
    }

    /// <summary>
    /// 완전히 똑같은 값을 갖는 새로운 객체를 복사하여 반환합니다.
    /// </summary>
    /// <returns>동일한 값을 갖는 새 객체</returns>
    public UpArrowNotation Copy()
    {
        UpArrowNotation copiedNumber = new UpArrowNotation();

        for (int i = 0; i < 3; i++)
        {
            copiedNumber._top3Coeffs[i] = _top3Coeffs[i];
        }
        copiedNumber._operatorLayerCount = _operatorLayerCount;

        return copiedNumber;
    }

    private void _ascendLayer()
    {
        _operatorLayerCount++;
        int expoLayerCount = _operatorLayerCount % 10;

        if (expoLayerCount == 0)
        {
            _operatorLayerCount++;
            _top3Coeffs = new List<double> { 1.1f, 1f, 0f };
            return;
        }

        (double c, double p) = _getCoeffAndPower(_top3Coeffs[expoLayerCount - 1]);

        _top3Coeffs[0] = _top3Coeffs[1];
        _top3Coeffs[1] = c;
        _top3Coeffs[2] = p;
    }

    private void _descendLayer()
    {
        int expoLayerCount = _operatorLayerCount % 10;
        _operatorLayerCount--;


        if (_operatorLayerCount >= 10 && expoLayerCount == 0)
        {
            _operatorLayerCount--;
            _top3Coeffs = new List<double> { 9.9999999999f, 9.999999999f, 9f };
            return;
        }

        double power = _top3Coeffs[1] * Math.Pow(10f, _top3Coeffs[2]);
        _top3Coeffs[2] = power;
        _top3Coeffs[expoLayerCount] = 9.9999999999f;
    }

    private void _checkLayer()
    {
	    if (_top3Coeffs[0] == 0f) {
            _top3Coeffs[1] = 0f;
            _top3Coeffs[2] = 0f;
            _operatorLayerCount = 0;
            return;
	    }

	    if (_top3Coeffs[1] == 0f) {
            _top3Coeffs[2] = 0f;
            _operatorLayerCount = 0;
	    }

        int expoLayerCount = _operatorLayerCount % 10;

        if (expoLayerCount < 3)
        {
            _operatorLayerCount /= 10;
            _operatorLayerCount *= 10;

            if (_top3Coeffs[1] != 0f)
            {
                _operatorLayerCount++;

            }
            if (_top3Coeffs[2] != 0f)
            {
                _operatorLayerCount++;
            }
        }

        if (_top3Coeffs[2] >= 10f)
        {
            _ascendLayer();
        }

        if (_operatorLayerCount >= 10 && _operatorLayerCount % 10 == 0)
        {
            _operatorLayerCount++;
            _top3Coeffs = new List<double> { 1.1f, 1f, 0f };
        }

        // if uan.operatorLayerCount >= 1000000000 {
        // 	// If number gets SOOOOOOOOOOOOO large..??
        // }

        if (_top3Coeffs[2] < 1f && _operatorLayerCount % 10 > 2)
        {
            _descendLayer();
        }
    }

    private static UpArrowNotation _reciprocated(UpArrowNotation uan)
    {
        UpArrowNotation result = uan.Copy();

        result._top3Coeffs[1] *= -1;

        if (result._top3Coeffs[0] == 1)
        {
            return result;
        }

        if (result._top3Coeffs[0] == 0)
        {
            throw new DivideByZeroException("division by zero");
        }

        result._top3Coeffs[0] = 10f / result._top3Coeffs[0];

        double resultPower = result._top3Coeffs[1] * Math.Pow(10f, result._top3Coeffs[2]);
        resultPower--;

        (double c, double p) = _getCoeffAndPower(resultPower);

        result._top3Coeffs[1] = c;
        result._top3Coeffs[2] = p;

        return result;
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
        result._top3Coeffs[0] *= -1;
        return result;
    }

    /// <summary>
    /// 자기 자신을 other만큼 증가시킵니다. += 연산 대신 사용합니다.
    /// </summary>
    /// <param name="other"></param>
    public void Add(UpArrowNotation other)
    {
        double coeff = _top3Coeffs[0];
        double power = Math.Round(_top3Coeffs[1] * Math.Pow(10f, _top3Coeffs[2]));
        double otherCoeff = other._top3Coeffs[0];
        double otherPower = Math.Round(other._top3Coeffs[1] * Math.Pow(10f, other._top3Coeffs[2]));

        double powerDiff = Math.Abs(power - otherPower);
        double resultPower;

        if (power >= otherPower)
        {
            _top3Coeffs[0] = coeff + otherCoeff / Math.Pow(10.0, powerDiff);
            resultPower = power;
        }
        else
        {
            _top3Coeffs[0] = otherCoeff + coeff / Math.Pow(10.0, powerDiff);
            resultPower = otherPower;
        }

        if (_top3Coeffs[0] == 0)
        {
            return;
        }

        if (Math.Abs(_top3Coeffs[0]) >= 10)
        {
            _top3Coeffs[0] /= 10f;
            resultPower += 1f;
        }

        while (Math.Abs(_top3Coeffs[0]) < 1f)
        {
            _top3Coeffs[0] *= 10f;
            resultPower -= 1f;
        }

        if (Math.Abs(resultPower) >= 10)
        {
            _top3Coeffs[2] = Math.Floor(Math.Log10(Math.Abs(resultPower)));
            _top3Coeffs[1] = resultPower / Math.Pow(10f, _top3Coeffs[2]);
        }
        else
        {
            _top3Coeffs[2] = 0f;
            _top3Coeffs[1] = resultPower;
        }

        _checkLayer();
    }

    /// <summary>
    /// 자기 자신을 other만큼 감소시킵니다. -= 연산 대신 사용합니다.
    /// </summary>
    /// <param name="other"></param>
    public void Sub(UpArrowNotation other)
    {
        Add(-other);
    }

    /// <summary>
    /// 자기 자신을 other배만큼 증가시킵니다. *= 연산 대신 사용합니다.
    /// </summary>
    /// <param name="other"></param>
    public void Mul(UpArrowNotation other)
    {
        double coeff = _top3Coeffs[0];
        double power = Math.Round(_top3Coeffs[1] * Math.Pow(10f, _top3Coeffs[2]));
        double otherCoeff = other._top3Coeffs[0];
        double otherPower = Math.Round(other._top3Coeffs[1] * Math.Pow(10f, other._top3Coeffs[2]));

        double resultPower = power + otherPower;

        _top3Coeffs[0] = coeff * otherCoeff;

        if (_top3Coeffs[0] == 0)
        {
            return;
        }

        if (Math.Abs(_top3Coeffs[0]) >= 10f)
        {
            _top3Coeffs[0] /= 10f;
            resultPower += 1f;
        }

        while (Math.Abs(_top3Coeffs[0]) < 1f)
        {
            _top3Coeffs[0] *= 10f;
            resultPower -= 1f;
        }

        if (Math.Abs(resultPower) >= 10f)
        {
            _top3Coeffs[2] = Math.Floor(Math.Log10(Math.Abs(resultPower)));
            _top3Coeffs[1] = resultPower / Math.Pow(10f, _top3Coeffs[2]);
        }
        else
        {
            _top3Coeffs[2] = 0f;
            _top3Coeffs[1] = resultPower;
        }

        _checkLayer();
    }

    /// <summary>
    /// 자기 자신을 other로 나눕니다. /= 연산 대신 사용합니다.
    /// </summary>
    /// <param name="other"></param>
    public void Div(UpArrowNotation other)
    {
        Mul(_reciprocated(other));
    }

    /// <summary>
    /// 자기 자신에 other를 지수로 올립니다. ^= 연산 대신 사용합니다.
    /// </summary>
    /// <param name="other"></param>
    public void Pow(UpArrowNotation other)
    {
        if (_top3Coeffs[0] == 0.0 && other._top3Coeffs[0] == 0.0)
        {
            throw new DivideByZeroException("base and exponent cannot be 0 at once");
        }

        if (_top3Coeffs[0] == 0.0)
        {
            return;
        }

        if (other._top3Coeffs[0] == 0.0)
        {
            _top3Coeffs[0] = 1.0;
            _top3Coeffs[1] = 0.0;
            _top3Coeffs[2] = 0.0;
            _operatorLayerCount = 0;
            return;
        }

        double coeff = _top3Coeffs[0];
        double power = Math.Round(_top3Coeffs[1] * Math.Pow(10.0, _top3Coeffs[2]));

        double otherCoeff = other._top3Coeffs[0];

        double otherPower = Math.Round(other._top3Coeffs[1] * Math.Pow(10.0, other._top3Coeffs[2]));


        if (coeff == 1.0 || otherPower >= 10.0)
        {
            _top3Coeffs[0] = 1.0;

            (double c, double p) = _getCoeffAndPower(power * otherCoeff);
            _top3Coeffs[1] = c;

            otherPower += p;
            _top3Coeffs[2] = otherPower;
        }
        else
        {
            double powerOfCoeff = Math.Log10(coeff) * otherCoeff * Math.Pow(10.0, otherPower);
            double additionalPower = Math.Floor(powerOfCoeff);
            double tempFrac = powerOfCoeff - additionalPower;

            _top3Coeffs[0] = Math.Pow(10.0, tempFrac);

            additionalPower /= Math.Pow(10.0, otherPower);

            (double c, double p) = _getCoeffAndPower(additionalPower + power * otherCoeff);
            _top3Coeffs[1] = c;

            otherPower += p;
            _top3Coeffs[2] = otherPower;
        }

        _checkLayer();
    }

    public static UpArrowNotation Log(UpArrowNotation a, UpArrowNotation b)
    {
        return new UpArrowNotation(Log10Top3Layer(a) / Log10Top3Layer(b));
    }

    public static UpArrowNotation Log(float a, UpArrowNotation b)
    {
        return new UpArrowNotation(Math.Log10(a) / Log10Top3Layer(b));
    }

    public static UpArrowNotation Log(int a, UpArrowNotation b)
    {
        return new UpArrowNotation(Math.Log10(a) / Log10Top3Layer(b));
    }

    public static UpArrowNotation Log(UpArrowNotation a, float b)
    {
        return new UpArrowNotation(Log10Top3Layer(a) / Math.Log10(b));
    }

    public static UpArrowNotation Log(UpArrowNotation a, int b)
    {
        return new UpArrowNotation(Log10Top3Layer(a) / Math.Log10(b));
    }



    public static double Log10Top3Layer(UpArrowNotation uan)
    {
        if (uan._top3Coeffs[0] <= 0.0)
        {
            throw new ArithmeticException("logarithm for 0");
        }
        double power = uan.CalculateTop2Layer();
        double logCoeff = Math.Log10(uan._top3Coeffs[0]);
        return logCoeff + power;
    }

    public static UpArrowNotation Slog10(UpArrowNotation uan)
    {
        if (uan.OperatorLayerCount > 9)
        {
            throw new Exception("Expressions with tetration or higher are not allowed");
        }

        double topLayerValue;
        List<double> top3Layer = uan.Top3Layer;

        switch (uan.OperatorLayerCount)
        {
            case 0:
                topLayerValue = top3Layer[0];
                break;
            case 1:
                topLayerValue = top3Layer[1] + Math.Log10(top3Layer[0]);
                break;
            default:
                topLayerValue = top3Layer[2] + Math.Log10(top3Layer[1]);
                break;
        }

        double ln10 = Math.Log(10);
        double fracPart = Math.Log(Math.Log10(topLayerValue) * (ln10 - 1) + 1, ln10);

        return new UpArrowNotation(uan.OperatorLayerCount + fracPart);
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
        return a._top3Coeffs[0] == b._top3Coeffs[0] && a._top3Coeffs[1] == b._top3Coeffs[1] && a._top3Coeffs[2] == b._top3Coeffs[2] && a._operatorLayerCount == b._operatorLayerCount;
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
        double aCoeff = a._top3Coeffs[0];
        double bCoeff = b._top3Coeffs[0];

	    // 두 수의 부호가 다르거나 0일 때
	    if (aCoeff*bCoeff <= 0f) {
		    if (aCoeff <= 0f && bCoeff >= 0f) {
                return false;
		    } else {
                return true;
		    }
	    }

        // power값 직접 계산
        double aPower = a.CalculateTop2Layer();
        double bPower = b.CalculateTop2Layer();

	    // 두 수의 부호가 모두 양일 때
	    if (aCoeff > 0f) {
		    if (aPower > bPower) {
                return true;
		    } else if (aPower < bPower) {
                return false;
		    }
            return aCoeff > bCoeff;
	    } else { // 두 수의 부호가 모두 음일 때
		    if (aPower > bPower) {
                return false;
		    } else if (aPower < bPower) {
                return true;
		    }
            return aCoeff > bCoeff;
	    }
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

public enum ECurrencyType
{
    NORMAL, 
    INTELLECT,
    MULTIPLIER, 
    NP,
    TP, 
}
