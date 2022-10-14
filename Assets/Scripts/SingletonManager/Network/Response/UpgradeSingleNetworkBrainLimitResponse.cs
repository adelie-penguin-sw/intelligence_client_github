using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeSingleNetworkBrainLimitResponse
{
    public AnsEquation NP;
    public List<BrainAttributes> brainAttributes;
    public long calcTime;
    public int statusCode;
}
