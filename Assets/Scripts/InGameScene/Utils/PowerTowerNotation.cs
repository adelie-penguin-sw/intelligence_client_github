using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerTowerNotation
{
    private static double _coeffMax = 10f;

    public double _top1Coeff = 0f;
    public double _top2Coeff = 0f;
    public double _top3Coeff = 0f;

    struct CoeffAndPower
    {
        public double coeff;
        public double power;
    }

    private static CoeffAndPower Decompose(double number)
    {
        CoeffAndPower cap = new CoeffAndPower();

        if (number == 0f)
        {
            cap.coeff = 0f;
            cap.power = 0f;
            return cap;
        }

        cap.power = Math.Floor(Math.Log10(Math.Abs(number)));
        cap.coeff = number / Math.Pow(10, cap.power);

        return cap;
    }

    private void Convert(double number)
    {
        CoeffAndPower cap1 = Decompose(number);
        CoeffAndPower cap2 = Decompose(cap1.power);

        _top1Coeff = cap1.coeff;
        _top2Coeff = cap2.coeff;
        _top3Coeff = cap2.power;
    }

    /// <summary>
    /// 파라미터 없이 객체를 생성하면 모든 레이어의 계수가 0으로 초기화됩니다.
    /// </summary>
    public PowerTowerNotation()
    {
        
    }

    /// <summary>
    /// 일반 숫자 타입을 파라미터로 받아 내부적으로 계수와 지수로 분해합니다.
    /// </summary>
    /// <param name="number"></param>
    public PowerTowerNotation(double number)
    {
        Convert(number);
    }

    /// <summary>
    /// 일반 숫자 타입을 파라미터로 받아 내부적으로 계수와 지수로 분해합니다.
    /// </summary>
    /// <param name="number"></param>
    public PowerTowerNotation(int number)
    {
        Convert(number);
    }

    /// <summary>
    /// 세 개의 실수를 입력으로 받아 이들을 각 레이어의 계수로 갖는 객체를 생성합니다. 세 입력의 절댓값은 10 이상이 될 수 없습니다.
    /// 특정 파라미터가 0이면 이후의 모든 파라미터 또한 0이 됩니다.
    /// </summary>
    /// <param name="layer1Coeff"></param>
    /// <param name="layer2Coeff"></param>
    /// <param name="layer3Coeff"></param>
    public PowerTowerNotation(double layer1Coeff, double layer2Coeff, double layer3Coeff)
    {
        if (Math.Abs(layer1Coeff) >= 10f || Math.Abs(layer2Coeff) >= 10f || Math.Abs(layer3Coeff) >= 10f)
        {
            throw new ArgumentOutOfRangeException("abs value of each coefficients cannot exceed 10");
        }

        if (layer1Coeff == 0f)
        {
            _top1Coeff = 0f;
            _top2Coeff = 0f;
            _top3Coeff = 0f;
            return;
        }
        if (layer2Coeff == 0f)
        {
            _top2Coeff = 0f;
            _top3Coeff = 0f;
        }

        //if (Math.Abs(_top1Coeff) < 1f)
        //{
        //    throw new ArgumentOutOfRangeException("use negative value on second layer instead of using small value on first layer");
        //}
        //if (Math.Abs(_top2Coeff) < 1f || Math.Abs(_top3Coeff) < 1f)
        //{
        //    throw new ArgumentOutOfRangeException("nonzero values less than 1 are not allowed");
        //}
        //if (_top3Coeff < 0f)
        //{
        //    throw new ArgumentOutOfRangeException("negative values are not allowed at top layer");
        //}

        _top1Coeff = layer1Coeff;
        _top2Coeff = layer2Coeff;
        _top3Coeff = layer3Coeff;
    }

    /// <summary>
    /// 화면에 출력하는 형식을 결정하여 문자열화합니다.
    /// </summary>
    /// <returns>문자열화된 숫자표현식</returns>
    public override string ToString()
    {
        if (_top3Coeff == 0f)
        {
            if (_top2Coeff >= 0f)
            {
                return (_top1Coeff * Math.Pow(10, _top2Coeff)).ToString("N0");
            }
            else
            {
                return (_top1Coeff * Math.Pow(10, _top2Coeff)).ToString("N" + (-_top2Coeff).ToString("N0"));
            }
        }

        string powerString = (_top2Coeff * Math.Pow(10, _top3Coeff)).ToString("N0");
        string coeffString = _top1Coeff.ToString("N2");

        return coeffString + "x10^" + powerString;
    }

    /// <summary>
    /// 완전히 똑같은 값을 갖는 새로운 객체를 복사하여 반환합니다.
    /// </summary>
    /// <returns>동일한 값을 갖는 새 객체</returns>
    public PowerTowerNotation Copy()
    {
        PowerTowerNotation copiedNumber = new PowerTowerNotation();
        copiedNumber._top1Coeff = _top1Coeff;
        copiedNumber._top2Coeff = _top2Coeff;
        copiedNumber._top3Coeff = _top3Coeff;

        return copiedNumber;
    }

    /// <summary>
    /// 양의 부호에 해당하는 단항 연산으로, 자기 자신을 반환합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <returns>자기 자신을 그대로 반환</returns>
    public static PowerTowerNotation operator +(PowerTowerNotation a) => a;

    /// <summary>
    /// 음의 부호에 해당하는 단항 연산으로, 최하층 계수의 부호를 뒤집은 새 객체를 반환합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <returns>부호가 바뀐 새 객처</returns>
    public static PowerTowerNotation operator -(PowerTowerNotation a)
    {
        PowerTowerNotation result = a.Copy();
        result._top1Coeff *= -1;
        return result;
    }

    private PowerTowerNotation Add(PowerTowerNotation other)
    {
        PowerTowerNotation result = new PowerTowerNotation();

        double coeff = _top1Coeff;
        double power = Math.Round(_top2Coeff * Math.Pow(10, _top3Coeff));
        double otherCoeff = other._top1Coeff;
        double otherPower = Math.Round(other._top2Coeff * Math.Pow(10, other._top3Coeff));

        double powerDiff = Math.Abs(power - otherPower);
        double resultPower;

        if (power >= otherPower)
        {
            result._top1Coeff = coeff + otherCoeff / Math.Pow(10, powerDiff);
            resultPower = power;
        }
        else
        {
            result._top1Coeff = otherCoeff + coeff / Math.Pow(10, powerDiff);
            resultPower = otherPower;
        }

        if (result._top1Coeff == 0f)
        {
            return result;
        }

        if (Math.Abs(result._top1Coeff) >= _coeffMax)
        {
            result._top1Coeff /= 10f;
            resultPower += 1f;
        }

        while (Math.Abs(result._top1Coeff) < 1f)
        {
            result._top1Coeff *= 10f;
            resultPower -= 1f;
        }

        if (Math.Abs(resultPower) >= _coeffMax)
        {
            result._top3Coeff = Math.Floor(Math.Log10(Math.Abs(resultPower)));
            result._top2Coeff = resultPower / Math.Pow(10, result._top3Coeff);
        }
        else
        {
            result._top2Coeff = resultPower;
        }

        return result;
    }

    /// <summary>
    /// 두 수의 합에 해당하는 새 객체를 반환합니다. PowerTowerNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>합</returns>
    public static PowerTowerNotation operator +(PowerTowerNotation a, PowerTowerNotation b) => a.Add(b);

    public static PowerTowerNotation operator +(PowerTowerNotation a, double b) => a.Add(new PowerTowerNotation(b));

    public static PowerTowerNotation operator +(PowerTowerNotation a, int b) => a.Add(new PowerTowerNotation(b));

    public static PowerTowerNotation operator +(double a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(b);

    public static PowerTowerNotation operator +(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(b);

    /// <summary>
    /// 첫째 파라미터에서 둘째 파라미터를 뺀 값에 해당하는 새 객체를 반환합니다. PowerTowerNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>차</returns>
    public static PowerTowerNotation operator -(PowerTowerNotation a, PowerTowerNotation b) => a.Add(-b);

    public static PowerTowerNotation operator -(PowerTowerNotation a, double b) => a.Add(-(new PowerTowerNotation(b)));

    public static PowerTowerNotation operator -(PowerTowerNotation a, int b) => a.Add(-(new PowerTowerNotation(b)));

    public static PowerTowerNotation operator -(double a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(-b);

    public static PowerTowerNotation operator -(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(-b);

    /// <summary>
    /// 현재 객체를 역수로 변환합니다.
    /// </summary>
    public void Reciprocate()
    {
        _top2Coeff *= -1f;

        if (_top1Coeff == 1f)
        {
            return;
        }

        try
        {
            _top1Coeff = 10f / _top1Coeff;      // NOT ACCURATE !!!!!
        }
        catch (DivideByZeroException e)
        {
            Debug.Log(e.Message);
            return;
        }
        _top2Coeff -= 1f;
    }

    private PowerTowerNotation Multiply(PowerTowerNotation other)
    {
        PowerTowerNotation result = new PowerTowerNotation();

        double coeff = _top1Coeff;
        double power = Math.Round(_top2Coeff * Math.Pow(10, _top3Coeff));
        double otherCoeff = other._top1Coeff;
        double otherPower = Math.Round(other._top2Coeff * Math.Pow(10, other._top3Coeff));

        double resultPower = power + otherPower;

        result._top1Coeff = coeff * otherCoeff;

        if (result._top1Coeff == 0f)
        {
            return result;
        }

        if (Math.Abs(result._top1Coeff) >= _coeffMax)
        {
            result._top1Coeff /= 10f;
            resultPower += 1f;
        }

        while (Math.Abs(result._top1Coeff) < 1f)
        {
            result._top1Coeff *= 10f;
            resultPower -= 1f;
        }

        if (Math.Abs(resultPower) >= _coeffMax)
        {
            result._top3Coeff = Math.Floor(Math.Log10(Math.Abs(resultPower)));
            result._top2Coeff = resultPower / Math.Pow(10, result._top3Coeff);
        }
        else
        {
            result._top2Coeff = resultPower;
        }

        return result;
    }

    /// <summary>
    /// 두 수의 곱에 해당하는 새 객체를 반환합니다. PowerTowerNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>곱</returns>
    public static PowerTowerNotation operator *(PowerTowerNotation a, PowerTowerNotation b) => a.Multiply(b);

    public static PowerTowerNotation operator *(PowerTowerNotation a, double b) => a.Multiply(new PowerTowerNotation(b));

    public static PowerTowerNotation operator *(PowerTowerNotation a, int b) => a.Multiply(new PowerTowerNotation(b));

    public static PowerTowerNotation operator *(double a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Multiply(b);

    public static PowerTowerNotation operator *(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Multiply(b);

    /// <summary>
    /// 첫째 파라미터를 둘째 파라미터로 나눈 값에 해당하는 새 객체를 반환합니다. PowerTowerNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>몫</returns>
    public static PowerTowerNotation operator /(PowerTowerNotation a, PowerTowerNotation b)
    {
        b.Reciprocate();
        return a.Multiply(b);
    }

    public static PowerTowerNotation operator /(PowerTowerNotation a, double b)
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

    public static PowerTowerNotation operator /(double a, PowerTowerNotation b)
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

    /// <summary>
    /// PowerTowerNotation 타입의 상용로그 값을 실수로 반환합니다. 파라미터가 양수가 아닌 경우 예외처리됩니다.
    /// </summary>
    /// <returns>상용로그값</returns>
    /// <exception cref="ArithmeticException"></exception>
    public static double Log10(PowerTowerNotation a)
    {
        if (a._top1Coeff < 0f)
        {
            throw new ArithmeticException("Only positive values allowed");
        }
        return a._top2Coeff * Math.Pow(10.0, a._top3Coeff) + Math.Log10(a._top1Coeff);
    }

    /// <summary>
    /// 첫째 파라미터를 밑수, 둘째 파라미터를 지수로 두는 지수 연산식의 계산값에 해당하는 새 객체를 반환합니다. PowerTowerNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>지수 연산값</returns>
    public static PowerTowerNotation operator ^(PowerTowerNotation a, double b)
    {
        PowerTowerNotation result = new PowerTowerNotation();

        if (a._top1Coeff == 0f)
        {
            if (b != 0f)
            {
                return result;
            }
            else
            {
                throw new DivideByZeroException("Base and Exponent cannot be 0 at once");
            }
        }

        if (b == 0f)
        {
            result._top1Coeff = 1f;
            return result;
        }

        double inputCoeff = a._top1Coeff;
        double inputPower = Math.Round(a._top2Coeff * Math.Pow(10, a._top3Coeff));

        double resultPower = (Math.Log10(inputCoeff) + inputPower) * b;
        double tempFrac = resultPower % 1;
        double resultCoeff = Math.Pow(10, tempFrac);

        result._top1Coeff = resultCoeff;

        resultPower -= tempFrac;
        result._top3Coeff = Math.Floor(Math.Log10(Math.Abs(resultPower)));
        result._top2Coeff = resultPower / Math.Pow(10, result._top3Coeff);

        return result;
    }

    public static PowerTowerNotation operator ^(PowerTowerNotation a, int b) => a ^ ((double)b);

    public static PowerTowerNotation operator ^(PowerTowerNotation a, PowerTowerNotation b)
    {
        if (a._top1Coeff == 0f && b._top1Coeff == 0f)
        {
            throw new DivideByZeroException("Base and Exponent cannot be 0 at once");
        }

        PowerTowerNotation result = new PowerTowerNotation();

        if (a._top1Coeff == 0f)
        {
            return result;
        }
        if (b._top1Coeff == 0f)
        {
            result._top1Coeff = 1f;
            return result;
        }

        double aCoeff = a._top1Coeff;
        double aPower = Math.Round(a._top2Coeff * Math.Pow(10, a._top3Coeff));
        double bCoeff = b._top1Coeff;
        double bPower = Math.Round(b._top2Coeff * Math.Pow(10, b._top3Coeff));

        if (aCoeff == 1f || bPower >= 10f)
        {
            result._top1Coeff = 1f;

            CoeffAndPower cap = Decompose(aPower * bCoeff);
            result._top2Coeff = cap.coeff;
            bPower += cap.power;
            result._top3Coeff = bPower;
        }
        else
        {
            double powerOfCoeff = Math.Log10(aCoeff) * bCoeff * Math.Pow(10, bPower);
            double tempFrac = powerOfCoeff % 1;
            double additionalPower = Math.Floor(powerOfCoeff);

            result._top1Coeff = Math.Pow(10, tempFrac);

            additionalPower /= Math.Pow(10, bPower);
            CoeffAndPower cap = Decompose(additionalPower + aPower * bCoeff);
            result._top2Coeff = cap.coeff;
            bPower += cap.power;
            result._top3Coeff = bPower;
        }

        return result;
    }

    public static PowerTowerNotation operator ^(double a, PowerTowerNotation b) => (new PowerTowerNotation(a)) ^ b;

    public static PowerTowerNotation operator ^(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) ^ b;

    /// <summary>
    /// 두 파라미터의 모든 속성이 같은지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>두 파라미터가 같은지에 대한 진리값</returns>
    public static bool operator ==(PowerTowerNotation a, PowerTowerNotation b)
    {
        if (a._top1Coeff != b._top1Coeff)
        {
            return false;
        }
        if (a._top2Coeff != b._top2Coeff)
        {
            return false;
        }
        if (a._top3Coeff != b._top3Coeff)
        {
            return false;
        }
        return true;
    }

    public static bool operator ==(PowerTowerNotation a, double b) => a == (new PowerTowerNotation(b));

    public static bool operator ==(PowerTowerNotation a, int b) => a == (new PowerTowerNotation(b));

    public static bool operator ==(double a, PowerTowerNotation b) => (new PowerTowerNotation(a)) == b;

    public static bool operator ==(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) == b;

    /// <summary>
    /// 두 파라미터의 속성 중 다른 부분이 있는지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>두 파라미터가 다른지에 대한 진리값</returns>
    public static bool operator !=(PowerTowerNotation a, PowerTowerNotation b) => !(a == b);

    public static bool operator !=(PowerTowerNotation a, double b) => a != (new PowerTowerNotation(b));

    public static bool operator !=(PowerTowerNotation a, int b) => a != (new PowerTowerNotation(b));

    public static bool operator !=(double a, PowerTowerNotation b) => (new PowerTowerNotation(a)) != b;

    public static bool operator !=(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) != b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 큰지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 큰지에 대한 진리값</returns>
    public static bool operator >(PowerTowerNotation a, PowerTowerNotation b)
    {
        if (a._top3Coeff > b._top3Coeff)
        {
            return true;
        }
        else if (a._top3Coeff < b._top3Coeff)
        {
            return false;
        }
        if (a._top2Coeff > b._top2Coeff)
        {
            return true;
        }
        else if (a._top2Coeff < b._top2Coeff)
        {
            return false;
        }
        if (a._top1Coeff > b._top1Coeff)
        {
            return true;
        }
        return false;
    }

    public static bool operator >(PowerTowerNotation a, double b) => a > (new PowerTowerNotation(b));

    public static bool operator >(PowerTowerNotation a, int b) => a > (new PowerTowerNotation(b));

    public static bool operator >(double a, PowerTowerNotation b) => (new PowerTowerNotation(a)) > b;

    public static bool operator >(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) > b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 크거나 같은지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 크거나 같은지에 대한 진리값</returns>
    public static bool operator >=(PowerTowerNotation a, PowerTowerNotation b) => (a > b) || (a == b);

    public static bool operator >=(PowerTowerNotation a, double b) => a >= (new PowerTowerNotation(b));

    public static bool operator >=(PowerTowerNotation a, int b) => a >= (new PowerTowerNotation(b));

    public static bool operator >=(double a, PowerTowerNotation b) => (new PowerTowerNotation(a)) >= b;

    public static bool operator >=(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) >= b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 작거나 같은지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 작거나 같은지에 대한 진리값</returns>
    public static bool operator <=(PowerTowerNotation a, PowerTowerNotation b) => !(a > b);

    public static bool operator <=(PowerTowerNotation a, double b) => a <= (new PowerTowerNotation(b));

    public static bool operator <=(PowerTowerNotation a, int b) => a <= (new PowerTowerNotation(b));

    public static bool operator <=(double a, PowerTowerNotation b) => (new PowerTowerNotation(a)) <= b;

    public static bool operator <=(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) <= b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 작은지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 작은지에 대한 진리값</returns>
    public static bool operator <(PowerTowerNotation a, PowerTowerNotation b) => !(a >= b);

    public static bool operator <(PowerTowerNotation a, double b) => a < (new PowerTowerNotation(b));

    public static bool operator <(PowerTowerNotation a, int b) => a < (new PowerTowerNotation(b));

    public static bool operator <(double a, PowerTowerNotation b) => (new PowerTowerNotation(a)) < b;

    public static bool operator <(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) < b;

    /// <summary>
    /// 최상위 계층의 수가 너무 커질 때 레이어를 추가하는 함수입니다. 이 객체를 속성으로 갖는 클래스에서 호출합니다.
    /// </summary>
    public void AscendLayer()
    {
        CoeffAndPower cap = Decompose(_top3Coeff);

        _top1Coeff = _top2Coeff;
        _top2Coeff = cap.coeff;
        _top3Coeff = cap.power;
    }

    /// <summary>
    /// 최상위 계층의 수가 0이고 하위 레이어가 존재할 때 최상위 레이어 한 개를 삭제하는 함수입니다. 이 객체를 속성으로 갖는 클래스에서 호출합니다.
    /// </summary>
    public void DescendLayer()
    {
        _top3Coeff = Math.Round(_top2Coeff * Math.Pow(10, _top3Coeff));
        _top2Coeff = _top1Coeff;
        _top1Coeff = 1f;
    }
}
