using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreateSingleNetworkBrainResponse
{
	public AnsEquation NP;
	public List<BrainAttributes> brainAttributes;
	public long calcTime;
	public long newBrain;
	public int statusCode;
	public long allBrainCount;
}


