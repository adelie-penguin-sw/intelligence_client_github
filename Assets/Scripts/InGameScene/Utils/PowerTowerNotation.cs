using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTowerNotation
{
    private static float _coeffMax = 10f;

    private float[] _coeffArr = {0f, 0f, 0f};
    private int _layer = 1;

    /// <summary>
    /// 레이어 값은 레이어링 표현식에서 사용되며, 본 클래스 내부적으로 직접 사용되지는 않고 수의 크기에 따라 값만 결정됩니다.
    /// </summary>
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
        copiedNumber._layer = _layer;

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
                // Exception
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
            // Exception
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

        return result;
    }

    public static PowerTowerNotation operator ^(float a, PowerTowerNotation b) => (new PowerTowerNotation(a)) ^ b;

    public static PowerTowerNotation operator ^(int a, PowerTowerNotation b) => (new PowerTowerNotation(a)) ^ b;
}
