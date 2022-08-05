using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeleteSingleNetworkBrainResponse
{
	public int statusCode;
	public List<AnsEquations> ansEquations;
	public List<Distances> distances;
	public List<int> deletedBrains;
	public AnsEquation NP;
	public int calcTime;
}
