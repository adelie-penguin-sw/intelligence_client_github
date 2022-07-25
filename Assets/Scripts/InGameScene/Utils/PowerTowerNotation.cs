using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerTowerNotation
{
    private static float _coeffMax = 10f;

    public float[] _coeffArr = {0f, 0f, 0f};

    struct CoeffAndPower
    {
        public float coeff;
        public float power;
    }

    private static CoeffAndPower Decompose(float number)
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
            return;
        }
        else if (Mathf.Abs(tmpPower) < _coeffMax)
        {
            _coeffArr[1] = tmpPower;
            return;
        }
        else
        {
            cap = Decompose(tmpPower);
            _coeffArr[1] = cap.coeff;
            _coeffArr[2] = cap.power;
            return;
        }
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
    public PowerTowerNotation(float number)
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
    public PowerTowerNotation(float layer1Coeff, float layer2Coeff, float layer3Coeff)
    {
        if (Mathf.Abs(layer1Coeff) >= 10f || Mathf.Abs(layer2Coeff) >= 10f || Mathf.Abs(layer3Coeff) >= 10f)
        {
            throw new ArgumentOutOfRangeException("All three parameters must have values between -10 and 10.");
        }

        if (layer1Coeff == 0f)
        {
            layer2Coeff = 0f;
            layer3Coeff = 0f;
        }
        if (layer2Coeff == 0f)
        {
            layer3Coeff = 0f;
        }

        _coeffArr[0] = layer1Coeff;
        _coeffArr[1] = layer2Coeff;
        _coeffArr[2] = layer3Coeff;
    }

    /// <summary>
    /// 화면에 출력하는 형식을 결정하여 문자열화합니다.
    /// </summary>
    /// <returns>문자열화된 숫자표현식</returns>
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

    /// <summary>
    /// 완전히 똑같은 값을 갖는 새로운 객체를 복사하여 반환합니다.
    /// </summary>
    /// <returns>동일한 값을 갖는 새 객체</returns>
    public PowerTowerNotation Copy()
    {
        PowerTowerNotation copiedNumber = new PowerTowerNotation();
        copiedNumber._coeffArr[0] = _coeffArr[0];
        copiedNumber._coeffArr[1] = _coeffArr[1];
        copiedNumber._coeffArr[2] = _coeffArr[2];

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

        return result;
    }

    /// <summary>
    /// 두 수의 합에 해당하는 새 객체를 반환합니다. PowerTowerNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>합</returns>
    public static PowerTowerNotation operator +(PowerTowerNotation a, PowerTowerNotation b) => a.Add(b);

    public static PowerTowerNotation operator +(PowerTowerNotation a, float b) => a.Add(new PowerTowerNotation(b));

    public static PowerTowerNotation operator +(PowerTowerNotation a, int b) => a.Add(new PowerTowerNotation(b));

    public static PowerTowerNotation operator +(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(b);

    public static PowerTowerNotation operator +(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(b);

    /// <summary>
    /// 첫째 파라미터에서 둘째 파라미터를 뺀 값에 해당하는 새 객체를 반환합니다. PowerTowerNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>차</returns>
    public static PowerTowerNotation operator -(PowerTowerNotation a, PowerTowerNotation b) => a.Add(-b);

    public static PowerTowerNotation operator -(PowerTowerNotation a, float b) => a.Add(-(new PowerTowerNotation(b)));

    public static PowerTowerNotation operator -(PowerTowerNotation a, int b) => a.Add(-(new PowerTowerNotation(b)));

    public static PowerTowerNotation operator -(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(-b);

    public static PowerTowerNotation operator -(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Add(-b);

    /// <summary>
    /// 현재 객체를 역수로 변환합니다.
    /// </summary>
    public void Reciprocate()
    {
        _coeffArr[1] *= -1f;

        if (_coeffArr[0] == 1f)
        {
            return;
        }

        try
        {
            _coeffArr[0] = 10f / _coeffArr[0];      // NOT ACCURATE !!!!!
        }
        catch (DivideByZeroException e)
        {
            Debug.Log(e.Message);
            return;
        }
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

        return result;
    }

    /// <summary>
    /// 두 수의 곱에 해당하는 새 객체를 반환합니다. PowerTowerNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>곱</returns>
    public static PowerTowerNotation operator *(PowerTowerNotation a, PowerTowerNotation b) => a.Multiply(b);

    public static PowerTowerNotation operator *(PowerTowerNotation a, float b) => a.Multiply(new PowerTowerNotation(b));

    public static PowerTowerNotation operator *(PowerTowerNotation a, int b) => a.Multiply(new PowerTowerNotation(b));

    public static PowerTowerNotation operator *(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)).Multiply(b);

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

    /// <summary>
    /// 첫째 파라미터를 밑수, 둘째 파라미터를 지수로 두는 지수 연산식의 계산값에 해당하는 새 객체를 반환합니다. PowerTowerNotation 타입을 리턴합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>지수 연산값</returns>
    public static PowerTowerNotation operator ^(PowerTowerNotation a, float b)
    {
        PowerTowerNotation result = new PowerTowerNotation();

        if (a._coeffArr[0] == 0f)
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
            result._coeffArr[0] = 1f;
            return result;
        }

        float inputCoeff = a._coeffArr[0];
        float inputPower = Mathf.Round(a._coeffArr[1] * Mathf.Pow(10, a._coeffArr[2]));

        float resultPower = (Mathf.Log10(inputCoeff) + inputPower) * b;
        float tempFrac = resultPower % 1;
        float resultCoeff = Mathf.Pow(10, tempFrac);

        result._coeffArr[0] = resultCoeff;

        resultPower -= tempFrac;
        result._coeffArr[2] = Mathf.Floor(Mathf.Log10(Mathf.Abs(resultPower)));
        result._coeffArr[1] = resultPower / Mathf.Pow(10, result._coeffArr[2]);

        return result;
    }

    public static PowerTowerNotation operator ^(PowerTowerNotation a, int b) => a ^ ((float)b);

    public static PowerTowerNotation operator ^(PowerTowerNotation a, PowerTowerNotation b)
    {
        if (a._coeffArr[0] == 0f && b._coeffArr[0] == 0f)
        {
            throw new DivideByZeroException("Base and Exponent cannot be 0 at once");
        }

        PowerTowerNotation result = new PowerTowerNotation();

        if (a._coeffArr[0] == 0f)
        {
            return result;
        }
        if (b._coeffArr[0] == 0f)
        {
            result._coeffArr[0] = 1f;
            return result;
        }

        float aCoeff = a._coeffArr[0];
        float aPower = Mathf.Round(a._coeffArr[1] * Mathf.Pow(10, a._coeffArr[2]));
        float bCoeff = b._coeffArr[0];
        float bPower = Mathf.Round(b._coeffArr[1] * Mathf.Pow(10, b._coeffArr[2]));

        if (aCoeff == 1f || bPower >= 10f)
        {
            result._coeffArr[0] = 1f;

            CoeffAndPower cap = Decompose(aPower * bCoeff);
            result._coeffArr[1] = cap.coeff;
            bPower += cap.power;
            result._coeffArr[2] = bPower;
        }
        else
        {
            float powerOfCoeff = Mathf.Log10(aCoeff) * bCoeff * Mathf.Pow(10, bPower);
            float tempFrac = powerOfCoeff % 1;
            float additionalPower = Mathf.Floor(powerOfCoeff);

            result._coeffArr[0] = Mathf.Pow(10, tempFrac);

            additionalPower /= Mathf.Pow(10, bPower);
            CoeffAndPower cap = Decompose(additionalPower + aPower * bCoeff);
            result._coeffArr[1] = cap.coeff;
            bPower += cap.power;
            result._coeffArr[2] = bPower;
        }

        return result;
    }

    public static PowerTowerNotation operator ^(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)) ^ b;

    public static PowerTowerNotation operator ^(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) ^ b;

    /// <summary>
    /// 두 파라미터의 모든 속성이 같은지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>두 파라미터가 같은지에 대한 진리값</returns>
    public static bool operator ==(PowerTowerNotation a, PowerTowerNotation b)
    {
        for (int i=0; i<3; i++)
        {
            if (a._coeffArr[i] != b._coeffArr[0])
            {
                return false;
            }
        }
        return true;
    }

    public static bool operator ==(PowerTowerNotation a, float b) => a == (new PowerTowerNotation(b));

    public static bool operator ==(PowerTowerNotation a, int b) => a == (new PowerTowerNotation(b));

    public static bool operator ==(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)) == b;

    public static bool operator ==(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) == b;

    /// <summary>
    /// 두 파라미터의 속성 중 다른 부분이 있는지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>두 파라미터가 다른지에 대한 진리값</returns>
    public static bool operator !=(PowerTowerNotation a, PowerTowerNotation b) => !(a == b);

    public static bool operator !=(PowerTowerNotation a, float b) => a != (new PowerTowerNotation(b));

    public static bool operator !=(PowerTowerNotation a, int b) => a != (new PowerTowerNotation(b));

    public static bool operator !=(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)) != b;

    public static bool operator !=(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) != b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 큰지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 큰지에 대한 진리값</returns>
    public static bool operator >(PowerTowerNotation a, PowerTowerNotation b)
    {
        for (int i = 2; i >= 0; i--)
        {
            if (a._coeffArr[i] > b._coeffArr[i])
            {
                return true;
            }
        }

        return false;
    }

    public static bool operator >(PowerTowerNotation a, float b) => a > (new PowerTowerNotation(b));

    public static bool operator >(PowerTowerNotation a, int b) => a > (new PowerTowerNotation(b));

    public static bool operator >(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)) > b;

    public static bool operator >(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) > b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 크거나 같은지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 크거나 같은지에 대한 진리값</returns>
    public static bool operator >=(PowerTowerNotation a, PowerTowerNotation b) => (a > b) || (a == b);

    public static bool operator >=(PowerTowerNotation a, float b) => a >= (new PowerTowerNotation(b));

    public static bool operator >=(PowerTowerNotation a, int b) => a >= (new PowerTowerNotation(b));

    public static bool operator >=(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)) >= b;

    public static bool operator >=(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) >= b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 작거나 같은지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 작거나 같은지에 대한 진리값</returns>
    public static bool operator <=(PowerTowerNotation a, PowerTowerNotation b) => !(a > b);

    public static bool operator <=(PowerTowerNotation a, float b) => a <= (new PowerTowerNotation(b));

    public static bool operator <=(PowerTowerNotation a, int b) => a <= (new PowerTowerNotation(b));

    public static bool operator <=(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)) <= b;

    public static bool operator <=(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) <= b;

    /// <summary>
    /// 첫째 파라미터의 값이 둘째 파라미터의 값보다 작은지에 대한 진리값을 반환합니다. 타입이 다른 경우 PowerTowerNotation으로 변환한 후 비교합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>첫째 파라미터의 값이 둘째 파라미터의 값보다 작은지에 대한 진리값</returns>
    public static bool operator <(PowerTowerNotation a, PowerTowerNotation b) => !(a >= b);

    public static bool operator <(PowerTowerNotation a, float b) => a < (new PowerTowerNotation(b));

    public static bool operator <(PowerTowerNotation a, int b) => a < (new PowerTowerNotation(b));

    public static bool operator <(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)) < b;

    public static bool operator <(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) < b;

    /// <summary>
    /// 최상위 계층의 수가 너무 커질 때 레이어를 추가하는 함수입니다. 이 객체를 속성으로 갖는 클래스에서 호출합니다.
    /// </summary>
    public void AscendLayer()
    {
        CoeffAndPower cap = Decompose(_coeffArr[2]);

        _coeffArr[0] = _coeffArr[1];
        _coeffArr[1] = cap.coeff;
        _coeffArr[2] = cap.power;
    }

    /// <summary>
    /// 최상위 계층의 수가 0이고 하위 레이어가 존재할 때 최상위 레이어 한 개를 삭제하는 함수입니다. 이 객체를 속성으로 갖는 클래스에서 호출합니다.
    /// </summary>
    public void DescendLayer()
    {
        _coeffArr[2] = Mathf.Round(_coeffArr[1] * Mathf.Pow(10, _coeffArr[2]));
        _coeffArr[1] = _coeffArr[0];
        _coeffArr[0] = 1f;
    }
}
