using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 방정식 계수들을 기반으로 현재 지능을 계산해주는 클래스입니다.
/// </summary>
public class Equation
{
    /// <summary>
    /// 방정식의 차수만큼 t^n / n! 를 계산하여 리스트로 반환합니다. 
    /// </summary>
    /// <param name="order"></param>
    /// <param name="elapsedTime"></param>
    /// <returns></returns>
    public static List<UpArrowNotation> GetTimeCoeffs(int order, double elapsedTime)
    {
        List<UpArrowNotation> resultList = new List<UpArrowNotation>();
        resultList.Add(new UpArrowNotation(1));

        for (int i = 0; i < order; i++)
        {
            resultList.Add((resultList[resultList.Count - 1] * elapsedTime) / resultList.Count);
        }

        return resultList;
    }

    /// <summary>
    /// 브레인이 가진 방정식 계수와 경과 시간을 이용하여 현재 지능 수치를 계산합니다.
    /// </summary>
    /// <param name="brainEqCoeffs"></param>
    /// <returns></returns>
    public static UpArrowNotation MultiplyCoeffs(List<UpArrowNotation> brainEqCoeffs, List<UpArrowNotation> timeCoeffs)
    {
        UpArrowNotation result = new UpArrowNotation();

        for (int i = 0; i < brainEqCoeffs.Count; i++)
        {
            result += brainEqCoeffs[i] * timeCoeffs[i];
        }

        return result;
    }

    public static UpArrowNotation GetCurrentIntellect(List<UpArrowNotation> brainEqCoeffs, double elapsedTime)
    {
        return MultiplyCoeffs(brainEqCoeffs, GetTimeCoeffs(brainEqCoeffs.Count - 1, elapsedTime));
    }
}
