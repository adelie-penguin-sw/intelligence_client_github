using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DefinitionKey
{
    public const string experimentGoalList = "ExperimentGoalList";

    #region BRAIN
    public const string brainGeneratingCostEquation = "BrainGeneratingCostEquation";
    public const string brainDecomposingGainEquation = "BrainDecomposingGainEquation";
    public const string brainMultiplierUpgradeCostEquation = "BrainMultiplierUpgradeCostEquation";
    public const string brainMultiplierEquation = "BrainMultiplierEquation";
    public const string brainLimitUpgradeCostEquation = "BrainLimitUpgradeCostEquation";
    public const string brainLimitEquation = "BrainLimitEquation";
    #endregion

    #region CHANNEL
    public const string channelGeneratingCostEquation = "ChannelGeneratingCostEquation";
    #endregion

    #region MULTIPLIER
    public const string multiplierRewardForReset = "MultiplierRewardForReset";
    public const string multiplierBoostForTPU007 = "MultiplierBoostForTPU007";
    public const string multiplierBoostForTPU010 = "MultiplierBoostForTPU010";
    public const string multiplierBoostForTPU015 = "MultiplierBoostForTPU015";
    public const string multiplierBoostForTPU019 = "MultiplierBoostForTPU019";
    public const string multiplierBoostForTPU020 = "MultiplierBoostForTPU020";
    public const string multiplierBoostForTPU026 = "MultiplierBoostForTPU026";
    public const string multiplierBoostForTPU028 = "MultiplierBoostForTPU028";
    #endregion

    #region TP
    public const string tpRewardForReset = "TPrewardForReset";

    public const string tpu001MaxLevel = "TPU001MaxLevel";
    public const string tpu002MaxLevel = "TPU002MaxLevel";
    public const string tpu003MaxLevel = "TPU003MaxLevel";
    public const string tpu004MaxLevel = "TPU004MaxLevel";
    public const string tpu005MaxLevel = "TPU005MaxLevel";
    public const string tpu006MaxLevel = "TPU006MaxLevel";
    public const string tpu007MaxLevel = "TPU007MaxLevel";
    public const string tpu008MaxLevel = "TPU008MaxLevel";
    public const string tpu009MaxLevel = "TPU009MaxLevel";
    public const string tpu010MaxLevel = "TPU010MaxLevel";
    public const string tpu011MaxLevel = "TPU011MaxLevel";
    public const string tpu012MaxLevel = "TPU012MaxLevel";
    public const string tpu013MaxLevel = "TPU013MaxLevel";
    public const string tpu014MaxLevel = "TPU014MaxLevel";
    public const string tpu015MaxLevel = "TPU015MaxLevel";
    public const string tpu016MaxLevel = "TPU016MaxLevel";
    public const string tpu017MaxLevel = "TPU017MaxLevel";
    public const string tpu018MaxLevel = "TPU018MaxLevel";
    public const string tpu019MaxLevel = "TPU019MaxLevel";
    public const string tpu020MaxLevel = "TPU020MaxLevel";
    public const string tpu021MaxLevel = "TPU021MaxLevel";
    public const string tpu022MaxLevel = "TPU022MaxLevel";
    public const string tpu023MaxLevel = "TPU023MaxLevel";
    public const string tpu024MaxLevel = "TPU024MaxLevel";
    public const string tpu025MaxLevel = "TPU025MaxLevel";
    public const string tpu026MaxLevel = "TPU026MaxLevel";
    public const string tpu027MaxLevel = "TPU027MaxLevel";
    public const string tpu028MaxLevel = "TPU028MaxLevel";
    public const string tpu029MaxLevel = "TPU029MaxLevel";

    public const string tpu001UnlockRequirement = "TPU001UnlockRequirement";
    public const string tpu002UnlockRequirement = "TPU002UnlockRequirement";
    public const string tpu003UnlockRequirement = "TPU003UnlockRequirement";
    public const string tpu004UnlockRequirement = "TPU004UnlockRequirement";
    public const string tpu005UnlockRequirement = "TPU005UnlockRequirement";
    public const string tpu006UnlockRequirement = "TPU006UnlockRequirement";
    public const string tpu007UnlockRequirement = "TPU007UnlockRequirement";
    public const string tpu008UnlockRequirement = "TPU008UnlockRequirement";
    public const string tpu009UnlockRequirement = "TPU009UnlockRequirement";
    public const string tpu010UnlockRequirement = "TPU010UnlockRequirement";
    public const string tpu011UnlockRequirement = "TPU011UnlockRequirement";
    public const string tpu012UnlockRequirement = "TPU012UnlockRequirement";
    public const string tpu013UnlockRequirement = "TPU013UnlockRequirement";
    public const string tpu014UnlockRequirement = "TPU014UnlockRequirement";
    public const string tpu015UnlockRequirement = "TPU015UnlockRequirement";
    public const string tpu016UnlockRequirement = "TPU016UnlockRequirement";
    public const string tpu017UnlockRequirement = "TPU017UnlockRequirement";
    public const string tpu018UnlockRequirement = "TPU018UnlockRequirement";
    public const string tpu019UnlockRequirement = "TPU019UnlockRequirement";
    public const string tpu020UnlockRequirement = "TPU020UnlockRequirement";
    public const string tpu021UnlockRequirement = "TPU021UnlockRequirement";
    public const string tpu022UnlockRequirement = "TPU022UnlockRequirement";
    public const string tpu023UnlockRequirement = "TPU023UnlockRequirement";
    public const string tpu024UnlockRequirement = "TPU024UnlockRequirement";
    public const string tpu025UnlockRequirement = "TPU025UnlockRequirement";
    public const string tpu026UnlockRequirement = "TPU026UnlockRequirement";
    public const string tpu027UnlockRequirement = "TPU027UnlockRequirement";
    public const string tpu028UnlockRequirement = "TPU028UnlockRequirement";
    public const string tpu029UnlockRequirement = "TPU029UnlockRequirement";

    public const string tpu001NameText = "TPU001NameText";
    public const string tpu002NameText = "TPU002NameText";
    public const string tpu003NameText = "TPU003NameText";
    public const string tpu004NameText = "TPU004NameText";
    public const string tpu005NameText = "TPU005NameText";
    public const string tpu006NameText = "TPU006NameText";
    public const string tpu007NameText = "TPU007NameText";
    public const string tpu008NameText = "TPU008NameText";
    public const string tpu009NameText = "TPU009NameText";
    public const string tpu010NameText = "TPU010NameText";
    public const string tpu011NameText = "TPU011NameText";
    public const string tpu012NameText = "TPU012NameText";
    public const string tpu013NameText = "TPU013NameText";
    public const string tpu014NameText = "TPU014NameText";
    public const string tpu015NameText = "TPU015NameText";
    public const string tpu016NameText = "TPU016NameText";
    public const string tpu017NameText = "TPU017NameText";
    public const string tpu018NameText = "TPU018NameText";
    public const string tpu019NameText = "TPU019NameText";
    public const string tpu020NameText = "TPU020NameText";
    public const string tpu021NameText = "TPU021NameText";
    public const string tpu022NameText = "TPU022NameText";
    public const string tpu023NameText = "TPU023NameText";
    public const string tpu024NameText = "TPU024NameText";
    public const string tpu025NameText = "TPU025NameText";
    public const string tpu026NameText = "TPU026NameText";
    public const string tpu027NameText = "TPU027NameText";
    public const string tpu028NameText = "TPU028NameText";
    public const string tpu029NameText = "TPU029NameText";

    public const string tpu001EffectText = "TPU001EffectText";
    public const string tpu002EffectText = "TPU002EffectText";
    public const string tpu003EffectText = "TPU003EffectText";
    public const string tpu004EffectText = "TPU004EffectText";
    public const string tpu005EffectText = "TPU005EffectText";
    public const string tpu006EffectText = "TPU006EffectText";
    public const string tpu007EffectText = "TPU007EffectText";
    public const string tpu008EffectText = "TPU008EffectText";
    public const string tpu009EffectText = "TPU009EffectText";
    public const string tpu010EffectText = "TPU010EffectText";
    public const string tpu011EffectText = "TPU011EffectText";
    public const string tpu012EffectText = "TPU012EffectText";
    public const string tpu013EffectText = "TPU013EffectText";
    public const string tpu014EffectText = "TPU014EffectText";
    public const string tpu015EffectText = "TPU015EffectText";
    public const string tpu016EffectText = "TPU016EffectText";
    public const string tpu017EffectText = "TPU017EffectText";
    public const string tpu018EffectText = "TPU018EffectText";
    public const string tpu019EffectText = "TPU019EffectText";
    public const string tpu020EffectText = "TPU020EffectText";
    public const string tpu021EffectText = "TPU021EffectText";
    public const string tpu022EffectText = "TPU022EffectText";
    public const string tpu023EffectText = "TPU023EffectText";
    public const string tpu024EffectText = "TPU024EffectText";
    public const string tpu025EffectText = "TPU025EffectText";
    public const string tpu026EffectText = "TPU026EffectText";
    public const string tpu027EffectText = "TPU027EffectText";
    public const string tpu028EffectText = "TPU028EffectText";
    public const string tpu029EffectText = "TPU029EffectText";

    public const string tpu001CostEquation = "TPU001CostEquation";
    public const string tpu002CostEquation = "TPU002CostEquation";
    public const string tpu003CostEquation = "TPU003CostEquation";
    public const string tpu004CostEquation = "TPU004CostEquation";
    public const string tpu005CostEquation = "TPU005CostEquation";
    public const string tpu006CostEquation = "TPU006CostEquation";
    public const string tpu007CostEquation = "TPU007CostEquation";
    public const string tpu008CostEquation = "TPU008CostEquation";
    public const string tpu009CostEquation = "TPU009CostEquation";
    public const string tpu010CostEquation = "TPU010CostEquation";
    public const string tpu011CostEquation = "TPU011CostEquation";
    public const string tpu012CostEquation = "TPU012CostEquation";
    public const string tpu013CostEquation = "TPU013CostEquation";
    public const string tpu014CostEquation = "TPU014CostEquation";
    public const string tpu015CostEquation = "TPU015CostEquation";
    public const string tpu016CostEquation = "TPU016CostEquation";
    public const string tpu017CostEquation = "TPU017CostEquation";
    public const string tpu018CostEquation = "TPU018CostEquation";
    public const string tpu019CostEquation = "TPU019CostEquation";
    public const string tpu020CostEquation = "TPU020CostEquation";
    public const string tpu021CostEquation = "TPU021CostEquation";
    public const string tpu022CostEquation = "TPU022CostEquation";
    public const string tpu023CostEquation = "TPU023CostEquation";
    public const string tpu024CostEquation = "TPU024CostEquation";
    public const string tpu025CostEquation = "TPU025CostEquation";
    public const string tpu026CostEquation = "TPU026CostEquation";
    public const string tpu027CostEquation = "TPU027CostEquation";
    public const string tpu028CostEquation = "TPU028CostEquation";
    public const string tpu029CostEquation = "TPU029CostEquation";
    #endregion

    #region LIMITS
    public const string baseIntellectLimitList = "BaseIntellectLimitList";
    #endregion

}
