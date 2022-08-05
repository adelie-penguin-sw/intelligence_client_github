using System;

public class Exchange
{
    /// <summary>
    /// Tetration함수 10^^x 의 역함수입니다. 즉, 지수 타워가 몇 층인지를 실수값으로 반환하는 연속함수입니다.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static double Slog10(UpArrowNotation number)
    {
        if (number.OperatorLayerCount > 9)
        {
            throw new Exception("Expressions with tetration or higher are not allowed");
        }

        double topLayerValue;
        PowerTowerNotation top3Layer = number.Top3Layer;

        switch (number.OperatorLayerCount)
        {
            case 0:
                topLayerValue = top3Layer._top1Coeff;
                break;
            case 1:
                topLayerValue = top3Layer._top2Coeff + Math.Log10(top3Layer._top1Coeff);
                break;
            default:
                topLayerValue = top3Layer._top3Coeff + Math.Log10(top3Layer._top2Coeff);
                break;
        }

        double ln10 = Math.Log(10);
        double fracPart = Math.Log(Math.Log10(topLayerValue) * (ln10 - 1) + 1, ln10);

        return number.OperatorLayerCount + fracPart;
    }

    /// <summary>
    /// 브레인 생성 시 소비될 NP량을 계산하는 함수입니다. 베이스 비용과 직전까지의 브레인 생성 횟수, 코어로부터의 물리적 거리, 그리고 비용 증가율에 따라 결정됩니다.
    /// </summary>
    /// <param name="baseCost"></param>
    /// <param name="pastBrainGenCount"></param>
    /// <param name="physicalDistance"></param>
    /// <param name="costGrowthRate"></param>
    /// <returns></returns>
    public static PowerTowerNotation GetNPCostForBrainGeneration(PowerTowerNotation baseCost, int pastBrainGenCount, double physicalDistance, double costGrowthRate)
    {
        // {기본 브레인 생성 비용} * {코어 브레인으로부터의 물리적 거리} * {브레인 생성 비용 증가율} ^ {직전까지의 브레인 생성 횟수(ASN제외)}NP

        return baseCost * physicalDistance * Math.Pow(costGrowthRate, pastBrainGenCount);
    }

    /// <summary>
    /// 두 브레인을 연결할 때, 즉 채널을 생성할 때 소비될 NP량을 계산하는 함수입니다. 베이스 비용과 리시버 브레인의 distance값, 그리고 센더 브레인의 현재 지능수준에 따라 결정됩니다.
    /// </summary>
    /// <param name="baseCost"></param>
    /// <param name="receiverDistance"></param>
    /// <param name="senderIntellect"></param>
    /// <returns></returns>
    public static PowerTowerNotation GetNPCostForBrainConnection(PowerTowerNotation baseCost, int receiverDistance, UpArrowNotation senderIntellect)
    {
        // {기본 채널 생성 비용} * A ^ {B * (C * {리시버 브레인의 distance}) ^ {D * slog{센더 브레인의 지능}}}NP

        double a = 2f;
        double b = 2f;
        double c = 2f;
        double d = 2f;

        return baseCost * Math.Pow(a, b * Math.Pow(c * receiverDistance, d * Slog10(senderIntellect)));
    }

    /// <summary>
    /// 브레인을 업그레이드할 때 소비될 NP량을 계산하는 함수입니다. 베이스 비용과 직전까지의 업그레이드 횟수, 그리고 비용 증가율에 따라 결정됩니다.
    /// </summary>
    /// <param name="baseCost"></param>
    /// <param name="upgradeCount"></param>
    /// <param name="growthRate"></param>
    /// <returns></returns>
    public static PowerTowerNotation GetNPCostForBrainUpgrade(PowerTowerNotation baseCost, int upgradeCount, double growthRate)
    {
        // {기본 업그레이드 비용} * {비용 증가율} ^ {업그레이드 횟수}

        return baseCost * Math.Pow(growthRate, upgradeCount);
    }

    /// <summary>
    /// 브레인 분해(판매) 시 획득할 NP량을 계산하는 함수입니다. 현재 브레인 지능수준에 따라 결정됩니다.
    /// </summary>
    /// <param name="intellect"></param>
    /// <returns></returns>
    public static PowerTowerNotation GetNPRewardForBrainDecomposition(UpArrowNotation intellect)
    {
        if (intellect.CalculateFull() < 10.0) {
            return new PowerTowerNotation(0);
        }

        // NP = a * log(지능) ^ {b * log(c * log(지능))}

        double a = 1.0;
        double b = 3.0;
        double c = 2.0;

        double logNumber = PowerTowerNotation.Log10(intellect.Top3Layer);
        return new PowerTowerNotation(Math.Floor(a * Math.Pow(logNumber, b * Math.Log10(c * logNumber))));
    }

    /// <summary>
    /// 네트워크를 초기화할 때 획득할 TP량을 계산하는 함수입니다. 현재 코어 브레인의 지능수준에 따라 결정됩니다.
    /// </summary>
    /// <param name="coreIntellect"></param>
    /// <returns></returns>
    public static PowerTowerNotation GetTPRewardForReset(UpArrowNotation coreIntellect)
    {
        // TP = (a * log(코어 브레인의 지능)) ^ {b}

        double a = 0.01;
        double b = 0.8;

        double logNumber = PowerTowerNotation.Log10(coreIntellect.Top3Layer);
        return new PowerTowerNotation(Math.Floor(Math.Pow(a * logNumber, b)));
    }
}
