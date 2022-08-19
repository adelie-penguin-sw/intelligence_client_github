using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeleteSingleNetworkBrainResponse
{
	public int statusCode;
    public List<AnsEquations> ansEquations;
    public List<Multiplier> multipliers;
    public List<Distances> distances;
	public List<long> deletedBrains;
	public AnsEquation NP;
	public long calcTime;
}
