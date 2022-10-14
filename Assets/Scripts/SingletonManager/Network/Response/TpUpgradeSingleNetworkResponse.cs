using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TpUpgradeSingleNetworkResponse
{
	public int statusCode;
	public AnsEquation TP;
	public List<BrainAttributes> brainAttributes;
	public List<UpgradeCondition> upgradeCondition;
	public long calcTime;
	public long brainUpgradePower;
	public long maxDepth;
	public long newBrain;		// 이거 뭐예요 ??
}